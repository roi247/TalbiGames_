using UnityEngine;
using System.Collections;

public class DistManager : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.End))
        {
            Debug.Log("DIST FROM THIS TO PLAYER IS: " + Vector3.Distance(this.gameObject.transform.position,GameManager.instance.playersList[0].gameObject.transform.position));
        }
	}
}
