using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    Vector2 interactionIconPos {  get; }
    float loadingTime {  get; }
    bool isInteractive {  get; }
    InteractType interactType { get; }
    void Interact(Interaction interactionComp);
}

public enum InteractType {
    Lootable
}