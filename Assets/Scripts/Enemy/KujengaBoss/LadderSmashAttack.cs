using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemy.KujengaBoss
{
    public class LadderSmashAttack : EnemyAttack
    {
        [SerializeField]
        private int _ladderSmashDamage;

        private BoxCollider2D _ladderSmashCollider;
        public ParticleSystem _smashParticles;

        public override float GetDamage()
        {
            return _ladderSmashDamage;
        }

        // Use this for initialization
        void Start()
        {
            _ladderSmashCollider = GetComponent<BoxCollider2D>();
            _ladderSmashCollider.enabled = false;
            _smashParticles.Stop();
        }

        // Called by the animation event in LadderSmash
        public void ActivateAOECollider()
        {
            _ladderSmashCollider.enabled = true;
        }

        // Called by the animation event in LadderSmash
        public void DeActivateAOECollider()
        {
            _ladderSmashCollider.enabled = false;
            _smashParticles.Play();

        }
    }
}