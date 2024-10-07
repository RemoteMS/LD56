using ServiceLocator;
using UnityEngine;
using UnityEngine.Serialization;


namespace Player
{
    [System.Serializable]
    public class PlayerSettings : IService
    {
        public Transform playerCameraTransform;
        public CharacterController characterController;
        public Transform playerTransform;

        public float mouseSensitivity = 2.0f;

        public float walkSpeed = 5f;
        public float sprintSpeed = 15f;

        public float jumpForce = 5f;
        public float gravity = -9.81f;
        public float jumpHeight = 1f;

        public float rayDistance = 10f;
        public LayerMask raycastLayerMask;
    }

    [System.Serializable]
    public class MainCharacterSettings : IService
    {
        public int health = 2;
        public int maxHealth = 2;
        public int energy = 20;
        public int maxEnergy = 20;
    }
}