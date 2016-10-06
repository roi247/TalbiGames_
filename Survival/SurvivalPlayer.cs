using UnityEngine;
using System.Collections;

public class SurvivalPlayer : MonoBehaviour {

    [SerializeField] float health=100f;
    [SerializeField] uint money;

	// Use this for initialization
	void Start () {
	
	}
	
    public float GetCurrentHealth()
    {
        return health;
    }
}
