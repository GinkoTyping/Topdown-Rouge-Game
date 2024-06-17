using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Ginko.Data
{
    [CreateAssetMenu(fileName = "newEnemyBasicDataData", menuName = "Data/Enemy/Basic Data")]
    public class EnemyBasicDataSO : EntityDataSO
    {
        [Header("Idle State")]
        public float idleTimeDuration;

        [Header("Detected State")]
        public GameObject AlertIcon;
        public float chaseVelocity;

        [Header("Attack State")]
        public float attackDistance;
        public float attackChargeTime;

    }
}
