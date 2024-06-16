using Ginko.Data;
using Ginko.StateMachineSystem;
using Ginko.Weapons;
using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Ginko.EnemySystem
{
    public abstract class Enemy : Entity
    {
        public Action OnObstaclesCollision;
        public AnimationEventHandler AnimationEventHandler { get; private set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Obstacles")
            {
                OnObstaclesCollision?.Invoke();
            }
        }
    }
}
