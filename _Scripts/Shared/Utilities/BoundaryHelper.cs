using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHelper : MonoBehaviour
{
    [SerializeField] public Vector3 rectSize;
    [SerializeField] public float radius;
    [Header("Debug")]
    [SerializeField] private bool isDebug;
    [SerializeField] private Color color;

    private void OnDrawGizmos()
    {
        if (isDebug)
        {
            Gizmos.color = color;

            if (rectSize != Vector3.zero)
            {
                Gizmos.DrawWireCube(transform.position, rectSize);
            }
            else if (radius != 0)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}
