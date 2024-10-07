using Player;
using ServiceLocator;
using UnityEngine;

namespace Common.Sensors
{
    [DefaultExecutionOrder(100)]
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider Collider;
        public LayerMask playerMask;

        public delegate void PlayerEnterEvent(Transform player);

        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        private PlayerSettings _playerSettings;


        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
            _playerSettings = SL.Current.Get<PlayerSettings>();
        }

        [SerializeField] private bool triggered = false;

        public void FixedUpdate()
        {
            if (triggered)
            {
                DoRaycastToPlayer();
            }
        }


        private void DoRaycastToPlayer()
        {
            if (Physics.Linecast(transform.position, _playerSettings.playerCameraTransform.position, out RaycastHit hit,
                playerMask))
            {
                if (hit.collider.TryGetComponent(out Player player))
                {
                    Debug.LogWarning("Seen");
                    Debug.DrawLine(transform.position, _playerSettings.playerCameraTransform.position, Color.red, 5f);
                }
                else
                {
                    // todo: go to last player point 
                    Debug.LogWarning("Not Seen");
                    Debug.DrawLine(transform.position, _playerSettings.playerCameraTransform.position, Color.blue, 5f);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            triggered = true;
            Debug.Log($"other entered - {other.gameObject.name}");
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerEnter?.Invoke(player.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            triggered = false;
            Debug.Log($"other exit - {other.gameObject.name}");
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerExit?.Invoke(other.transform.position);
            }
        }
    }
}