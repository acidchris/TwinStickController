
using UnityEngine;

namespace Game.Shooting.Scripts
{
    public class SimpleBulletMover : MonoBehaviour
    {
        private Vector3 _startPosition = Vector3.zero;
        private Vector3 _endPosition = Vector3.zero;
        private float _progress = 0f;
        private bool _start = false;

        [SerializeField, Range(0.01f, 1f)]
        private float _tracerDuration = 0.06f;

        public void Setup(Vector3 start, Vector3 destination)
        {
            _startPosition = start;
            _endPosition = destination;
            _start = true;
            _progress = 0f;
        }


        void Update()
        {
            if (_start)
            {
                _progress += Time.deltaTime;
                float percentageComplete = _progress / _tracerDuration;

                transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

                //moves some stuff regardless
                //transform.position += (_endPosition - _startPosition).normalized * Time.deltaTime * _speed;

                if (percentageComplete > 2f)
                    Destroy(gameObject);
            }
        }
    }

}
