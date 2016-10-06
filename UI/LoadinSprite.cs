using UnityEngine;
using System.Collections;

public class LoadinSprite : MonoBehaviour {
    public static LoadinSprite instance;
    // Use this for initialization
    void Awake () {
        instance = this;
	}
	
}
