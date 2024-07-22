using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    private PoolManager poolManager;

    private Image buffImage;
    private GameObject stackGO;
    private TextMeshProUGUI textMeshProUGUI;

    private Buff currentBuff;

    private void OnEnable()
    {
        buffImage = transform.Find("Icon").GetComponent<Image>();
        stackGO = transform.Find("Stack").gameObject;

        textMeshProUGUI = stackGO.transform.Find("StackText").GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = "0";
    }

    public void SetPool(PoolManager poolManager)
    {
        this.poolManager = poolManager;
    }

    public void Set(Buff buff)
    {
        currentBuff = buff;
        buffImage.sprite = buff.data.iconSprite;
    }

    private void Update()
    {
        if (currentBuff != null)
        {
            if (currentBuff.data.stackable)
            {
                UpdateStackableBuff();
            }
        }
    }

    private void UpdateStackableBuff()
    {
        if (currentBuff.currenrStack == 0)
        {
            poolManager.Pool.Release(gameObject);
        } 
        else if (textMeshProUGUI.text != currentBuff.currenrStack.ToString())
        {
            stackGO.SetActive(true);
            textMeshProUGUI.text = currentBuff.currenrStack.ToString();
        }
    }

    private void OnDisable()
    {
        currentBuff = null;
        stackGO.SetActive(false);
    }
}
