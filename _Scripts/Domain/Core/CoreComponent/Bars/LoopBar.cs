using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class LoopBar : MonoBehaviour
{
    public event Action OnLoadingEnd;

    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI textMesh;

    private float loadingTimeAll;
    private float loadingTimeLeft;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingTimeAll > 0 && loadingTimeLeft > 0)
        {
            loadingTimeLeft -= Time.deltaTime;
            spriteRenderer.material.SetFloat("_Progress", 1 - loadingTimeLeft / loadingTimeAll);
            textMesh.text = loadingTimeLeft.ToString("f2");
        }
        else if (loadingTimeLeft <= 0)
        {
            SetBar(0);
            OnLoadingEnd?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void SetBar(float time = 0)
    {
        loadingTimeAll = time;
        loadingTimeLeft = time;
    }

    public void SetBar(float time, Vector3 position)
    {
        SetBar(time);

        if (time > 0)
        {
            spriteRenderer.sortingLayerName = "Status Bar";
        }
        else
        {
            spriteRenderer.sortingLayerName = "Default";
        }
        
        if (position != null)
        {
            transform.position = position;
        }
    }
}
