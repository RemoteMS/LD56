using UnityEngine;
using UnityEngine.AI;

namespace Common.AI.Audio
{
    public class FootstepSoundNavMeshAgent : MonoBehaviour
    {
        public AudioClip[] footstepSounds;
        public AudioSource audioSource;
        public NavMeshAgent agent;
        public Transform player;

        public float stepInterval = 0.5f;
        private float stepTimer = 0f;

        public float maxVolume = 1f;
        public float minDistance = 1f;
        public float maxDistance = 20f;

        void Update()
        {
            if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
            {
                stepTimer += Time.deltaTime;


                if (stepTimer >= stepInterval)
                {
                    PlayFootstepSound();
                    stepTimer = 0f;
                }


                AdjustVolume();
            }
            else
            {
                stepTimer = 0f;
            }
        }

        void PlayFootstepSound()
        {
            int index = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[index];
            audioSource.Play();
        }

        void AdjustVolume()
        {
            float distance = Vector3.Distance(transform.position, player.position);


            if (distance <= minDistance)
            {
                audioSource.volume = maxVolume;
            }

            else if (distance >= maxDistance)
            {
                audioSource.volume = 0;
            }

            else
            {
                float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
                audioSource.volume = Mathf.Lerp(maxVolume, 0, normalizedDistance);
            }
        }
    }
}