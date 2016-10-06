using UnityEngine;
using System.Collections;

public class _DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
	}
	
}
