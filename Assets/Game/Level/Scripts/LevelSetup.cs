
using System.Collections.Generic;
using UnityEngine;
using Spawner = Game.Enemies.Scripts.EnemySpawner;

namespace Game.Level.Scripts
{

    public class LevelSetup : MonoBehaviour
    {
        [SerializeField] private List<Spawner> _enemySpawnersInThisLevel = new List<Spawner>();



        private void Start()
        {
            int idx = Random.Range(0, _enemySpawnersInThisLevel.Count);

            _enemySpawnersInThisLevel[idx].StartSpawning();
        }
    }

}
