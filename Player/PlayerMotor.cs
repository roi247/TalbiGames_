using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
    //private Vector3 thrusterForce = Vector3.zero;

    [SerializeField] bool damagePlayerOnFall=true;

    [SerializeField]
    FallingPlayerData fallingPlayer;

    [SerializeField]
    float jumpUpForce;

    [SerializeField] bool enableJump;

    [SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

    [Serializable] public class OnPlayerFall : UnityEvent<int> { }
    public OnPlayerFall onPlayerFall;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	// Gets a rotational vector
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	// Gets a rotational vector for the camera
	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}
	
	// Get a force vector for our thrusters
	//public void ApplyThruster (Vector3 _thrusterForce)
	//{
		//thrusterForce = _thrusterForce;
	//}

	// Run every physics iteration
	void FixedUpdate ()
	{
		PerformMovement();
		PerformRotation();
	}

	//Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

		//if (thrusterForce != Vector3.zero)
		//{
			//rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		//}

	}

	//Perform rotation
	void PerformRotation ()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if (cam != null)
		{
			// Set our rotation and clamp it
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//Apply our rotation to the transform of our camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
		}
        if (Input.GetButtonDown("Jump") && enableJump)
        {
            enableJump = false;
            //Debug.Log ("Jump! . count= " + count.ToString());
            rb.AddForce(Vector3.up * jumpUpForce, ForceMode.Acceleration);
            //++count;
        }
    }

    public void StopMovingAndRotating()
    {
        //Debug.Log("StopMovingAndRotating");
        //rb.isKinematic = true;
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        //rb.isKinematic = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        enableJump = true;
        float _velocity = collision.relativeVelocity.magnitude;
        int healthDecrease = fallingPlayer.CalculateHealthDecrease(_velocity);
        Debug.Log("collision! . velocity is: " + _velocity.ToString() + "Damage is: " + healthDecrease.ToString());
        if (damagePlayerOnFall && healthDecrease>0)
        {
            //Damage Player
            //GetComponent<PlayerShoot>().CmdPlayerShot(transform.name, healthDecrease, transform.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("BUILDING 18 COLLIDER");
        enableJump = true;
        float _velocity = rb.velocity.magnitude;
        int healthDecrease = fallingPlayer.CalculateHealthDecrease(_velocity);
        Debug.Log("collision! . velocity is: " + _velocity.ToString() + "Damage is: " + healthDecrease.ToString());
        if (damagePlayerOnFall && healthDecrease>0)
        {
            //Damage Player
            //GetComponent<PlayerShoot>().CmdPlayerShot(transform.name, healthDecrease, transform.name);
        }
    }



}


[System.Serializable]
class FallingPlayerData
{
    public float minDamageVelocity = 6.5f;
    public float constantFactor = 1.25f;
    public double relativeVelocityFactor = 2.5f;

    //[HideInInspector]
    //public string floorTag = "Floor";

    public float InterpulateVelocityDamage(float x)
    {
        //0->0
        //1->5
        //5->50
        //9->100
        //return ((-35 / 300) * (float)Math.Pow (x, 3) + (95 / 40) * (float)Math.Pow (x, 2) + (905 / 288) * x)*_factor;
        return constantFactor * ((float)Math.Pow(x, relativeVelocityFactor));
    }

    public int CalculateHealthDecrease(float velocity, int maxHealth = 100)
    {
        if (velocity < minDamageVelocity)
            return 0;
        else
        {
            float relativeVelocity = velocity - minDamageVelocity;
            float _relativeDamage = InterpulateVelocityDamage(relativeVelocity) * (maxHealth / 100f);
            return (Mathf.FloorToInt(_relativeDamage));
        }
    }
}