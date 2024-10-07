using UnityEngine;
using UnityEngine.AI;

namespace Common.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyRunner : MonoBehaviour
    {
        private static readonly int Walk = Animator.StringToHash("Walk");
        [SerializeField] private Transform Player;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float updateInterval = 5f;

        [SerializeField] private float stoppingDistance = 1f;

        private float _timeSinceLastUpdate = 0f;
        

        private void Update()
        {
            _timeSinceLastUpdate += Time.deltaTime;

            if (_timeSinceLastUpdate >= updateInterval)
            {
                MoveTowardsPlayer();
                _timeSinceLastUpdate = 0f;
            }

            CheckIfReachedDestination();
        }

        private void MoveTowardsPlayer()
        {
            animator.SetBool(Walk, true);
            navMeshAgent.SetDestination(Player.position);
        }

        private void CheckIfReachedDestination()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool(Walk, false);
                }
            }
        }
    }

}