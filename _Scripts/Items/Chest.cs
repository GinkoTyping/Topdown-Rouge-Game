using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Chest : MonoBehaviour, IInteract
{
    [SerializeField]
    public float openingTime;
    [SerializeField]
    public Vector3 interactionIconOffset;
    [SerializeField]
    public Sprite openedChectSprite;
    [SerializeField]
    public AudioClip audioOnSearch;
    [SerializeField]
    public AudioClip audioOnOpen;


    public Vector2 interactionIconPos { get; private set; }
    public float loadingTime { get; private set; }
    public bool isInteractive { get; private set; }

    private Interaction interactionComp;
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        interactionIconPos = transform.position + interactionIconOffset;
        loadingTime = openingTime;

        spriteRenderer = GetComponent<SpriteRenderer>();
        isInteractive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenChest()
    {
        isInteractive = false;

        spriteRenderer.sprite = openedChectSprite;
        interactionComp.loopBar.OnLoadingEnd -= OpenChest;

        SoundManager.Instance.StopSound();
        SoundManager.Instance.PlaySound(audioOnOpen);
    }

    public void Interact(Interaction comp)
    {
        if (isInteractive)
        {
            interactionComp = comp;
            comp.loopBar.OnLoadingEnd += OpenChest;
            SoundManager.Instance.PlaySound(audioOnSearch);
        }
    }
}
