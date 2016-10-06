using UnityEngine;
using System.Collections;

namespace MultiplayerFps
{
    public class WeaponSpawnPoint : MonoBehaviour
    {
        [SerializeField]
        Color gizmoColor;
        public bool isTaken;
        public int spawnPointIndex;
        public PlayerWeapon containedWeapon;
        [SerializeField]
        float gizmoSize = 0.5f;

        public PlayerWeapon prefferableWeapon = null;
        // Use this for initialization

        public void Empty()
        {
            isTaken = false;
            if (containedWeapon != null)
                Destroy(containedWeapon.gameObject);
            containedWeapon = null;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(this.transform.position, gizmoSize);
        }

    }
}

