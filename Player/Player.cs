using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    public string playerName;


    [SerializeField]
    PlayerUI playerUI;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    [SerializeField]
    private GameObject deathEffect;

    public TextMesh nameText;
    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public Camera playerCamera;

    public int kills;
    public int deaths;

    bool damaged;
    [SerializeField] float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    [SerializeField] AudioSource hitGroundAudio;


    public void PlayLocalEffects()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            Debug.Log("DAMAGE EFFETCTS !!!!!!!!!!!!!!");
            // ... set the colour of the damageImage to the flash colour.
            playerUI.damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            playerUI.damageImage.color = Color.Lerp(playerUI.damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            //Switch cameras
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        playerUI = GetComponent<PlayerSetup>().playerUI;
        CmdBroadCastNewPlayerSetup();
    }

    
    [Command] 
    void CmdChangeName(string newName)
    {
        playerName = newName;
        RpcJoinRoomMessage();
    }

    [ClientRpc]
    void RpcJoinRoomMessage()
    {
        if (!isLocalPlayer)
            GameManager.instance.messanger.AddToGameInfoText(playerName + " Has Joined The Game", 3f);
        else
            GameManager.instance.messanger.AddToGameInfoText("You " + " Have Joined The Game", 3f);
    }


    void Start()
    {
        if (isLocalPlayer)
        {
            CmdChangeName(UserAccountManager.LoggedIn_Username);
        }
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }


    void Update()
    {
        PlayLocalEffects();
    }

    public void PlayHitGroundSound()
    {
        if (!hitGroundAudio.isPlaying)
            hitGroundAudio.Play();
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string playerShootingName, string weaponName)
    {
        if (isDead)
            return;

        if (_amount > 1)
            damaged = true;

        currentHealth -= _amount;

        // Debug.Log(transform.name + " now has " + currentHealth + " health.");


        if (currentHealth <= 0)
        {
            GameManager.instance.messanger.AddKillsInfoText(playerShootingName, this.name, weaponName);
            Die(playerShootingName);
        }
    }

    public int GetCurrentHealth()
    {
        if (currentHealth > 0)
            return currentHealth;
        else
            return 0;
    }

    private void Die(string _sourceID)
    {
        isDead = true;


        if (GameManager.instance.matchSettings.atDeath == AtDeath.RemoveLastWeapon)
        {
            GetComponent<WeaponManager>().RemoveLastWeapon();
        }
        else if (GameManager.instance.matchSettings.atDeath == AtDeath.RemoveAllWeapons)
        {
            GetComponent<WeaponManager>().RemoveAllWeapons();
        }

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
        }

        deaths++;
        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable the collider
        ////Collider _col = GetComponent<Collider>();
       //// if (_col != null)
            ////_col.enabled = false;

        //Spawn a death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
    }


    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        //GetComponent<WeaponManager>().setWeapon();
        SetupPlayer();

        Debug.Log(transform.name + " respawned.");
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        //Enable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        //Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

}

