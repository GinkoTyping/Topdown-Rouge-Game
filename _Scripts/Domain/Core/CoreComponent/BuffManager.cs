using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : CoreComponent
{
    public float continousAttackTime = 0;
    public float continousStayingTime = 0;
    public float continousMovingTime = 0;
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

    public void Add(Buff buff)
    {
        if (buff == null)
        {
            return;
        }

        Buff exsitBuff = buffList.Where(item => item.gameObject.name == buff.gameObject.name).FirstOrDefault();
        if (exsitBuff != null)
        {
            if (buff.data == null)
            {
                Debug.LogError($"{buff.gameObject.name}: Empty Buff data");
            } else
            {
                exsitBuff.UpdateBuffData(buff.data);
            }
        }
        else
        {
            GameObject newBuff = Instantiate(buff.gameObject, transform);
            newBuff.name = buff.name;

            newBuff.GetComponent<Buff>().Init();
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
            continousStayingTime += Time.deltaTime;
            continousMovingTime = 0;
        } 
        else
        {
            continousMovingTime += Time.deltaTime;
            continousStayingTime = 0;
        }
    }

    private void HandleEffectiveTimerDone()
    {
        isStaying = movement.CurrentVelocity.sqrMagnitude == 0;
    }
}
