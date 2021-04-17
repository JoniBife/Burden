using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemy
{
    /**
     * This abstract class represents a generic enemy attack
     * All the enemy attacks should extend this abstract class;
     */
    public abstract class EnemyAttack : MonoBehaviour
    {
        public abstract float GetDamage();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SanityManager sanityManager = collision.gameObject.GetComponent<SanityManager>();

            // If the player collides with the enemy attack, decrease its sanity
            if (sanityManager != null)
            {
                sanityManager.DecreaseSanity(GetDamage());
            }
        }
    }
}