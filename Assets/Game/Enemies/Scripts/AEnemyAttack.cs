
using UnityEngine;

namespace Game.Enemies.Scripts
{

    public abstract class AEnemyAttack : MonoBehaviour
    {
        private Collider _collider = null;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            Debug.Assert(_collider != null);

            _collider.isTrigger = true; //just to get notified
        }

        protected abstract void OnTriggerEnter(Collider other);
        protected abstract void OnTriggerExit(Collider other);
    }

}
