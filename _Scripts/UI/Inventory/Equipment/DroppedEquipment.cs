using Ginko.PlayerSystem;
using UnityEngine;

public class DroppedEquipment : MonoBehaviour
{
    [SerializeField]
    private LineRenderer laserLineRenderer;
    [SerializeField]
    private ParticleSystem[] laserStartParticles;

    private DroppedItemController droppedItemController;
    private LayerMask dropsLayer;
    private Material[] laserMaterials;

    private static Vector2 PADDING_SIZE = new Vector2(.5f, .5f);
    private static int SAFE_RECURSION_COUNT = 50;

    public void Set(InventoryItem item)
    {
        SetReference();

        transform.position = GetPositionToDrop(Player.Instance.transform.position, 0);
        SetColor(item.rarity);

        item.GetComponent<RectTransform>().SetParent(transform);
        item.gameObject.SetActive(false);
    }

    private Vector3 GetPositionToDrop(Vector3 defaultPos, int startIndex)
    {
        if (startIndex >= SAFE_RECURSION_COUNT)
        {
            Debug.LogError("Stack Error");
            return Vector3.zero;
        }
        if (startIndex == 0)
        {
            bool IsEmpty = Physics2D.OverlapBoxAll(defaultPos, PADDING_SIZE, 0, dropsLayer).Length == 0;

            if (IsEmpty)
            {
                return defaultPos;
            } else
            {
                return GetPositionToDrop(defaultPos, startIndex + 1);
            }
        }

        for(int x = -startIndex; x <= startIndex; x++)
        {
            for (int y = -startIndex; y <= startIndex; y++)
            {
                if (Mathf.Abs(x) != startIndex && Mathf.Abs(y) != startIndex)
                {
                    continue;
                }
                Vector3 pos = defaultPos + new Vector3(PADDING_SIZE.x * x, PADDING_SIZE.y * y);
                bool IsEmpty = Physics2D.OverlapBox(pos, PADDING_SIZE, 0, dropsLayer) == null;

                if (IsEmpty)
                {
                    return pos;
                }
            }
        }

        return GetPositionToDrop(defaultPos, startIndex + 1);
    }
    
    private void SetReference()
    {
        droppedItemController = GetComponentInParent<DroppedItemController>();
        dropsLayer = droppedItemController.dropsLayer;
        laserMaterials = droppedItemController.laserMaterials;
    }

    private void SetColor(Rarity rarity)
    {
        laserLineRenderer.materials = new Material[] { laserMaterials[(int)rarity] };

        foreach(ParticleSystem particle in laserStartParticles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.startColor = laserLineRenderer.material.GetColor("_Color");
        }
    }
}
