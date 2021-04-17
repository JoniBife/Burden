using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Assets.Scripts.Attacks
{
    public class SlashAttack : Attack
    {
        [SerializeField]
        private float _slashAcceleration;

        [SerializeField]
        private float _slashSpeed;

        [SerializeField]
        private float _maxSpeed;

        [SerializeField]
        private int _slashDamage;

        private Vector3 _direction = new Vector3(1,0,0);

        private Animator _slashAnimator;

        public override int GetDamage()
        {
            return _slashDamage;
        }

        private void Start()
        {
            _slashAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Adding acceleration
            _slashSpeed += _slashAcceleration * Time.deltaTime;

            if (_slashSpeed > _maxSpeed)
                _slashSpeed = _maxSpeed;

            // Translating the slash at _slashSpeed and calculating the direction using the sign of the localScale.X (1 or -1)
            transform.position += _direction * -Mathf.Sign(transform.localScale.x) * (_slashSpeed * Time.deltaTime);

            // Has the slash animation finished? If so delete the obj
            if (_slashAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                Destroy(this.gameObject);
            }
        }
    }
}