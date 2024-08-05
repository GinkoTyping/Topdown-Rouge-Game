using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceItemIndicator : MonoBehaviour
{
    [SerializeField]
    private Color safeColor;
    [SerializeField]
    private Color errorColor;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SwitchStatus(bool isOK)
    {
        image.color = isOK ? safeColor : errorColor;
    }
}
