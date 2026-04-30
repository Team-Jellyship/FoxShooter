using UnityEngine;

namespace FoxShooter.Props.Projectiles
{
    public class HitscanParticle : MonoBehaviour
    {
        private Vector3 _origin;
        private Vector3 _end;
        private float _speed;
        private float _time;
        private float _maxTime;

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time > _maxTime)
            {
                Destroy(gameObject);
            }

            transform.position = Vector3.Lerp(_origin, _end, _time / _maxTime);
        }

        public void SetPath(Vector3 origin, Vector3 end, float speed, float startTime)
        {
            _origin = origin;
            _end = end;
            _speed = speed;

            _time = startTime;
            var delta = origin - end;
            var distance = delta.magnitude;
            _maxTime = distance / speed;
            transform.rotation = Quaternion.LookRotation(delta);
            transform.position = Vector3.Lerp(_origin, _end, _time / _maxTime);
        }
    }
}