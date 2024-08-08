using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : CoreComponent
{
    public float continousAttackTime;
    public float startAttackTime;

    public float continousStillnessTime;
    public float startStillnessTime;

    public float continousMovingTime;
    public float startMovingTime;

    private bool isStaying = true;

    [SerializeField] public PoolManager buffsPool;
    [SerializeField] private List<Buff> defaultBuffList;
    [SerializeField] private float effectiveTime = 0.05f;

    public List<Buff> buffList;
    public event Action<int> OnBuffChange;

    public PlayerStats stats { get; private set; }
    public NormalAttack normalAttack { get; private set; }

    private Movement movement;
    private Death death;
    private Player player;
    private Timer effectiveTimer;

    private void Start()
    {
        player = Core.GetComponentInParent<Player>();
        normalAttack = Core.GetCoreComponent<NormalAttack>();
        movement = Core.GetCoreComponent<Movement>();

        effectiveTimer = new Timer(effectiveTime);
        effectiveTimer.OnTimerDone += HandleEffectiveTimerDone;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        Reset();

        stats = Core.GetCoreComponent<PlayerStats>();
        death = Core.GetCoreComponent<Death>();
        death.OnDeath += Reset;

        foreach (Buff buff in defaultBuffList)
        {
            Add(buff);
        }
    }

    private void OnDisable()
    {
        death.OnDeath -= Reset;
    }

    private void Reset()
    {
        buffList = new List<Buff>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0;i < buffsPool.transform.childCount; i++)
        {
            Destroy(buffsPool.transform.GetChild(i).gameObject);
        }
    }

    public void Add(Buff buff, BaseBuffDataSO buffData = null)
    {
        if (buff == null)
        {
            return;
        }

        Buff exsitBuff = buffList.Where(item => item.gameObject.name == buff.gameObject.name).FirstOrDefault();
        if (exsitBuff != null)
        {
            if (buff.data == null && buffData == null)
            {
                Debug.LogError($"{buff.gameObject.name}: Empty Buff data");
            } else
            {
                exsitBuff.UpdateBuffData(
                    buffData == null 
                    ? buff.data 
                    : buffData);
            }
        }
        else
        {
            GameObject newBuffGO = Instantiate(buff.gameObject, transform);
            newBuffGO.name = buff.name;
            Buff newBuff = newBuffGO.GetComponent<Buff>();

            if (buffData != null)
            {
                newBuff.UpdateBuffData(buffData);
            }

            buffList.Add(newBuff);
            newBuff.Init();
            OnBuffChange?.Invoke(buffList.Count);
        }
    }

    public void RegisterBuff(Buff buff)
    {
        buffList.Add(buff);
        OnBuffChange?.Invoke(buffList.Count);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        UpdateContinousAttackTime();
        UpdateMoveAndStayTime();
        UpdateBuffs();
    }

    private void UpdateBuffs()
    {
        foreach (Buff buff in buffList)
        {
            buff.LogicUpdate();
        }
    }

    private void UpdateContinousAttackTime()
    {
        if (player != null && player.IsAttackInput)
        {
            if (continousAttackTime == 0)
            {
                startAttackTime = Time.time;
            }

            continousAttackTime += Time.deltaTime;
        }
        else if (continousAttackTime != 0)
        {
            continousAttackTime = 0;
        }
    }

    private void UpdateMoveAndStayTime()
    {
        effectiveTimer.Tick();

        if (isStaying)
        {
            if (movement.CurrentVelocity.sqrMagnitude > 0)
            {
                isStaying = false;
            }
        } 
        else if (!isStaying && movement.CurrentVelocity.sqrMagnitude == 0 &&!effectiveTimer.isActive)
        {
            effectiveTimer.StartTimer();
        }

        if (isStaying)
        {
            if (continousStillnessTime == 0)
            {
                startStillnessTime = Time.time;
            }
            continousStillnessTime += Time.deltaTime;
            continousMovingTime = 0;
        } 
        else
        {
            if (continousMovingTime == 0)
            {
                startMovingTime = Time.time;
            }
            continousMovingTime += Time.deltaTime;
            continousStillnessTime = 0;
        }
    }

    private void HandleEffectiveTimerDone()
    {
        isStaying = movement.CurrentVelocity.sqrMagnitude == 0;
    }

    public float GetContinousAttackTime(float startTime)
    {
        if (startTime > startAttackTime)
        {
            return continousAttackTime - (startTime - startAttackTime);
        }
        return continousAttackTime;
    }

    public float GetContinousTime(ChargingBaseType type, float validStart)
    {
        float time;
        float startTime;
        if (type == ChargingBaseType.ContinousAttack)
        {
            time = continousAttackTime;
            startTime = startAttackTime;
        } else if (type == ChargingBaseType.Stillness)
        {
            time = continousStillnessTime;
            startTime = startStillnessTime;
        }
        else
        {
            time = continousMovingTime;
            startTime = startMovingTime;
        }

        if (validStart > startTime)
        {
            return time - (validStart - startTime);
        }

        return time;
    }
}
