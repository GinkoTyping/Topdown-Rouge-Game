using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosHelper : MonoBehaviour
{
    [SerializeField] bool showSite;
    private void OnDrawGizmos()
    {
        if (showSite)
        {
            Gizmos.DrawWireCube(new Vector2(16, 9), new Vector2(32, 18));
        }
    }
}
