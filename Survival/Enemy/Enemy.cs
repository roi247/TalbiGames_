using UnityEngine;
using System.Collections;


namespace SurvivalGame
{
    public class Enemy : MonoBehaviour
    {
        public EnemyAttack enemyAttack;
        [SerializeField]
        NavMeshAgent navAgent;
        [SerializeField]
        float health = 100f;
        SurvivalPlayer targetPlayer;
        [SerializeField]
        float attackPlayerRaious = 5f;
        [SerializeField]
        float timeBetweenAttacks;

        public Color debugGizmoColor;

        float _attackTimer = 0f;
        // Use this for initialization
        void Start()
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
        void Update()
        {
            FollowLocalPlayer();

            _attackTimer += Time.deltaTime;
            if (_attackTimer < timeBetweenAttacks && Vector3.Distance(targetPlayer.transform.position, this.transform.position) < attackPlayerRaious)
            {
                _attackTimer = 0f;
                enemyAttack.Attack();
            }

        }

        void OnDrawGizmos()
        {
            //ATTACK ZONE
            Gizmos.color = debugGizmoColor;
            Gizmos.DrawSphere(this.gameObject.transform.position, attackPlayerRaious);


            //RAY FROM ENEMY TO PLAYER
            if (targetPlayer != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(this.gameObject.transform.position, targetPlayer.transform.position - this.gameObject.transform.position);
            }

        }

    }

}

