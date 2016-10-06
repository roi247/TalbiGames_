using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] float health=100f;
    SurvivalPlayer targetPlayer;
    [SerializeField] float attackPlayerRaious=5f;
    [SerializeField] float timeBetweenAttacks;

    float _attackTimer = 0f;
    public Color debugGizmoColor;

	// Use this for initialization
	void Start ()
    {
     
    }

    void FollowLocalPlayer()
    {
        // If the enemy and the player have health left...
        if (targetPlayer != null && health > 0 && targetPlayer.GetCurrentHealth() > 0)
        {
            // ... set the destination of the nav mesh agent to the player.
            navAgent.SetDestination(targetPlayer.transform.position); //targetPlayer.transform.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            navAgent.enabled = false;
        }
    }

    public void SetTarget(SurvivalPlayer _player)
    {
        targetPlayer = _player;
    }

    // Update is called once per frame
    void Update ()
    {
        FollowLocalPlayer();

        _attackTimer += Time.deltaTime;
        if (_attackTimer < timeBetweenAttacks && Vector3.Distance(targetPlayer.transform.position,this.transform.position) < attackPlayerRaious )
        {
            _attackTimer = 0f;
            Attack();
        }

    }

    void OnDrawGizmos()
    {
        //ATTACK ZONE
        Gizmos.color = debugGizmoColor;
        Gizmos.DrawSphere(this.gameObject.transform.position, attackPlayerRaious);


        //RAY FROM ENEMY TO PLAYER
        if (targetPlayer!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.gameObject.transform.position, targetPlayer.transform.position - this.gameObject.transform.position);
        }

    }

    void Attack()
    {
        //PREFORM ATTACK!
    }


}
