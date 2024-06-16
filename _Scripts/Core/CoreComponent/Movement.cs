using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Ginko.CoreSystem
{
    public class Movement: CoreComponent
    {
        public Rigidbody2D RB {  get; private set; }

        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }

        private Vector2 workSpace;

        protected override void Awake()
        {
            base.Awake();

            FacingDirection = 1;
            RB = GetComponentInParent<Rigidbody2D>();
        }

        public override void LogicUpdate()
        {
            CurrentVelocity = RB.velocity;
        }
        
        public void SetVelocity(float amount, Vector2 direction)
        {
            Vector2 normalizedDirection = direction.normalized;
            workSpace.Set(normalizedDirection.x * amount, normalizedDirection.y * amount);
            RB.velocity = workSpace;
            CurrentVelocity = workSpace;

            CheckIfShouldFlip();
        }
        public void SetVelocityZero()
        {
            workSpace.Set(0, 0);
            RB.velocity = workSpace;
            CurrentVelocity = workSpace;
        }

        public void MoveByPathList(List<Vector2> pathLeftToGo, float speed)
        {
            if (pathLeftToGo.Count > 0) //if the target is not yet reached
            {
                Vector3 dir = (Vector3)pathLeftToGo[0] - RB.transform.position;

                SetVelocity(speed, dir.normalized);
            }
        }

        public void Flip()
        {
            FacingDirection *= -1;
            RB.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
        public void CheckIfShouldFlip()
        {
            if (CurrentVelocity.x != 0 && CurrentVelocity.x * FacingDirection < 0)
            {
                Flip();
            }
        }

        public void FaceToItem(Transform itemTransform)
        {
            if (transform.position.x < itemTransform.position.x)
            {
                if (FacingDirection < 0)
                {
                    Flip();
                }
            } else
            {
                if (FacingDirection > 0)
                {
                    Flip();
                }
            }
        }
    }
}
