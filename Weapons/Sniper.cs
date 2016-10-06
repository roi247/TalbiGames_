using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Sniper : RaycastFirearm
{
    PlayerUI playerUI;
    [SerializeField] MeshRenderer sniperGraphicsRenderer;
    [SerializeField] float zoomFactor=7;
    [SerializeField] int maxNumberOfToggles=3;
    int zoomTogglesCount = 0;

    float camOldFieldOfView;
    void Start()
    {
        if (playerShooting!=null)
        {
            playerUI = playerShooting.GetComponent<PlayerSetup>().playerUI;
            player = playerShooting.GetComponent<Player>();
            camOldFieldOfView = player.playerCamera.fieldOfView;
        }
    }

    public override void PlayShootingAudio()
    {
        base.PlayShootingAudio();
    }

    public override void ScopeManagment()
    {
        if (playerShooting == null)
            return;
        if (Input.GetMouseButtonDown(1) && player!=null)
        {
            player.playerCamera.fieldOfView -= zoomFactor*2f;
        }

        if (Input.GetMouseButton(1))
        {
            enableWeaponSwitch = false;
            if (playerUI!=null)
                playerUI.SwitchScope(Scope.SniperScope);
            OnSniperScopeToggle(false);
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                if (zoomTogglesCount<maxNumberOfToggles)
                {
                    if (player!=null)
                        player.playerCamera.fieldOfView -= zoomFactor;
                    zoomTogglesCount++;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                if (zoomTogglesCount > 0)
                {
                    if (player != null)
                        player.playerCamera.fieldOfView += zoomFactor;
                    zoomTogglesCount--;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            enableWeaponSwitch = true;
            player.playerCamera.fieldOfView = camOldFieldOfView;
            OnSniperScopeToggle(true);
            zoomTogglesCount = 0;
            playerUI.SwitchScope(Scope.defaultScope);
        }
    }

    void OnSniperScopeToggle(bool isSniperGraphicsEnabled)
    {
        sniperGraphicsRenderer.enabled = isSniperGraphicsEnabled;
    }


}
