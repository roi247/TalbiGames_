using UnityEngine;
using System.Collections;

namespace SurvivalGame
{
    [System.Serializable]
    public class EnemyAttack
    {
        [SerializeField] int damagePerShot = 20;                  // The damage inflicted by each bullet.
        [SerializeField] float range = 100f;                      // The distance the gun can fire.
        [SerializeField] Transform rayOrigin;
        float timer;                                    // A timer to determine when to fire.
        Ray shootRay;                                   // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
        [SerializeField] ParticleSystem gunParticles;                    // Reference to the particle system.
        [SerializeField] LineRenderer gunLine;                           // Reference to the line renderer.
        [SerializeField] AudioSource gunAudio;                           // Reference to the audio source.
        [SerializeField] Light gunLight;                                 // Reference to the light component.
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.


        public void DisableEffects()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
            gunLight.enabled = false;
        }

        public void Attack()
        {
            shootableMask = LayerMask.GetMask("Shootable");

            // Play the gun shot audioclip.
            gunAudio.Play();

            // Enable the light.
            gunLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Stop();
            gunParticles.Play();

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunLine.enabled = true;
            gunLine.SetPosition(0, rayOrigin.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = rayOrigin.position;
            shootRay.direction = rayOrigin.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                SurvivalPlayer player = shootHit.collider.GetComponent<SurvivalPlayer>();

                // If the EnemyHealth component exist...
                if (player != null)
                {
                    // ... the enemy should take damage.
                    player.TakeDamage(damagePerShot);
                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine.SetPosition(1, shootHit.point);
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }
    }
}


