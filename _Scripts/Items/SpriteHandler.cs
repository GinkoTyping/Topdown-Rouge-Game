using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField]
    public Color hidingcolor;
    [SerializeField]
    public Color defaultColor;

    private SpriteRenderer[] spriteRenderers;

    // TODO: spriteRenderers 获取方式错误；该方法应该放在 SpriteEffect 类 里面
    private void Awake()
    {
        spriteRenderers = transform.parent.GetComponentsInChildren<SpriteRenderer>();
    }
    public void HideSprite(bool isHide)
    {
        
        foreach (var spriteRender in spriteRenderers)
        {
            if (spriteRender.gameObject.name != "Shadow")
            {
                spriteRender.color = isHide ? hidingcolor : defaultColor;
            }
        }
    }
}
