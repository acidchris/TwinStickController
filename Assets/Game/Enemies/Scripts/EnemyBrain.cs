
using Game.Damaging.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies.Scripts
{

    public class EnemyBrain : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent = null;
        private Vector3 _destinationPosition;

        private GameObject _chaseTarget = null;

        private int _agentState = 0;
        private float _lastChaseTime = 0f;
        private float _chaseCheckInterval = 0.5f;

        private EnemyDamageReceiver _damageReceiver = null;

        protected void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.enabled = false;

            _damageReceiver = GetComponent<EnemyDamageReceiver>();
        }

        protected void Start()
        {
            _damageReceiver._onDied += OnDeath;
        }

        protected void OnDestroy()
        {
            _damageReceiver._onDied -= OnDeath;
        }

        public void Setup(Vector3 toPosition)
        {
            _destinationPosition = toPosition;

            _navMeshAgent.enabled = true;
            _agentState = 1;
        }

        public void SetChaseTarget(GameObject target)
        {
            _navMeshAgent.enabled = true;
            _chaseTarget = target;
            _agentState = 1;
        }

        protected void FixedUpdate()
        {

            //chasing
            if (_agentState == 1 &&
                _lastChaseTime + _chaseCheckInterval <= Time.time)
            {
                _lastChaseTime = Time.time;

                _navMeshAgent.SetDestination(_chaseTarget != null ? _chaseTarget.transform.position : _destinationPosition);   //refresh position
            }
        }


        private void OnDeath(object sender, HealthChangeInfo _)
        {
            Destroy(gameObject);
        }

    }

}
