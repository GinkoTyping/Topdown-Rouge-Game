using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteract
{
    [SerializeField]
    public Vector3 interactionIconOffset;
    public Vector2 interactionIconPos {  get; private set; }

    public void Interact()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        interactionIconPos = transform.position + interactionIconOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
