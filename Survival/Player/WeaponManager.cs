using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SurvivalGame
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private string weaponLayerName = "Weapon";

        [SerializeField]
        private Transform weaponHolder;

        [SerializeField]
        public PlayerWeapon primaryWeapon;

        public PlayerWeapon currentWeapon;
        private WeaponGraphics currentGraphics;

        public List<PlayerWeapon> playerWeaponsList;

        private PlayerWeapon weaponIns = null;

        void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponGraphics GetCurrentGraphics()
        {
            return currentGraphics;
        }

        public bool HasWeapon(string _weaponName)
        {
            foreach (PlayerWeapon _weapon in playerWeaponsList)
            {
                if (_weapon.weaponName == _weaponName)
                    return true;
            }
            return false;
        }

        public void SwitchWeapon()
        {
            EquipWeapon(playerWeaponsList[GetCurrentWeaponIndex()+1]);
        }


        void EquipWeapon(PlayerWeapon _weapon)
        {
            if (_weapon == null)
            {
                Debug.Log("_weapon is NULL !!!");
                return;
            }

            if (weaponIns != null)
            {
                Destroy(weaponIns.gameObject);
            }

            weaponIns = Instantiate(_weapon, weaponHolder.position, weaponHolder.rotation) as PlayerWeapon;
            weaponIns.transform.SetParent(weaponHolder);
            currentWeapon = weaponIns;

            //currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
            //if (currentGraphics == null)
                //Debug.LogError("No WeaponGraphics component on the weapon object: " + weaponIns.name);

            Util.SetLayerRecursively(weaponIns.gameObject, LayerMask.NameToLayer(weaponLayerName));

        }

        int GetCurrentWeaponIndex()
        {
            int index = 0;
            foreach (PlayerWeapon weap in playerWeaponsList)
            {
                if (weap.weaponName==playerWeaponsList[index].weaponName)
                {
                    return index;
                }
                index++;
            }
            return index;
        }


    }

}

 


