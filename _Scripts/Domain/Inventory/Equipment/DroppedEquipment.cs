using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using UnityEngine;
using static IInteractable;

public class DroppedEquipment : MonoBehaviour, IInteractable
{
    [SerializeField]
    private LineRenderer laserLineRenderer;
    [SerializeField]
    private ParticleSystem[] laserStartParticles;
    [SerializeField]
    private string HintText;

    [SerializeField]
    private Vector2 InteractionIconPos;

    public string keyboardText { get => "E"; }
    public string controllText { get => "Y"; }

    private DroppedItemController droppedItemController;
    private InventoryController inventoryController;
    private CommonPool poolManager;
    private AttributeHelper attributeHelper;

    private LayerMask dropsLayer;
    private Material[] laserMaterials;

    private static Vector2 PADDING_SIZE = new Vector2(.5f, .5f);
    private static int SAFE_RECURSION_COUNT = 50;

    #region interaction
    public Vector2 interactionIconPos { get; private set; }

    public float loadingTime {  get; private set; }

    public bool isInteractive { get; private set; }

    public InteractType interactType { get; private set; }

    public string hintText { get; private set; }

    #endregion

    private void Start()
    {
        inventoryController = GameObject.Find("Inventory").GetComponent<InventoryController>();
        poolManager = GetComponentInParent<CommonPool>();
    }
    public void Set(InventoryItem item, Vector3 postion)
    {
        SetReference();
        isInteractive = true;

        hintText = $"{HintText} (<#{attributeHelper.GetAttributeColor(item.rarity)}><u>{item.data.itemName}</u></color>)";
        interactionIconPos = postion + (Vector3)InteractionIconPos;

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
        attributeHelper = GameObject.Find("Helper").GetComponent<AttributeHelper>();

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

    public void Interact(Interaction comp)
    {
        InventoryItem item = GetComponentInChildren<InventoryItem>(true);

        Vector2Int? pos =  inventoryController.backpackInventory.GetSpaceToPlaceItem(item);
        if (pos != null)
        {
            isInteractive = false;
            poolManager.Pool.Release(gameObject);

            item.gameObject.SetActive(true);
        }
    }
}
