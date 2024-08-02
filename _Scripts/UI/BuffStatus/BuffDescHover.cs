using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffDescHover : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private RectTransform textRect;
    [SerializeField] private RectTransform backGoundRect;
    [SerializeField] private Vector2 padding;

    private RectTransform hoverRect;
    private BuffIcon buffIcon;

    private void Awake()
    {
        hoverRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (buffIcon == null || !buffIcon.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void Set(BuffIcon buffIcon)
    {
        this.buffIcon = buffIcon;

        textMesh.text = buffIcon.currentBuff.GetDesc();
        RectTransform buffIconRect = buffIcon.GetComponent<RectTransform>();

        hoverRect.localPosition = new Vector3(buffIconRect.localPosition.x, hoverRect.localPosition.y, 0);

        float width = textMesh.preferredWidth > 300.0f ? 300.0f : textMesh.preferredWidth;
        textRect.sizeDelta = new Vector2(width, textRect.sizeDelta.y);
        float height = textMesh.preferredHeight;

        textRect.sizeDelta = new Vector2(width, height);

        width += padding.x;
        height += padding.y;
        backGoundRect.sizeDelta = new Vector2(width, height);
        hoverRect.sizeDelta = new Vector2(width, height);
    }
}
