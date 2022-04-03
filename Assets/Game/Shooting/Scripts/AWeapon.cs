
using System;
using UnityEngine;

namespace Game.Shooting.Scripts
{
    [Serializable]
    public struct WeaponPropertiesDefinition
    {
        public float BaseDamage;
        public float ShootingInterval;
        public float BulletSpreadFactor;
    }

    public class AWeapon : MonoBehaviour
    {
        [SerializeField] protected WeaponPropertiesDefinition _weaponProperties = new WeaponPropertiesDefinition();

        public WeaponPropertiesDefinition WeaponProperties { get { return _weaponProperties; } }

        //to have the 'enable/disable' checkbox in Unity :D
        protected virtual void Start() { }

    }

}
