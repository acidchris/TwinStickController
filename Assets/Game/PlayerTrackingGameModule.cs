
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
            
        }


        public Transform PlayerTransform => _playerTransform;


        private Transform _playerTransform = null;
    }

}
