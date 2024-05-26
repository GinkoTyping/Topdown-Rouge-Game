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


        public Vector2 interactionIconPos { get; private set; }
        public float loadingTime { get; private set; }
        public bool isInteractive { get; private set; }
        public InteractType interactType { get; private set; }

        private Interaction interactionComp;
        private SpriteRenderer spriteRenderer;
        private LootsRespawning lootsRespawning;

        private bool hasOpened;

        protected override void Awake()
        {
            base.Awake();

            interactionIconPos = transform.position + interactionIconOffset;
            loadingTime = interactingTime;

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

            interactionComp.loopBar.OnLoadingEnd -= OpenChest;
            interactionComp.loopBar.OnLoadingEnd -= lootsRespawning.OnLoots;

            SoundManager.Instance.StopSound();
            SoundManager.Instance.PlaySound(audioOnOpen);
        }

        public void Interact(Interaction comp)
        {
            if (isInteractive)
            {
                interactionComp = comp;

                comp.loopBar.OnLoadingEnd += OpenChest;
                interactionComp.loopBar.OnLoadingEnd += lootsRespawning.OnLoots;

                SoundManager.Instance.PlaySound(audioOnSearch);
            }
        }
    }
}