using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Attacks
{
    /**
     * This interface represents a generic attack
     * All the player attacks should implement
     * this interface.
     */
    public interface IAttack 
    {
        /**
         * Should instanciate the attack prefab, add the player as parent
         * and perform the attack, this includes starting the animation for example
         */
        //public void PerformAttack(GameObject player);

        /**
         * Returns the damage of the attack as an integer
         */
        int GetDamage();
    }
}