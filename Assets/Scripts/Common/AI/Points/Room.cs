using UnityEngine;

namespace Common.AI.Points
{
    public class Room : MonoBehaviour
    {
        public int Level = 0;
        public InterestsPoint[] interestsPoints;

        private void Start()
        {
            interestsPoints = GetComponentsInChildren<InterestsPoint>();

            foreach (var interestsPoint in interestsPoints)
            {
                interestsPoint.Level = Level;
            }
        }
    }
}