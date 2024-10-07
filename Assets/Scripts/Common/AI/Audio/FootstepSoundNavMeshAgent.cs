using UnityEngine;
using UnityEngine.AI;

namespace Common.AI.Audio
{
    public class FootstepSoundNavMeshAgent : MonoBehaviour
    {
        public AudioClip[] footstepSounds;
        public AudioSource audioSource;
        public NavMeshAgent agent;

        public float stepInterval = 0.5f;
        private float _stepTimer = 0f;

        private void Update()
        {
            if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
            {
                _stepTimer += Time.deltaTime;


                if (!(_stepTimer >= stepInterval)) return;
                PlayFootstepSound();
                _stepTimer = 0f;
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