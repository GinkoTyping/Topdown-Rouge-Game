using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private Buff currentBuff;

    public void SetPool(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void Set(Buff buff)
    {
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
        if (currentBuff.currenrStack == 0)
        {
            poolManager.Pool.Release(gameObject);
        } 
        else if (stackText.text != currentBuff.currenrStack.ToString())
        {
            stackGO.SetActive(true);
            stackText.text = currentBuff.currenrStack.ToString();
        }
    }

    private void OnDisable()
    {
        currentBuff = null;
        stackGO.SetActive(false);
    }
}
