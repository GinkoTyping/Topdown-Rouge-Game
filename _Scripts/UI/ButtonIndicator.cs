using TMPro;
using UnityEngine;


public class ButtonIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI keyText;
    [SerializeField]
    private TextMeshProUGUI hintText;

    public void Set(Vector3 position, string key, string hint)
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
        transform.position = position;

        keyText.text = key; 
        hintText.text = hint;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
