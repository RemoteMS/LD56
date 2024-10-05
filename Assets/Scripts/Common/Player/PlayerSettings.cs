using ServiceLocator;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerSettings : IService
    {
        public Transform playerCamera;
        public CharacterController characterController;
        public Transform playerTransform;

        public float mouseSensitivity = 2.0f;

        public float walkSpeed = 5f;
        public float sprintSpeed = 15f;
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