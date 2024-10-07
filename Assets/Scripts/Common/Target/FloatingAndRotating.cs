using UnityEngine;

namespace Common.Target
{

    public class FloatingAndRotating : MonoBehaviour
    {
        public float floatAmplitude = 0.5f;
        public float floatSpeed = 1f;
        public float rotationSpeed = 30f;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            var newY = _startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);


            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

}