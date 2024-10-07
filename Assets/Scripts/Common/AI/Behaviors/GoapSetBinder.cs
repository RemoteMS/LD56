using AI.Factories;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace AI.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapSetBinder : MonoBehaviour
    {
        [SerializeField]
        private GoapRunnerBehaviour GoapRunner;

        void Awake()
        {
            AgentBehaviour agent = GetComponent<AgentBehaviour>();
            agent.GoapSet = GoapRunner.GetGoapSet(GoapSetConfigFactory.MonsterSet);
        }
    }
}