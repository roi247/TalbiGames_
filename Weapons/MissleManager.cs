using UnityEngine;
using System.Collections;

namespace MultiplayerFps
{
    public class MissleManager : MonoBehaviour
    {

        const string limitTag = "WorldLimit";
        [SerializeField]
        GameObject hitParticles;
        // Use this for initialization
        [SerializeField]
        AudioSource hitAudio;

        float maxDamageRange = 8f;

        float maxDamage;

        public PlayerShoot rocketOwner;


        public void SetMissileProperties(float _damage, float _range)
        {
            maxDamage = _damage;
            maxDamageRange = _range;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(limitTag))
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
                hitAudio.Play();
                GetComponent<Renderer>().enabled = false;
                DamageNearbyPlayers(this.gameObject.transform.position);

                Destroy(this.gameObject, 1.5f);
            }

        }

        void DamageNearbyPlayers(Vector3 hitPos)
        {
            foreach (Player player in GameManager.instance.playersList)
            {
                float dist = Vector3.Distance(hitPos, player.gameObject.transform.position);
                if (dist < maxDamageRange)
                {
                    //DAMAGE HIM !
                    int damage = (int)(maxDamage * (1 / (dist * dist)));

                    rocketOwner.CmdPlayerShot(player.GetComponent<Collider>().name, damage, rocketOwner.transform.name);
                    Debug.Log(player.transform.name + "[dist= " + dist.ToString() + "] GOT " + damage.ToString() + "Damage");
                }
            }
        }
    }

}
