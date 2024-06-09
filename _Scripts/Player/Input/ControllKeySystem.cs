using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllKeySystem : MonoBehaviour
{
    public keyMap keyboradMap;
    public keyMap controllerMap;

    private void Awake()
    {
        keyboradMap.Interact = "E";
        controllerMap.Interact = "Y";
    }
}

public struct keyMap
{
    public string Interact;
}

