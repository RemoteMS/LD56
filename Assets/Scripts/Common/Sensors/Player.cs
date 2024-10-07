using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private int Health = 1000;

    }
}