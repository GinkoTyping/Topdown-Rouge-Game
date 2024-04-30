using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ginko.Data
{
    [CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity/Basic Data")]
    public class EntityDataSO : ScriptableObject
    {
        [SerializeField]
        public float moveVelocity;

        public float dashVelocity;
        public float dashDuaration;
    }
}
