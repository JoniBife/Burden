using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.Enemy.MchawiBoss
{
    public class MchawiMovement : MonoBehaviour
    {

        [SerializeField]
        private Transform[] _teleportPoints; // Possible teleport locations

        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float _teleportToPlayerProbability;

        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float _teleportCoolDown;
        private float _nextTeleportTime = 0.0f;

        [SerializeField]
        private float _attackDuration;

        [SerializeField]
        private float _explosionPreparationDuration;

        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float _explosionProbability;

        [SerializeField]
        private GameObject _projectilePrefab;
        private GameObject _projectileContainer;

        [SerializeField]
        private float _throwProjectileCoolDown;
        private float _nextProjectileTime = 0.0f;

        private bool _teleporting = false;
        private bool _performingExplosion = false;
        private bool _started = false;

        private GameObject _player;

        private Animator _animator;

        public ParticleSystem _explosionParticles;

        public void Start()
        {
            _player = GameObject.Find("Player");
            if (_player == null)
                throw new ArgumentNullException("Player is not in the scene therefore cannot create the Mchawi Boss");

            _animator = GetComponent<Animator>();
            if (_animator == null)
                throw new ArgumentNullException("Animator is not associated with the Mchawi boss");

            _projectileContainer = GameObject.Find("ProjectileContainer");
            if (_projectileContainer == null)
                throw new ArgumentNullException("ProjectileContainer is not in the scene");

            _explosionParticles.Stop();
        }

        public void StartTeleporting()
        {
            _teleporting = true;
            _started = true;
        }

        /**
         * If the random teleport probabily
         * We teleport to the closest telepoint to the player
         */
        void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                if (_started)
                {
                    if (_teleporting && Time.time > _nextTeleportTime)
                    {
                        _nextTeleportTime = Time.time + _teleportCoolDown;
                        Teleport();
                    }
                    else if (Time.time > _nextProjectileTime)
                    {
                        _nextProjectileTime = Time.time + _throwProjectileCoolDown;
                        ThrowProjectile();
                    }


                    float random = UnityEngine.Random.Range(0.0f, 100.0f);

                    if (!_performingExplosion && random < _explosionProbability)
                        StartPerformingExplosion();
                }
            }
        }

        private void TeleportAwayFromPlayer()
        {
            float random = UnityEngine.Random.Range(0.0f, 100.0f);

            Transform selectedTelePoint = _teleportPoints[UnityEngine.Random.Range(0, _teleportPoints.Length - 1)];

            float furthestDist = 0.0f; // Closest distance from telepoint to player

            Vector3 playerPosition = _player.transform.position;

            // Finding the closest telepoint thats not the exact position where the boss is at
            foreach (Transform telePoint in _teleportPoints)
            {
                float distToTelepoint = (playerPosition - telePoint.position).magnitude;
                if (telePoint.position != transform.position && distToTelepoint > furthestDist)
                {
                    selectedTelePoint = telePoint;
                    furthestDist = distToTelepoint;
                }
            }

            transform.position = selectedTelePoint.position;
        }

        private void Teleport()
        {
            float random = UnityEngine.Random.Range(0.0f, 100.0f);

            Transform selectedTelePoint = _teleportPoints[UnityEngine.Random.Range(0, _teleportPoints.Length - 1)];

            if (random < _teleportToPlayerProbability)
            {
                float closestDist = float.MaxValue; // Closest distance from telepoint to player

                Vector3 playerPosition = _player.transform.position;

                // Finding the closest telepoint thats not the exact position where the boss is at
                foreach (Transform telePoint in _teleportPoints)
                {
                    float distToTelepoint = (playerPosition - telePoint.position).magnitude;
                    if (telePoint.position != transform.position && distToTelepoint < closestDist && distToTelepoint > 3)
                    {
                        selectedTelePoint = telePoint;
                        closestDist = distToTelepoint;
                    }
                }
            }

            transform.position = selectedTelePoint.position;
        }

        private void StartPerformingExplosion()
        {
            // It first starts the animation and stops teleporting
            _animator.SetBool(AnimatorParameter.PREPARE_EXPLOSION, true);
            _teleporting = false;
            _performingExplosion = true;

            // After a small preparation delay it then performs the attack
            StartCoroutine(PerformExplosionRoutine());

        }

        private IEnumerator PerformExplosionRoutine()
        {
            yield return new WaitForSeconds(_explosionPreparationDuration);

            _animator.SetBool(AnimatorParameter.PREPARE_EXPLOSION, false);

            _explosionParticles.Play();

            yield return new WaitForSeconds(3.0f);

            _explosionParticles.Stop();

            _performingExplosion = false;

            _teleporting = true;
        }

        private void ThrowProjectile()
        {
            GameObject projectile = GameObject.Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.parent = _projectileContainer.transform;
        }

        public void OnCollisionWithAttack()
        {
            if (_teleporting && !_performingExplosion)
                TeleportAwayFromPlayer();
        }
    }
    
}