using UnityEngine;
using System.Collections;

public class DestroyOnTouch : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
