
using UnityEngine;

namespace Game.Enemies.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnLocation = null;
        [SerializeField] private Transform _seekerDestination = null;

        [SerializeField] private GameObject _templateEnemy = null;

        private void Start()
        {
            GameObject obj = Instantiate(_templateEnemy, _spawnLocation.position, Quaternion.identity);

            var brain = obj.GetComponent<EnemyBrain>();
            if (brain)
            {
                brain.Setup(_seekerDestination.position);
                //brain.SetChaseTarget(GameObject.FindGameObjectWithTag("Player"));   //hahahha

            }

        }

    }

}
