using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Interactable : CoreComponent, IInteractable
    {
        [SerializeField]
        public float interactingTime;
        [SerializeField]
        public Vector3 interactionIconOffset;
        [SerializeField]
        public Sprite spriteOnInteractEnd;
        [SerializeField]
        public AudioClip audioOnSearch;
        [SerializeField]
        public AudioClip audioOnOpen;
        [SerializeField]
        public InteractType InteractType; 
        [SerializeField]
        public string HintText;

        public string keyboardText
        {
            get => "E";
        }
        public string controllText
        {
            get => "Y";
        }

        public Vector2 interactionIconPos { get; private set; }
        public float loadingTime { get; private set; }
        public bool isInteractive { get; private set; }
        public InteractType interactType { get; private set; }
        public string hintText { get; private set; }

        private Interaction interactionComp;
        private SpriteRenderer spriteRenderer;
        private LootsRespawning lootsRespawning;


        private bool hasOpened;

        protected override void Awake()
        {
            base.Awake();

            interactionIconPos = transform.position + interactionIconOffset;
            loadingTime = interactingTime;
            hintText = HintText;

            spriteRenderer = GetComponentInParent<SpriteRenderer>();
            lootsRespawning = Core.GetCoreComponent<LootsRespawning>();

            isInteractive = true;
        }

        private void OpenChest()
        {
            if (hasOpened)
            {

            } else
            {
                hasOpened = true;
                spriteRenderer.sprite = spriteOnInteractEnd;
            }

            interactionComp.loopBarPrefab.OnLoadingEnd -= OpenChest;
            interactionComp.loopBarPrefab.OnLoadingEnd -= lootsRespawning.OnLoots;

            SoundManager.Instance.StopSound();
            SoundManager.Instance.PlaySound(audioOnOpen);
        }

        public void Interact(Interaction comp)
        {
            if (isInteractive)
            {
                interactionComp = comp;

                comp.loopBarPrefab.OnLoadingEnd += OpenChest;
                comp.loopBarPrefab.OnLoadingEnd += lootsRespawning.OnLoots;

                SoundManager.Instance.PlaySound(audioOnSearch);
            }
        }
    }
}