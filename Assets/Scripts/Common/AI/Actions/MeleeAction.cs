using Common.AI.Config;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Common.AI.Actions
{
    public class MeleeAction : ActionBase<AttackData>, IInjectable
    {
        private AttackConfigSO AttackConfig;

        public override void Created()
        {
        }

        public override void Start(IMonoAgent agent, AttackData data)
        {
            data.Timer = AttackConfig.AttackDelay;
        }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }

        public override ActionRunState Perform(
            IMonoAgent agent,
            AttackData data,
            ActionContext context
        )
        {
            data.Timer -= context.DeltaTime;

            bool shouldAttack =
                data.Target != null
                && Vector3.Distance(data.Target.Position, agent.transform.position)
                <= AttackConfig.MeleeAttackRadius;

            data.Animator.SetBool(AttackData.ATTACK, shouldAttack);

            if (shouldAttack)
            {
                agent.transform.LookAt(data.Target.Position);
            }

            if (data.Timer > 0)
            {
                return ActionRunState.Continue;
            }

            return ActionRunState.Stop;
        }

        public override void End(IMonoAgent agent, AttackData data)
        {
            data.Animator.SetBool(AttackData.ATTACK, false);
        }
    }
}