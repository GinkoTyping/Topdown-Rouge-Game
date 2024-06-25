using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Utilities
{
    public class VectorHelper
    {
        public Vector3 GetNearestVector(Vector3 vector)
        {
            float angle = Vector2.SignedAngle(vector, Vector3.up);
            float angleToCalculate = angle + 22.5f;
            int remainder = Mathf.Clamp(Mathf.FloorToInt(angleToCalculate / 45), -4, 4);
            Quaternion rotation = Quaternion.Euler(0f, 0f, -remainder * 45f);
            return rotation * Vector3.up.normalized;
        }
    }
}