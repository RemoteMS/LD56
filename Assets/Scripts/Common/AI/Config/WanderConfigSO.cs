using UnityEngine;

namespace AI.Config
{
    [CreateAssetMenu(menuName = "AI/Wander config", fileName = "Wander config", order = 2)]
    public class WanderConfigSO : ScriptableObject
    {
        public Vector2 WaitRangeBetweenWanders = new(1, 5);
        public float WanderRadius = 5f;
    }
}