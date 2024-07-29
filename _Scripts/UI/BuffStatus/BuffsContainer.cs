using Ginko.PlayerSystem;
using UnityEngine;

public class BuffsContainer : MonoBehaviour
{
    private RectTransform rect;
    private int buffCount = 0;

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
            buffCount = count;
            rect.sizeDelta = new Vector2(count * 30 + (count - 1) * 6, rect.sizeDelta.y);
        }
    }
}
