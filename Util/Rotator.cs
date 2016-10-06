using UnityEngine;
using System.Collections;

public enum Axis { X ,Y, Z};
public class Rotator : MonoBehaviour {
    [SerializeField] Axis ax;
    [SerializeField] float speed=40f;
    Vector3 axPivot;
    // Use this for initialization
    void Start ()
    {
        switch (ax)
        {
            case Axis.X:
                axPivot = Vector3.right;
                break;
            case Axis.Y:
                axPivot = Vector3.up;
                break;
            case Axis.Z:
                axPivot = Vector3.forward;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(axPivot, Time.deltaTime * speed);
	}
}
