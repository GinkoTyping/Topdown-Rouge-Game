using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField]
    public Color hidingcolor;
    [SerializeField]
    public Color defaultColor;

    private SpriteRenderer[] spriteRenderers;

    // TODO: spriteRenderers ��ȡ��ʽ���󣻸÷���Ӧ�÷��� SpriteEffect �� ����
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
