using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace MultiplayerFps
{

    public class IObtainable : MonoBehaviour
    {

        // Use this for initialization
        Collider col;
        [SerializeField]
        float rotationFactor = 10f;
        [SerializeField]
        Transform graphicsTransform;
        [SerializeField]
        Vector3 upVector;
        void Start()
        {
            col = GetComponent<Collider>();
            ObteinableWeaponsManager.Add(this.GetComponent<PlayerWeapon>(), transform);
            GameManager.instance.worldWeapons.Add(this.GetComponent<PlayerWeapon>());
        }

        /*
        [Command]
        void CmdTakeWeapon()
        {
            RpcTake();
        }

        [ClientRpc]
        */
        void Take()
        {
            col.enabled = false;
            this.gameObject.SetActive(false);
        }


        // Update is called once per frame
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            if (other.CompareTag("Player"))
            {
                WeaponManager _weaponManager = other.GetComponent<WeaponManager>();
                string _weaponName = this.GetComponent<PlayerWeapon>().weaponName;
                if (true)//!_weaponManager.HasWeapon(_weaponName))
                {
                    Take();
                    _weaponManager.CmdAddWeapon(_weaponName);
                    Remove();
                }

            }
        }

        void Update()
        {
            Rotate();
        }

        void Rotate()
        {

            graphicsTransform.Rotate(upVector, rotationFactor * Time.deltaTime);
        }

        void Remove()
        {
            //Update the data syncVar
            WeaponSpawnPoint _point = GetComponentInParent<WeaponSpawnPoint>();
            if (_point != null)
            {
                ObteinableWeaponsManager.instance.Remove(_point.spawnPointIndex);
                _point.Empty();
            }

        }

    }

}
