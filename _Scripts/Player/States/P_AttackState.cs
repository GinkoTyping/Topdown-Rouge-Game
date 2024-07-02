using Ginko.CoreSystem;
using Ginko.PlayerSystem;
using Ginko.Weapons;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Ginko.StateMachineSystem
{
    public class P_AttackState : AttackState
    {
        private Player player;
        private AbilityManager abilityManager;

        private AttributeStat attackInterval;

        public P_AttackState(Entity entity, FiniteStateMachine stateMachine) : base(entity, stateMachine)
        {
            player = (Player)entity;
            
            abilityManager = entity.Core.GetCoreComponent<AbilityManager>();
        }
        
        public override void Enter()
        {
            base.Enter();

            attackInterval = Entity.Core.GetCoreComponent<PlayerStats>().GetAttribute(AttributeType.AttackInterval);

            abilityManager.SetCooldown(attackInterval.CurrentValue);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            abilityManager.CheckIfAttack();

            if (!player.IsAttackInput)
            {
                if (player.MoveDirection == Vector2.zero)
                {
                    StateMachine.ChangeState(Entity.IdleState);
                }
                else
                {
                    StateMachine.ChangeState(Entity.MoveState);
                }
            }
        }

        protected override void SetAnimBoolName()
        {
            AnimBoolName = AnimBoolName.RangedAttack;
        }
    }
}
