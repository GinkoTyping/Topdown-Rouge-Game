using Ginko.PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.UIElements;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Interaction : CoreComponent
    {
        [SerializeField]
        public LoopBar loopBar;
        [SerializeField]
        public GameObject textMesh;
        [SerializeField]
        public string interactionText;

        public IInteractable currentInteractingItem;

        private Detections detections;
        private GameObject interactTextGO;
        private bool isShowInteractHint;
        private bool isInteracting;

        private GameObject InventoryUI;

        protected override void Awake()
        {
            base.Awake();

            isShowInteractHint = false;
            isInteracting = false;

            loopBar = GameObject.FindGameObjectWithTag("HintContainer").GetComponentInChildren<LoopBar>();
            detections = Core.GetCoreComponent<Detections>();
            InventoryUI = GameObject.Find("Inventory").gameObject;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        private void SwitchInteractHint(bool isShow)
        {
            // TODO: 是否需要对象池来创建字体？
            if (isShow) {
                isShowInteractHint = true;
                // TODO: 同时检测到多个可交互物品时，怎么处理？
                if (detections.interactiveObjects?.Length == 1)
                {
                    currentInteractingItem = detections.interactiveObjects[0].GetComponentInChildren<IInteractable>();
                    interactTextGO = Instantiate(textMesh, currentInteractingItem.interactionIconPos, Quaternion.identity);
                    TextMeshProUGUI meshGO = interactTextGO.GetComponentInChildren<TextMeshProUGUI>();
                    meshGO.text = interactionText;
                }
            } else
            {
                isShowInteractHint = false;
                Destroy(interactTextGO);
            }
        }

        private void InitLoopBar()
        {
            loopBar.gameObject.SetActive(true);
            loopBar.SetBar(currentInteractingItem.loadingTime, currentInteractingItem.interactionIconPos);
        }

        public void CheckIfShowInteractHint()
        {
            if (detections.IsAbleToInteract && !isShowInteractHint && !isInteracting)
            {
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
            loopBar.OnLoadingEnd -= OnInteractEnd;
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

                loopBar.OnLoadingEnd += OnInteractEnd;

                currentInteractingItem.Interact(this);
            }
        }

        public void HandleLootable()
        {
            if (currentInteractingItem.interactType == InteractType.Lootable)
            {
                InventoryUI.SetActive(true);
                Player.Instance.InputAction.SwitchCurrentActionMap("UI");
            }
        }
    }
}