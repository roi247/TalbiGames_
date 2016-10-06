using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public enum Scope {defaultScope, SniperScope};
public class PlayerUI : MonoBehaviour {

    [SerializeField]
	RectTransform thrusterFuelFill;

	[SerializeField]
	GameObject pauseMenu;

    [SerializeField]
    ChatController chatController;

    [SerializeField]
    Text healthText;

    public PlayerMotor playerMotor;
	public PlayerController controller;

    [SerializeField] UnityEvent onClosePauseMenu;

    public Image damageImage;

    [SerializeField]
   Image sniperScope;

    [SerializeField]
   Image defaultScope;

    public void SetController (PlayerController _controller)
	{
		controller = _controller;
	}

	void Start ()
	{
		PauseMenu.IsOn = false;

        //Cursor.lockState = CursorLockMode.Locked;
       // Cursor.visible = false;

        playerMotor =controller.GetComponent<PlayerMotor>();
    }

	void Update ()
	{
		//SetFuelAmount (controller.GetThrusterFuelAmount());
        setHealth(controller.GetComponent<Player>().GetCurrentHealth());

        if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
            if (ChatController.isOn)
                chatController.ToggleChaWindow();

        }
        if (Input.GetKeyDown(KeyCode.Tab) && !PauseMenu.IsOn)
        {
            chatController.ToggleChaWindow();
        }

    }


    public void SwitchScope(Scope scope)
    {
        if (scope==Scope.SniperScope)
        {
            Debug.Log("SNIPER SCOPE NOW!");
            sniperScope.gameObject.SetActive (true);
            defaultScope.gameObject.SetActive(false);
        }
        else if (scope == Scope.defaultScope)
        {
            Debug.Log("DEFAULT SCOPE NOW!");
            sniperScope.gameObject.SetActive(false);
            defaultScope.gameObject.SetActive(true);
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
        if (PauseMenu.IsOn)
        { 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
         }
        else
        {
            if (onClosePauseMenu != null)
                onClosePauseMenu.Invoke();
           Cursor.lockState = CursorLockMode.Locked;
           Cursor.visible = false;
        }
        playerMotor.StopMovingAndRotating();
    }


    void SetFuelAmount (float _amount)
	{
		thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
	}

    public void setHealth(int health)
    {
        string strHealth = health.ToString();
        healthText.text = strHealth;
    }


}
