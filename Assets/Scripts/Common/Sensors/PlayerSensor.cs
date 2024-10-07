using UnityEngine;

namespace Common.Sensors
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider Collider;

        public delegate void PlayerEnterEvent(Transform player);

        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"other entered - {other.gameObject.name}");
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerEnter?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log($"other exit - {other.gameObject.name}");
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerExit?.Invoke(other.transform.position);
            }
        }
    }
}