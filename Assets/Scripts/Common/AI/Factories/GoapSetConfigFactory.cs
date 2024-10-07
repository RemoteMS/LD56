using System;
using AI.Actions;
using AI.Goals;
using AI.Sensors;
using AI.Targets;
using AI.WorldKeys;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;
using UnityEngine;

namespace AI.Factories
{
    [RequireComponent(typeof(DependencyInjector))]
    public class GoapSetConfigFactory : GoapSetFactoryBase
    {
        private DependencyInjector Injector;
        public const string MonsterSet = "MonsterSet";

        public override IGoapSetConfig Create()
        {
            Injector = GetComponent<DependencyInjector>();

            var builder = new GoapSetBuilder(MonsterSet);

            BuildGoals(builder);
            BuildActions(builder);
            BuildSensors(builder);

            return builder.Build();
        }

        private void BuildGoals(GoapSetBuilder builder)
        {
            builder
                .AddGoal<WanderGoal>()
                .AddCondition<IsWandering>(Comparison.GreaterThanOrEqual, 1);

            builder
                .AddGoal<KillPlayer>()
                .AddCondition<PlayerHealth>(Comparison.SmallerThanOrEqual, 0);
        }

        private void BuildActions(GoapSetBuilder builder)
        {
            builder
                .AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(EffectType.Increase)
                .SetBaseCost(5)
                .SetInRange(10);

            builder
                .AddAction<MeleeAction>()
                .SetTarget<PlayerTarget>()
                .AddEffect<PlayerHealth>(EffectType.Decrease)
                .SetBaseCost(Injector.AttackConfig.MeleeAttackCost)
                .SetInRange(Injector.AttackConfig.SensorRadius);
        }

        private void BuildSensors(GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>().SetTarget<WanderTarget>();

            builder.AddTargetSensor<PlayerTargetSensor>().SetTarget<PlayerTarget>();
        }
    }
}
