using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Enemy.MchawiBoss
{
    public class ProjectileAttack : EnemyAttack
    {

        [SerializeField]
        private float _projectileDamage;

        [SerializeField]
        private float _maxProjectileSpeed;
        [SerializeField]
        private float _minProjectileSpeed;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _scaleChangeSpeed;

        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float _randomDirProbability;

        [SerializeField]
        private float _lifeTime;

        private GameObject _player;

        private Vector3 _dir;

        private float _projectileSpeed;

        private float _destructionTime;

        public override float GetDamage()
        {
            return _projectileDamage;
        }

        void Start()
        {
            // If the random is below the probability then we throw the projectile in a random dir within a circle
            /*if (UnityEngine.Random.Range(0.0f, 100.0f) < _randomDirProbability)
            {
                Vector2 randomPoint = UnityEngine.Random.insideUnitCircle;
                Vector2 dir = randomPoint - Vector2.zero;
                _dir = new Vector3(dir.x, dir.y, 0.0f);
            }
            else // We throw the projectile in the direction of the player
            {*/
                _player = GameObject.Find("Player");
                if (_player == null)
                    throw new ArgumentNullException("Could not find the player");
                _dir = (_player.transform.position - transform.position).normalized;
            //}
            
            _projectileSpeed = UnityEngine.Random.Range(_minProjectileSpeed, _maxProjectileSpeed);
            _destructionTime = Time.time + _lifeTime;
        }

        private void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                transform.Translate(_dir * _projectileSpeed * Time.deltaTime);
                //transform.localEulerAngles += ((new Vector3(0.0f, 0.0f, 90.0f)) * Time.deltaTime);
                //transform.localScale *= _scaleChangeSpeed * Time.deltaTime;

                if (Time.time > _destructionTime)
                    GameObject.Destroy(this.gameObject);
            }
        }
    }
}