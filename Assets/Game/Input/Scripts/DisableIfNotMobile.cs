
using UnityEngine;

namespace Game.Input.Scripts
{

    public class DisableIfNotMobile : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(Application.isMobilePlatform);
        }
    }

}
