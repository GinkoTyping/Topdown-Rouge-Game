using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsBox : MonoBehaviour
{
    [SerializeField]
    private GameObject optionButton;
    [SerializeField]
    private Sprite activeSprite; 
    [SerializeField]
    private Sprite inActivedSprite;

    private Transform container;

    private InteractOptionItem[] options;
    private int currentIndex = -1;

    private void Awake()
    {
        container = GameObject.Find("GameplayUI").transform;
    }

    public void Set(Collider2D[] options, Vector3 position)
    {
        Set(CollidersToOptionItems(options), position);
    }

    public void Set(InteractOptionItem[] options, Vector3 position)
    {
        transform.SetParent(container);
        transform.position = position;

        this.options = options;
        currentIndex = 0;

        if (options.Length > 1)
        {
            for (int i = 0; i < options.Length; i++)
            {
                GameObject option;
                if (i >= 1)
                {
                    option = Instantiate(optionButton, transform);
                    option.GetComponent<RectTransform>().localPosition = new Vector3(0, -i, 0);
                    option.GetComponent<Image>().sprite = inActivedSprite;
                } else
                {
                    option = GetComponentInChildren<Button>().gameObject;
                    option.GetComponent<Image>().sprite = activeSprite;
                }

                option.GetComponentInChildren<TextMeshProUGUI>().text = options[i].OptionName;
            }
        }
    }

    public InteractOptionItem[] CollidersToOptionItems(Collider2D[] colliders)
    {
        List<InteractOptionItem> interactOptionItems = new List<InteractOptionItem>();

        foreach (Collider2D collider in colliders)
        {
            InventoryItem inventoryItem = collider.GetComponentInChildren<InventoryItem>(true);
            if (inventoryItem != null)
            {
                interactOptionItems.Add(new InteractOptionItem(
                        collider.GetComponent<IInteractable>(),
                        inventoryItem.data.itemName)
                    );
            } else
            {
                Debug.LogError("Unmatch option type");
            }
        }

        return interactOptionItems.ToArray();
    }
}

public class InteractOptionItem
{
    public IInteractable IInteractable;
    public string OptionName;

    public InteractOptionItem(IInteractable interactable, string name)
    {
        IInteractable = interactable;
        OptionName = name;
    }
}
