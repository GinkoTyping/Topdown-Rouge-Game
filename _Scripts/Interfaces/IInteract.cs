using Ginko.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    Vector2 interactionIconPos {  get; }
    float loadingTime {  get; }
    void Interact(Interaction interactionComp);
}
