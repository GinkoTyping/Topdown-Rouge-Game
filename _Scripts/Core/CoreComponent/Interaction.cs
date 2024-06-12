using Ginko.PlayerSystem;
using TMPro;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Interaction : CoreComponent
    {
        [SerializeField]
        public GameObject interactHintPrefab;
        [SerializeField]
        private GameObject optionBoxPrefab;
        [SerializeField]
        private PoolManager indicatorPool;

        private static Vector3 SWITCH_HINT_BUTTON_OFFSET = Vector3.up *  0.5f;

        public LoopBar loopBarPrefab { get; private set; }

        public IInteractable currentInteractingItem;

        private Detections detections;

        private GameObject interactHintGO;
        private GameObject switchHintGO;

        private bool isShowInteractHint;
        private bool isShowSwitchHint;
        private bool isInteracting;
        private int switchIndex = 0;
        private int switchAmount = 0;

        protected override void Awake()
        {
            base.Awake();

            isShowInteractHint = false;
            isInteracting = false;

            loopBarPrefab = GameObject.FindGameObjectWithTag("HintContainer").GetComponentInChildren<LoopBar>();
            detections = Core.GetCoreComponent<Detections>();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            detections.OnInteractionItemsChange += HandleInteractionItemChange;
        }

        private void OnDisable()
        {
            detections.OnInteractionItemsChange -= HandleInteractionItemChange;
        }

        private void HandleInteractionItemChange(Collider2D[] colliders)
        {
            switchIndex = 0;
            switchAmount = colliders.Length;
        }

        private void SwitchMultiItemHint(bool isShow)
        {
            if (isShow)
            {
                isShowSwitchHint = true;
                string connectWord = switchAmount > 2 ? "among" : "between";
                switchHintGO = indicatorPool.Pool.Get();
                switchHintGO.GetComponent<ButtonIndicator>()
                .Set(
                    (Vector3)currentInteractingItem.interactionIconPos + SWITCH_HINT_BUTTON_OFFSET,
                    "Y",
                    $"Switch {connectWord} {switchAmount}"
                );
            }
            else
            {
                isShowSwitchHint = false;

                indicatorPool.Pool.Release(switchHintGO);
                switchHintGO = null;
            }
        }

        private void SwitchInteractHint(bool isShow)
        {
            // TODO: 是否需要对象池来创建字体？
            if (isShow)
            {
                isShowInteractHint = true;

                interactHintGO = indicatorPool.Pool.Get();
                interactHintGO.GetComponent<ButtonIndicator>()
                    .Set(
                        currentInteractingItem.interactionIconPos,
                        currentInteractingItem.keyboardText,
                        currentInteractingItem.hintText
                    );
                if (switchHintGO != null)
                {
                    switchHintGO.GetComponent<ButtonIndicator>().SetPosition((Vector3)currentInteractingItem.interactionIconPos + SWITCH_HINT_BUTTON_OFFSET);
                }
            }
            else
            {
                isShowInteractHint = false;
                indicatorPool.Pool.Release(interactHintGO);
                interactHintGO = null;
            }
        }

        private void UpdateCurrrentItem()
        {
            if (detections.interactiveObjects?.Length == 1)
            {
                currentInteractingItem = detections.interactiveObjects[0].GetComponentInChildren<IInteractable>();
            } else
            {
                currentInteractingItem = null;
            }
        }

        private void InitLoopBar()
        {
            loopBarPrefab.gameObject.SetActive(true);
            loopBarPrefab.SetBar(currentInteractingItem.loadingTime, currentInteractingItem.interactionIconPos);
        }

        public void CheckIfShowInteractHint()
        {
            if (detections.interactiveObjects.Length > 1 && !isShowSwitchHint)
            {
                SwitchMultiItemHint(true);
            } else if (detections.interactiveObjects.Length <= 1 && isShowSwitchHint)
            {
                SwitchMultiItemHint(false);
            }

            if (detections.IsAbleToInteract && !isShowInteractHint && !isInteracting)
            {
                UpdateCurrrentItem();
                SwitchInteractHint(true);
            }
            else if (!detections.IsAbleToInteract && isShowInteractHint)
            {
                SwitchInteractHint(false);
            }
        }

        private void OnInteractEnd()
        {
            isInteracting = false;
            loopBarPrefab.OnLoadingEnd -= OnInteractEnd;
        }

        public void InteractItem()
        {
            if (currentInteractingItem == null)
            {
                return;
            }

            if (!isInteracting && currentInteractingItem.isInteractive)
            {
                isInteracting = true;

                SwitchInteractHint(false);

                InitLoopBar();

                loopBarPrefab.OnLoadingEnd += OnInteractEnd;

                currentInteractingItem.Interact(this);
            }
        }

        public void SwitchInteratItem()
        {
            switchIndex = switchIndex + 1 >= detections.interactiveObjects.Length 
                ? 0 
                : switchIndex + 1;
            currentInteractingItem = detections.interactiveObjects[switchIndex].GetComponentInChildren<IInteractable>();

            SwitchInteractHint(false);
            SwitchInteractHint(true);
        }
    }
}