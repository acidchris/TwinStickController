
using UnityEngine;

namespace Game
{

    public class PlayerTrackingGameModule : IGameModule
    {
        public PlayerTrackingGameModule(Transform obj)
        {
            _playerTransform = obj;
        }

        public void InitializeModule()
        {
            Debug.Assert(_playerTransform != null);
        }


        public Transform PlayerTransform => _playerTransform;


        private Transform _playerTransform = null;
    }

}
