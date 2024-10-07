using System;
using AI.Config;
using AI.Goals;
using CrashKonijn.Goap.Behaviours;
using Sensors;
using UnityEngine;

namespace AI.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class MonsterBrain : MonoBehaviour
    {
        [SerializeField]
        private PlayerSensor PlayerSensor;

        [SerializeField]
        private AttackConfigSO AttackConfig;

        private AgentBehaviour AgentBehaviour;

        void Awake()
        {
            AgentBehaviour = GetComponent<AgentBehaviour>();
        }

        void Start()
        {
            AgentBehaviour.SetGoal<WanderGoal>(false);

            PlayerSensor.Collider.radius = AttackConfig.SensorRadius;
        }

        void OnEnable()
        {
            PlayerSensor.OnPlayerEnter += PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit += PlayerSensorOnPlayerExit;
        }

        void OnDisable()
        {
            PlayerSensor.OnPlayerEnter -= PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit -= PlayerSensorOnPlayerExit;
        }

        private void PlayerSensorOnPlayerExit(Vector3 lastKnownPosition)
        {
            AgentBehaviour.SetGoal<WanderGoal>(true);
        }

        private void PlayerSensorOnPlayerEnter(Transform player)
        {
            AgentBehaviour.SetGoal<KillPlayer>(true);
        }
    }
}