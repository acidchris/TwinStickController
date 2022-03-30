
using System;
using UnityEngine;

namespace Game.Enemies.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnLocation = null;
        [SerializeField] private Transform _seekerDestination = null;

        [SerializeField] private GameObject _templateEnemy = null;


        [SerializeField] private int _numberOfEnemies = 3;
        [SerializeField] private float _spawnInterval = 2.0f;

        private int _numberOfSpawnedEnemies = 0;
        
        private float _lastSpawnTime = 0f;

        enum State
        {
            Idle = 0,
            WaitForNextSpawn,
            SpawnEnenmy
        }

        private State _state = State.Idle;

        public void StartSpawning()
        {
            //get it going
            _state = State.WaitForNextSpawn;
        }

        private void Update()
        {
            switch (_state)
            {
                default:
                case State.Idle:
                    break;

                case State.WaitForNextSpawn:
                    if (_lastSpawnTime + _spawnInterval <= Time.time)
                    {
                        _lastSpawnTime = Time.time;

                        _state = State.SpawnEnenmy;
                    }
                    break;

                case State.SpawnEnenmy:
                    SpawnEnemy();
                    _state = State.WaitForNextSpawn;
                    break;
            }
        }

        private void SpawnEnemy()
        {
            if (_numberOfSpawnedEnemies + 1 <= _numberOfEnemies)
            {
                GameObject obj = Instantiate(_templateEnemy, _spawnLocation.position, Quaternion.identity);
                var brain = obj.GetComponent<EnemyBrain>();
                if (brain)
                {
                    brain.Setup(_seekerDestination.position);
                    //brain.SetChaseTarget(GameObject.FindGameObjectWithTag("Player"));   //hahahha
                }

                ++_numberOfSpawnedEnemies;
            }
            else
            {
                _state = State.Idle;
            }
        }
    }

}
