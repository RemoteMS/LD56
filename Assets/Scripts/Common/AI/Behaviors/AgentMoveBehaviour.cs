using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Common.AI.Behaviors
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AgentBehaviour))]
    public class AgentMoveBehaviour : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent NavMeshAgent;

        [SerializeField] private Animator Animator;

        private AgentBehaviour AgentBehavior;

        private ITarget CurrentTarget;

        [SerializeField] private float MinMoveDistance = 0.25f;
        private Vector3 LastPosition;
        private static readonly int WALK = Animator.StringToHash("Walk");

        private void Awake()
        {
            if (Animator == null)
                throw new ArgumentException("Animator is null ");

            NavMeshAgent = GetComponent<NavMeshAgent>();
            AgentBehavior = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            AgentBehavior.Events.OnTargetChanged += EventsOnTargetChanged;
            AgentBehavior.Events.OnTargetOutOfRange += EventsOnTargetOutOfRange;
        }

        private void OnDisable()
        {
            AgentBehavior.Events.OnTargetChanged -= EventsOnTargetChanged;
            AgentBehavior.Events.OnTargetOutOfRange -= EventsOnTargetOutOfRange;
        }

        private void EventsOnTargetOutOfRange(ITarget target)
        {
            Animator.SetBool(WALK, false);
        }

        private void EventsOnTargetChanged(ITarget target, bool inRange)
        {
            CurrentTarget = target;
            LastPosition = CurrentTarget.Position;
            NavMeshAgent.SetDestination(target.Position);
            Animator.SetBool(WALK, true);
        }

        void Update()
        {
            if (CurrentTarget == null)
                return;

            if (MinMoveDistance <= Vector3.Distance(CurrentTarget.Position, LastPosition))
            {
                LastPosition = CurrentTarget.Position;
                NavMeshAgent.SetDestination(CurrentTarget.Position);
            }

            Animator.SetBool(WALK, NavMeshAgent.velocity.magnitude > 0.1f);
        }
    }
}