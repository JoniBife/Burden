using UnityEngine;
using UnityEditor;
using System;

namespace Assets.Scripts.Enemy.MchawiBoss
{
    public class ExplosionAttack : MonoBehaviour
    {

        [SerializeField]
        private float _explosionAttackDamage;

        private void OnParticleCollision(GameObject other)
        {
            SanityManager sanityManager = other.GetComponent<SanityManager>();

            if (sanityManager != null)
                sanityManager.DecreaseSanity(_explosionAttackDamage);
        }

    }
}