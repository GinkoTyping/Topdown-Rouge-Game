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

    public void Init(PoolManager poolManager, Vector3 position, string content, Color color, FontStyles fontStyles = FontStyles.Bold, float fontSize = 0, bool flipped = false)
    {
        this.poolManager = poolManager;

        Vector3 calculatePos = position;
        if (flipped)
        {
            calculatePos = new Vector3(-position.x, position.y);
        }

        rect.anchoredPosition = calculatePos;
        startPos = calculatePos;
        aliveTargetPos = calculatePos + endPositionOffset;
        fadeTargetPos = aliveTargetPos + fadeOffset;

        textMeshPro.text = content;
        textMeshPro.alignment = flipped ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
        textMeshPro.color = color;
        textMeshPro.fontStyle = fontStyles;
        if (fontSize > 0)
        {
            textMeshPro.fontSize = fontSize;
        }

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
            Color color = textMeshPro.color;
            color.a = Mathf.LerpUnclamped(color.a, 0, fadeTime / fadeTimeTotal);
            textMeshPro.color = color;

            fadeTime += Time.deltaTime;
        }
    }
}
