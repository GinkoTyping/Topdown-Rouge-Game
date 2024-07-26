using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
    [SerializeField] private bool loop;
    [SerializeField] private float aliveTime;
    [SerializeField] private Vector3 endPositionOffset;
    [Header("Realse")]
    [SerializeField] private float fadeTimeTotal;
    [SerializeField] private Vector3 fadeOffset;
    
    private float fadeTime;

    private bool isActive;
    private bool isRealsing;
    private float passedTime = 0;
    private Vector3 startPos;
    private Vector3 aliveTargetPos;
    private Vector3 fadeTargetPos;

    private PoolManager poolManager;
    private TextMeshProUGUI textMeshPro;
    private RectTransform rect;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        passedTime = 0;
        fadeTime = 0;
        isActive = false;
        isRealsing = false;
    }

    private void Update()
    {
        if (isActive)
        {
            Appearing();
        }
        else if (isRealsing)
        {
            Fading();
        }
    }

    public void Init(PoolManager poolManager, Vector3 position, string content, Color color)
    {
        this.poolManager = poolManager;

        rect.anchoredPosition = position;
        startPos = position;
        aliveTargetPos = position + endPositionOffset;
        fadeTargetPos = aliveTargetPos + fadeOffset;

        textMeshPro.text = content;
        textMeshPro.color = color;

        isActive = true;
    }

    public void Appearing()
    {
        if (passedTime >= aliveTime)
        {
            if (loop)
            {
                passedTime = 0;
                transform.position = startPos;
            }
            else
            {
                isActive = false;
                isRealsing = true;
            }
        }
        else
        {
            Vector2 newPos = Vector2.LerpUnclamped(rect.anchoredPosition, aliveTargetPos, passedTime / aliveTime);
            rect.anchoredPosition = newPos;

            passedTime += Time.deltaTime;
        }
    }

    public void Fading()
    {
        if (fadeTime >= fadeTimeTotal)
        {
            poolManager.Pool.Release(gameObject);
        }
        else
        {
            Vector2 newPos = Vector2.LerpUnclamped(rect.anchoredPosition, fadeTargetPos, fadeTime / fadeTimeTotal);
            rect.anchoredPosition = newPos;
            Debug.Log(rect.anchoredPosition);
            Color color = textMeshPro.color;
            color.a = Mathf.LerpUnclamped(color.a, 0, fadeTime / fadeTimeTotal);
            textMeshPro.color = color;

            fadeTime += Time.deltaTime;
        }
    }
}
