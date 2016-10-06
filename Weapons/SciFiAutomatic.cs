using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace MultiplayerFps
{
    public class SciFiAutomatic : RaycastFirearm
    {
        public override void PlayShootingAudio()
        {
            if (!weaponManager.currentWeaponAudio.isPlaying)
                weaponManager.currentWeaponAudio.Play();
        }

        void Update()
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (playerShooting != null)
                    playerShooting.CmdOnStopShootingAudio();
            }


        }
    }

}

