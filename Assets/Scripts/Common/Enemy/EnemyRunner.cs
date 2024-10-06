using UnityEngine;
using UnityEngine.AI;

namespace Common.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyRunner : MonoBehaviour
    {
        [SerializeField] private Transform Player;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private float timeInterval = 5f;

        private void Start()
        {
            InvokeRepeating(nameof(MoveTowardsPlayer), 0f, timeInterval);
        }

        private void MoveTowardsPlayer()
        {
            navMeshAgent.SetDestination(Player.position);
        }
    }
}