using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingItem : MonoBehaviour
{
    [SerializeField]
    private AudioClip searchAudio;

    public event Action OnSearchingDone;
    public bool needSearch {  get; private set; }
    public bool isSearching { get; private set; }


    private PoolManager searchingUIPool;
    private GameObject searchUI;
    private GameObject spinner;

    private float totalTime;
    private float startTime;

    private static float SCALE_SIZE = .8f;

    private void Awake()
    {
        needSearch = false;
        isSearching = false;
    }

    private void Update()
    {
        CheckSwitchSearchingUI();
    }

    public void Set(InventoryItem item)
    {
        needSearch = true;

        CreateUI(item);
        SetSearchingTimeByRarity(item.rarity);
    }

    public void StartSearch()
    {
        if (needSearch)
        {
            isSearching = true;
            startTime = Time.time;
        }
    }

    private void CreateUI(InventoryItem item)
    {
        searchingUIPool = GameObject.Find("UI").transform.Find("Inventory").Find("Loot").GetComponentInChildren<PoolManager>();
        RectTransform selfRect = GetComponent<RectTransform>();


        searchUI = searchingUIPool.Pool.Get();
        RectTransform rect = searchUI.GetComponent<RectTransform>();
        spinner = rect.Find("Spinner").gameObject;

        rect.position = item.transform.position;
        rect.localScale = Vector3.one * selfRect.sizeDelta.x * SCALE_SIZE;
    }

    private void SetSearchingTimeByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                totalTime = 1.0f;
                break;
            case Rarity.Uncommon:
                totalTime = 1.5f;
                break;
            case Rarity.Rare:
                totalTime = 2.0f;
                break;
            case Rarity.Legend:
                totalTime = 4.0f;
                break;
        }
    }

    private void CheckSwitchSearchingUI()
    {
        if (isSearching && Time.time < startTime + totalTime)
        {
            if (!spinner.activeSelf)
            {
                spinner.SetActive(true);
                SoundManager.Instance.PlaySound(searchAudio);
            }
        }
        else if (isSearching 
            && Time.time > startTime + totalTime
            && searchUI.activeSelf)
        {
            totalTime = 0;
            startTime = 0;
            isSearching = false;
            needSearch = false;

            ClearSpinner();

            OnSearchingDone?.Invoke();
        }
    }

    public void ClearSpinner()
    {
        spinner.SetActive(false);
        SoundManager.Instance.StopSound();
        searchingUIPool.Pool.Release(searchUI);
    }
}
