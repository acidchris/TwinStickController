using UnityEngine;
using UnityEngine.Pool;

namespace Game.Shooting.Scripts
{
    public abstract class ABullet : MonoBehaviour
    {
        private IObjectPool<ABullet> _pool = null;

        protected IObjectPool<ABullet>  Pool => _pool;

        public virtual void SetPool(IObjectPool<ABullet> pool)
        {
            _pool = pool;
        }


        public abstract void SetupBullet(Vector3 start, Vector3 destination);

    }

}

