using Ginko.PlayerSystem;
using Unity.VisualScripting;
using UnityEngine;

public class BuffsContainer : MonoBehaviour
{
    [SerializeField] private BuffDescHover descHover;

    private BuffIcon hoverBuffIcon;
    private RectTransform rect;
    private int buffCount = 0;
    private float iconWidth = 0;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        int count = GetComponentsInChildren<BuffIcon>().Length;
        HandleBuffChange(count);
    }

    private void HandleBuffChange(int count)
    {
        if (buffCount != count)
        {
            if (iconWidth == 0)
            {
                iconWidth = GetComponentInChildren<BuffIcon>().GetComponent<RectTransform>().sizeDelta.x;
            }

            buffCount = count;
            rect.sizeDelta = new Vector2(count * iconWidth + (count - 1) * iconWidth / 2, rect.sizeDelta.y);
        }
    }
    
    public void SetHoverBuffIcon(BuffIcon buffIcon)
    {
        hoverBuffIcon = buffIcon;
        if(hoverBuffIcon != null)
        {
            descHover.gameObject.SetActive(true);
            descHover.Set(buffIcon);
        } else
        {
            descHover.gameObject.SetActive(false);
        }
    }
}
