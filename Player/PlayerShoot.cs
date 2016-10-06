using UnityEngine;
using UnityEngine.Networking;
//using MultiplayerFps;

namespace MultiplayerFps
{

    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : NetworkBehaviour
    {


        [SerializeField]
        private Camera cam;

        [SerializeField]
        private LayerMask mask;

        private PlayerWeapon currentWeapon;
        private WeaponManager weaponManager;

        float timer = 0f;

        //public delegate void MyDelegate();
        // public MyDelegate myDelegate;


        void Start()
        {
            if (cam == null)
            {
                Debug.LogError("PlayerShoot: No camera referenced!");
                this.enabled = false;
            }

            weaponManager = GetComponent<WeaponManager>();
        }

        void Update()
        {

            currentWeapon = weaponManager.GetCurrentWeapon();

            if (PauseMenu.IsOn)
                return;

            if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer && currentWeapon.enableWeaponSwitch && !ChatController.isOn)
            {
                //string nextWeap = weaponManager.FindNextWeaponName(currentWeapon.weaponName);
                weaponManager.SwitchWeapon();
            }

            timer += Time.deltaTime;
            if (currentWeapon != null && timer > currentWeapon.timeBetweenBullets && Input.GetButton("Fire1") && !ChatController.isOn)
            {
                timer = 0f;
                Shoot();
            }


            /*
            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            } else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
                } else if (Input.GetButtonUp ("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }
            */

        }

        //Is called on the server when a player shoots

        //Is called on the server when we hit something
        //Takes in the hit point and the normal of the surface

        [Command]
        void CmdOnBulletShoot()
        {
            RpcBulletShoot();
        }


        [ClientRpc]
        void RpcBulletShoot()
        {
            //BulletsWeaponManager bulletManager = weaponManager.GetCurrentGraphics().gameObject.GetComponent<BulletsWeaponManager>();
            //if (bulletManager!=null)
            //  bulletManager.ShootMissile(this);
        }

        [Client]
        void Shoot()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            currentWeapon.Shoot();
            currentWeapon.MakeShootEffects();
            currentWeapon.MakeLocalShootEffects();
            //We are shooting, call the OnShoot method on the server
            //CmdOnBulletShoot();


        }


        [Command]
        public void CmdOnStopShootingAudio()
        {
            RpcStopShootEffect();
        }

        [ClientRpc]
        void RpcStopShootEffect()
        {
            weaponManager.currentWeaponAudio.Stop();
        }

        [Command]
        public void CmdOnHit(Vector3 _pos, Vector3 _normal)
        {
            RpcDoHitEffect(_pos, _normal);
        }

        //Is called on all clients
        //Here we can spawn in cool effects

        [ClientRpc]
        void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
        {
            //Debug.Log("HIT EFFECTS!!");
            GameObject _hitEffect = (GameObject)Instantiate(currentWeapon.graphics.hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
            Destroy(_hitEffect, 2f);
        }

        [Command]
        public void CmdPlayerShot(string _playerID, int _damage, string shootinPlayerName)
        {
            Debug.Log(_playerID + " has been shot.");

            Player _player = GameManager.GetPlayer(_playerID);
            _player.RpcTakeDamage(_damage, shootinPlayerName, currentWeapon.weaponName);
        }



        [Command]
        public void CmdOnShoot()
        {
            RpcDoShootEffect();
        }


        //Is called on all clients when we need to to
        // a shoot effect
        [ClientRpc]
        void RpcDoShootEffect()
        {
            if (currentWeapon.graphics.muzzleFlash != null)
            {
                currentWeapon.graphics.muzzleFlash.Play();
            }
            currentWeapon.PlayShootingAudio();
        }

        [Command]
        public void CmdOnFireRocket()
        {
            RpcFireRocket();
        }

        [ClientRpc]
        void RpcFireRocket()
        {
            currentWeapon.SendMessage("RocketShoot");
        }



        /*
        [Command]
        void CmdOnBulletShoot()
        {
            RpcBulletShoot();
        }


        [ClientRpc]
        void RpcBulletShoot()
        {
            BulletsWeaponManager bulletManager = weaponManager.GetCurrentGraphics().gameObject.GetComponent<BulletsWeaponManager>();
            if (bulletManager != null)
                bulletManager.ShootMissile(this);
        }
        */




        #region Old
        /*

        public void SetDelegate(MyDelegate _myDelegate)
        {
            myDelegate = _myDelegate;
        }


        [Command]
        public void CmdGeneric()
        {
            RpcGeneric();
        }

        [ClientRpc]
        void RpcGeneric()
        {
            myDelegate();
        }

        */



        #endregion

    }
}

