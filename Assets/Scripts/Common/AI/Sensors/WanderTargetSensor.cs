using Common.AI.Config;
using Common.AI.GameMaster;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using ServiceLocator;
using UnityEngine;
using UnityEngine.AI;

namespace Common.AI.Sensors
{
    public class WanderTargetSensor : LocalTargetSensorBase, IInjectable
    {
        private WanderConfigSO WanderConfig;
        
        private GM _gameMaster;

        public override void Created()
        {
            Debug.Log("Created WanderTargetSensor");
            _gameMaster = SL.Current.Get<GM>();
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var position = _gameMaster.GetPlayerPoint();
            return new PositionTarget(position);
        }

        private Vector3 GetRandomPosition(IMonoAgent agent)
        {
            var count = 0;

            while (count < 5)
            {
                var random = Random.insideUnitCircle * WanderConfig.WanderRadius;
                var position = new Vector3(random.x, 0, random.y);

                if (NavMesh.SamplePosition(position, out var hit, 1, NavMesh.AllAreas))
                {
                    return hit.position;
                }

                count++;
            }

            return agent.transform.position;
        }

        public override void Update()
        {
        }

        public void Inject(DependencyInjector injector)
        {
            WanderConfig = injector.WanderConfig;
        }
    }
}