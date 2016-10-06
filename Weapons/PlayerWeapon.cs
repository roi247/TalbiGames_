using UnityEngine;
using System.Collections;

namespace MultiplayerFps
{
    public class PlayerWeapon : MonoBehaviour
    {
        public string weaponName = "Glock";
        [SerializeField]
        protected PlayerShoot playerShooting;
        public int damage = 10;

        public float timeBetweenBullets = 0.1f;

        public WeaponGraphics graphics;

        public AudioSource weaponAudio;
        public bool enableWeaponSwitch = true;
        public virtual void Shoot() { }

        protected WeaponManager weaponManager;

        public virtual void PlayShootingAudio()
        {
            //Debug.Log("BASE- PLAYING WEAPON AUDIO NOW");
            weaponManager.currentWeaponAudio.Play();
        }

        public virtual void ScopeManagment() { }

        public void SetPlayerShooting(PlayerShoot _playerShooting)
        {
            playerShooting = _playerShooting;
            weaponManager = playerShooting.GetComponent<WeaponManager>();
        }

        public virtual void MakeLocalShootEffects()
        {

        }

        public virtual void MakeShootEffects()
        {
            playerShooting.CmdOnShoot();
        }
        void Update()
        {
            ScopeManagment();
        }


        //public GameObject debugBall;
    }

}

