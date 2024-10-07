using AI.Config;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace AI.Sensors
{
    public class PlayerTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private AttackConfigSO AttackConfig;
        private Collider[] Colliders = new Collider[1];

        public override void Created() { }

        public void Inject(DependencyInjector injector)
        {
            AttackConfig = injector.AttackConfig;
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            if (
                Physics.OverlapSphereNonAlloc(
                    agent.transform.position,
                    AttackConfig.SensorRadius,
                    Colliders,
                    AttackConfig.AttackableLayerMask
                ) > 0
            )
            {
                //todo: 
                return new TransformTarget(Colliders[0].transform);
            }

            return null;
        }

        public override void Update() { }
    }
}