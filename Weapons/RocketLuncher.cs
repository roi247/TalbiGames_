using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace MultiplayerFps
{
    public class RocketLuncher : BulltetsFirearm
    {
        public override void Shoot()
        {
            playerShooting.CmdOnFireRocket();
        }

        public void RocketShoot()
        {
            Rigidbody rocketInstance;
            rocketInstance = Instantiate(bullet, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
            rocketInstance.AddForce(barrelEnd.up * bulletForceFactor);
            MissleManager missleManager = rocketInstance.GetComponent<MissleManager>();
            missleManager.rocketOwner = playerShooting;
            missleManager.SetMissileProperties(damage, maxDamageRange);
            playerShooting.CmdOnShoot();
        }
    }

}

