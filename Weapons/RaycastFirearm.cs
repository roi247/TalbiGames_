using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RaycastFirearm : PlayerWeapon
{
    private const string PLAYER_TAG = "Player";
    [SerializeField] protected LayerMask mask;
    protected Player player;
    public float range = 100f;

    public override void Shoot()
    {
        player = playerShooting.GetComponent<Player>();
        base.Shoot();
        //Debug.Log("SHOOTING NOW -RaycastFirearm");
        RaycastHit _hit;
        if (Physics.Raycast(player.playerCamera.transform.position, player.playerCamera.transform.forward, out _hit, range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                playerShooting.CmdPlayerShot(_hit.collider.name, damage,playerShooting.name);
            }

            // We hit something, call the OnHit method on the server
            playerShooting.CmdOnHit(_hit.point, _hit.normal);
        }
        playerShooting.CmdOnShoot();

    }

    /*
    public void showBall()
    {
        Instantiate(debugBall,transform.position+new Vector3(0,4,0), Quaternion.identity);
    }
    */

}
