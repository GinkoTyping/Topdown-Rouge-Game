using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ginko.PlayerSystem;
using Ginko.CoreSystem;
using Ginko.Data;

namespace Ginko.StateMachineSystem
{
    public class E_HostileDetectedState : HostileDetectedState
    {
        private EnemyBasicDataSO EnemyBasicData;
        private List<Vector2> pathLeftToGo;
        private Vector3 playerPos;
        private GameObject alertIcon;
        private float chaseVelocity;

        public E_HostileDetectedState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            EnemyBasicData = (EnemyBasicDataSO)Entity.EntityData;
            chaseVelocity = EnemyBasicData.chaseVelocity > 0 ? EnemyBasicData.chaseVelocity : EnemyBasicData.moveVelocity * 1.25f;
        }

        public override void Enter()
        {
            base.Enter();

            InitMovementSetting();
            InitAlertIcon();
        }

        public override void Exit()
        {
            base.Exit();

            Entity.Anim.SetBool(AnimBoolName.Move.ToString(), false);
            Entity.Anim.SetBool(AnimBoolName.Chase.ToString(), false);
        }

        private void InitMovementSetting()
        {
            Entity.Movement.SetVelocityZero();

            if ((Player.Instance.transform.position.x < Entity.transform.position.x && Entity.Movement.FacingDirection > 0) || (Player.Instance.transform.position.x > Entity.transform.position.x && Entity.Movement.FacingDirection < 0))
            {
                Entity.Movement.Flip();
            }
        }

        private void InitAlertIcon()
        {
            GameObject redEye = Entity.gameObject.transform.Find("Red Eye")?.gameObject;
            if (redEye != null)
            {
                GameObject.Destroy(redEye);
            }
            alertIcon = GameObject.Instantiate(EnemyBasicData.AlertIcon, GetAlertIconPos(), Quaternion.identity);
        }

        private Vector3 GetAlertIconPos()
        {
            return Entity.transform.position + new Vector3(0, 1.2f, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            playerPos = Player.Instance.transform.position;
            if (alertIcon != null)
            {
                alertIcon.transform.position = GetAlertIconPos();
            }

            if (Entity.Detections.IsInMeleeAttackRange)
            {
                IsToMeleeAttackState = true;
                Entity.Movement.SetVelocityZero();
            } 
            else if (Entity.Detections.IsInRangedAttackRange && Entity.RangedAttackState != null)
            {
                IsToRangedAttackState = true;
            }
            else if (Entity.Detections.IsHostileDetected && !Entity.Detections.IsInRangedAttackRange)
            {
                pathLeftToGo = Entity.Pathfinding.GetMoveCommand(playerPos);

                Entity.Movement.MoveByPathList(pathLeftToGo, chaseVelocity);

                Entity.Anim.SetBool(AnimBoolName.HostileDetected.ToString(), false);
                Entity.Anim.SetBool(AnimBoolName.Chase.ToString(), true);
            } 
            else
            {
                IsToIdleState = true;
                Entity.Movement.SetVelocityZero();
            }
        }
    }
}