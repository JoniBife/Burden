using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemy.KujengaBoss
{
    public class KujengaMovement : MonoBehaviour
    {
        [SerializeField]
        private float _maxAcceleration = 1.0f;

        [SerializeField]
        private float _slowRadius = 3.0f;

        [SerializeField]
        private float _stopRadius = 1.0f;

        [SerializeField]
        private float _maxSpeed = 5.0f;

        [SerializeField]
        private float _timeToDesiredSpeed = 0.5f;

        // Arrive Target
        private GameObject _arriveTarget; // In this case the player 

        private Rigidbody2D _rb2D;

        public bool _following = false;

        private bool _facingRight = true;

        // Use this for initialization
        void Start()
        {
            _arriveTarget = GameObject.FindGameObjectWithTag("Player");
            _rb2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_following && _arriveTarget != null)
            {
                // First we calculate the desired velocity 
                Vector2 targetlinearVelocity = DynamicArrive();

                // Finally we calculate our velocity based on our current velocity and our target velocity
                VelocityMatch(targetlinearVelocity);

                // Ensures that the character faces the direction of movement
                FaceDirection(targetlinearVelocity);

            }
        }

        private Vector2 DynamicArrive()
        {
            // Movement direction
            Vector3 direction = this._arriveTarget.transform.position - this.transform.position;
            direction.z = 0; // We don't consider the z axis since we are in 2D

            float distance = direction.magnitude;

            float targetSpeed;

            if (distance < _stopRadius) // If we are within the stop radius we stop
            {
                targetSpeed = 0.0f;
            }
            else if (distance < _slowRadius) // If we are within the slow radius we slow down the closer we are to the player
            {
                targetSpeed = _maxSpeed * (distance / _slowRadius);
            }
            else
            {
                targetSpeed = _maxSpeed; // Otherwise we move at full speed
            }

            return targetSpeed * direction.normalized;
        }

        private void VelocityMatch(Vector2 targetLinearVelocity)
        {

            if (targetLinearVelocity.x != 0)
            {
                Vector2 linearVelocity = _rb2D.velocity;

                Vector2 desiredAcceleration = (targetLinearVelocity - linearVelocity) / _timeToDesiredSpeed;

                if (desiredAcceleration.magnitude > _maxAcceleration)
                {
                    desiredAcceleration = desiredAcceleration.normalized * _maxAcceleration;
                }

                _rb2D.velocity = new Vector2(/*_rb2D.velocity.x +*/ desiredAcceleration.x, _rb2D.velocity.y);
            }
        }


        public void FaceDirection(Vector2 direction)
        {
            if (_facingRight && direction.x < 0)
            {
                _facingRight = false;
                Flip();
            }
            else if (!_facingRight && direction.x > 0)
            {
                _facingRight = true;
                Flip();
            }
        }

        /**
        * Flips the boss's sprite direction
        */
        private void Flip()
        {
            Vector3 currLocalScale = transform.localScale;
            currLocalScale.x *= -1;
            transform.localScale = currLocalScale;
        }

        public void StopFollowing() { _following = false; }
        public void StartFollowing() { _following = true; }
    }
}