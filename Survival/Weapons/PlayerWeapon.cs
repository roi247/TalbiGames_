using UnityEngine;
using System.Collections;

namespace SurvivalGame
{
    public class PlayerWeapon : MonoBehaviour
    {
        [Header("Survival Game Weapon")]
        public string weaponName = "Weapon";
        [SerializeField]
        //protected PlayerShoot playerShooting;
        public int damage = 10;

        public float timeBetweenBullets = 0.1f;

        public WeaponGraphics graphics;

        public AudioSource weaponAudio;

        public bool enableWeaponSwitch = true;

        public virtual void Shoot() { }

        public virtual void PlayShootingAudio()
        {
            weaponAudio.Play();
        }

        public virtual void ScopeManagment() { }

        public virtual void MakeShootEffects()
        {
            PlayShootingAudio();
            graphics.muzzleFlash.Play();
        }
        void Update()
        {
            ScopeManagment();
        }


        //public GameObject debugBall;
    }

}


