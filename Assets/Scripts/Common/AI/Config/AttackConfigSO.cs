using UnityEngine;

namespace Common.AI.Config
{
    [CreateAssetMenu(menuName = "AI/Attack config", fileName = "Attack config", order = 1)]
    public class AttackConfigSO : ScriptableObject
    {
        public float SensorRadius = 10f;
        public float MeleeAttackRadius = 1f;
        public int MeleeAttackCost = 1;
        public float AttackDelay = 1f;
        public LayerMask AttackableLayerMask;
    }
}