using UnityEngine;
using System.Collections;


/*
public class BulletsWeaponManager : MonoBehaviour {
    [SerializeField]
    Rigidbody bullet;
    [SerializeField]
    Transform barrelEnd;

    [SerializeField]
    float bulletForceFactor=2000f;

    public void ShootMissile(PlayerShoot _rocketOwner)
    {
        Rigidbody rocketInstance;
        rocketInstance =  Instantiate(bullet, barrelEnd.position,barrelEnd.rotation) as Rigidbody;
        rocketInstance.AddForce(barrelEnd.up * bulletForceFactor);
        rocketInstance.GetComponent<MissleManager>().rocketOwner = _rocketOwner;
    }

}
*/