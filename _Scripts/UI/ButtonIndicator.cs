using TMPro;
using UnityEngine;


public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI keyText;
    [SerializeField]
    private TextMeshProUGUI hintText;

    private Transform container;


    private void Awake()
    {
        container = GameObject.Find("GameplayUI").transform;
    }
    public void Set(Vector3 position, string key, string hint)
    {
        GetComponent<RectTransform>().SetParent(container);

        GetComponent<RectTransform>().localScale = Vector3.one;
        transform.position = position;

        keyText.text = key; 
        hintText.text = hint;
    }
}
