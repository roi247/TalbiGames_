using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace SurvivalGame
{
    public class SurvivalPlayer : MonoBehaviour
    {
        [SerializeField]
        uint money;

        [SerializeField]
        float health = 100f;

        // Use this for initialization
        void Start()
        {

        }

        public float GetCurrentHealth()
        {
            return health;
        }

        public void TakeDamage(int _damage)
        {
            health -= _damage;
        }

    }
}

