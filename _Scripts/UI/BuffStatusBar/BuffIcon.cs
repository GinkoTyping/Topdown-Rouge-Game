using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image buffImage;
    [SerializeField] private GameObject stackGO;
    [SerializeField] private TextMeshProUGUI stackText;
    [SerializeField] private GameObject timerGO;
    [SerializeField] private TextMeshProUGUI timerText;

    [HideInInspector] public PoolManager poolManager;
    public Buff currentBuff;

    private BuffsContainer buffsContainer;

    public void SetPool(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void Set(Buff buff)
    {
        buffsContainer = GetComponentInParent<BuffsContainer>();

        currentBuff = buff;
        buffImage.sprite = buff.data.iconSprite;
        stackGO.SetActive(buff.data.stackable);
        timerGO.SetActive(buff.data.hasTimer);
    }

    private void Update()
    {
        UpdateTimerText();

        if (currentBuff != null)
        {
            if (currentBuff.data.stackable)
            {
                UpdateStackableBuff();
            }
        }
    }

    private void UpdateTimerText()
    {
        if (currentBuff == null)
        {
            return;
        }

        if (currentBuff.buffTimer < 1 && timerGO.activeSelf)
        {
            timerGO.SetActive(false);
        }
        else if (timerGO.activeSelf)
        {
            timerText.text = Mathf.Round(currentBuff.buffTimer).ToString();
        }
    }

    private void UpdateStackableBuff()
    {
        if (currentBuff.currentStack == 0)
        {
            poolManager.Pool.Release(gameObject);
        } 
        else if (stackText.text != currentBuff.currentStack.ToString())
        {
            stackGO.SetActive(true);
            stackText.text = currentBuff.currentStack.ToString();
        }
    }

    private void OnDisable()
    {
        currentBuff = null;
        stackGO.SetActive(false);
    }

    public void HandleMouseHover()
    {
        buffsContainer.SetHoverBuffIcon(this);
    }

    public void HandleMouseExist()
    {
        buffsContainer.SetHoverBuffIcon(null);
    }
}
