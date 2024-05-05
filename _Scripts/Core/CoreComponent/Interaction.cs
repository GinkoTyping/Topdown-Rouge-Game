using Ginko.PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Interaction : CoreComponent
    {
        [SerializeField]
        public GameObject loopBar;
        [SerializeField]
        public GameObject textMesh;
        [SerializeField]
        public string interactionText;

        private Detections detections;
        private GameObject interactTextGO;
        private bool isShowInteractHint;

        protected override void Awake()
        {
            base.Awake();

            isShowInteractHint = false;
            detections = Core.GetCoreComponent<Detections>();
        }
        private void SwitchInteractHint(bool isShow)
        {
            // TODO: 是否需要对象池来创建字体？
            if (isShow) {
                isShowInteractHint = true;
                // TODO: 同时检测到多个可交互物品时，怎么处理？
                if (detections.interactiveObjects?.Length == 1)
                {
                    IInteract interactiveObject = detections.interactiveObjects[0].GetComponent<IInteract>();
                 interactTextGO = Instantiate(textMesh, interactiveObject.interactionIconPos, Quaternion.identity);
                    TextMeshProUGUI meshGO = interactTextGO.GetComponentInChildren<TextMeshProUGUI>();
                    meshGO.text = interactionText;
                }
            } else
            {
                isShowInteractHint = false;
                Destroy(interactTextGO);
            }
        }

        public void CheckIfShowInteractHint()
        {
            if (detections.IsAbleToInteract && isShowInteractHint == false)
            {
                SwitchInteractHint(true);
            }
            else if (!detections.IsAbleToInteract && isShowInteractHint)
            {
                SwitchInteractHint(false);
            }
        }
    }
}