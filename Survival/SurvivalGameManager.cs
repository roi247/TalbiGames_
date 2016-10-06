using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalGame
{
    public class SurvivalGameManager : MonoBehaviour
    {
        public static SurvivalGameManager Instance;
        //[SerializeField] GameObject 
        // Use this for initialization
        [SerializeField] List<Enemy> enemyList;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            enemyList = new List<Enemy>();
        }

        public void AddEnemyToList(Enemy enemy)
        {
            enemyList.Add(enemy);
        }

        public void RemoveEnemyFromList(Enemy enemy)
        {
            enemyList.Remove(enemy);
        }
        // Update is called once per frame

    }
}

