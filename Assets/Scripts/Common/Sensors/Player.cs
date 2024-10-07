using UnityEngine;

namespace Common.Sensors
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private int Health = 1000;

    }
}