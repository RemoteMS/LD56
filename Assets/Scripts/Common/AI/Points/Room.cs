using UnityEngine;

namespace Common.AI.Points
{
    [DefaultExecutionOrder(-10)]
    public class Room : MonoBehaviour
    {
        public int Level = 0;
        public InterestsPoint[] interestsPoints;

        private void Awake()
        {
            interestsPoints = GetComponentsInChildren<InterestsPoint>();

            foreach (var interestsPoint in interestsPoints)
            {
                interestsPoint.Level = Level;
            }
        }
    }
}