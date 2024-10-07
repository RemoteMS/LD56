using UnityEngine;

namespace Common.AI.Audio
{
    public class FootstepSoundInCharacterController : MonoBehaviour
    {
        public AudioClip[] footstepSounds;
        public AudioSource audioSource;
        public CharacterController controller;

        public float stepInterval = 0.5f;
        private float _stepTimer = 0f;

        private void Update()
        {
            if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
            {
                _stepTimer += Time.deltaTime;

                if (_stepTimer >= stepInterval)
                {
                    PlayFootstepSound();
                    _stepTimer = 0f;
                }
            }
            else
            {
                _stepTimer = 0f;
            }
        }

        private void PlayFootstepSound()
        {
            var index = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[index]);
        }
    }
}