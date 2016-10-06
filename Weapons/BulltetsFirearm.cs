using UnityEngine;
using System.Collections;
using UnityEngine.Networking; 

namespace MultiplayerFps
{
    public class BulltetsFirearm : PlayerWeapon
    {
        [SerializeField]
        protected Rigidbody bullet;
        [SerializeField]
        protected Transform barrelEnd;

        [SerializeField]
        protected float bulletForceFactor = 2000f;

        [SerializeField]
        protected float maxDamageRange = 8f;

        public override void Shoot()
        {
            base.Shoot();
        }

    }
}

