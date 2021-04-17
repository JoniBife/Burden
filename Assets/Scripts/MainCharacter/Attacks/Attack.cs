using UnityEngine;
using UnityEditor;
using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.MchawiBoss;

namespace Assets.Scripts.Attacks
{
    /**
     * This abstract class represents a generic attack
     * All the player attacks should extend this abstract class;
     */
    public abstract class Attack : MonoBehaviour
    {
        /**
         * Returns the damage of the attack as an integer
         */
        public abstract int GetDamage();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyHealthManager healthManager = collision.gameObject.GetComponent<EnemyHealthManager>();

            // If the boss collided with an enemy attack, decrease its health
            if (healthManager != null)
            {
                healthManager.DecreaseHealth(GetDamage());
                //GameObject.Destroy(playerAttack.gameObject);
            }

            MchawiMovement mchawiMovement = collision.gameObject.GetComponent<MchawiMovement>();
            if (mchawiMovement != null)
            {
                mchawiMovement.OnCollisionWithAttack();
            }
        }
    }
}