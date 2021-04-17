using UnityEngine;
using System.Collections;
using Assets.Scripts.Common;

namespace Assets.Scripts.Enemy.KujengaBoss
{
    public class KujengaAttackController : MonoBehaviour
    {

        [SerializeField]
        private float _performLadderSmashRadius;

        [SerializeField]
        private float _ladderSmashRadius;

        [SerializeField]
        private float _slamProbability;

        [SerializeField]
        private float _slamRadius;

        private GameObject _attackTarget; // In this case the player 

        private bool _performingAttack = true;

        private KujengaMovement _kujengaMovement;

        private Animator _animator;

        public LadderSmashAttack LadderSmashAttack;

        // Use this for initialization
        void Start()
        {
            _attackTarget = GameObject.FindGameObjectWithTag("Player");
            _animator = GetComponent<Animator>();
            _kujengaMovement = GetComponent<KujengaMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                if (!_performingAttack)
                {
                    float random = Random.value;

                    if (random < _slamProbability)
                    {
                        _performingAttack = true;
                        _kujengaMovement.StopFollowing();
                        PerformSlam();

                    }
                    else
                    {
                        Vector3 direction = this._attackTarget.transform.position - this.transform.position;
                        float distance = direction.magnitude;
                        if (distance < _performLadderSmashRadius)
                        {
                            _performingAttack = true;
                            _kujengaMovement.StopFollowing();
                            PerformLadderSmash();
                        }
                    }
                }
            }
        }

        void PerformLadderSmash()
        {
            _kujengaMovement.FaceDirection(_attackTarget.transform.position - transform.position);
            _animator.SetTrigger(AnimatorParameter.LADDER_SMASH);
            StartCoroutine(WaitForAttackEnd("LadderSmash"));
        }

        void PerformSlam()
        {
            _animator.SetTrigger(AnimatorParameter.SLAM);
            StartCoroutine(WaitForAttackEnd("Slam"));
        }

        // Called by the animation event in LadderSmash
        public void ActivateSmashAOECollider()
        {
            LadderSmashAttack.ActivateAOECollider();
        }

        // Called by the animation event in LadderSmash
        public void DeActivateSmashAOECollider()
        {
            LadderSmashAttack.DeActivateAOECollider();
        }

        IEnumerator WaitForAttackEnd(string attackName)
        {
            while (_animator.GetCurrentAnimatorStateInfo(0).IsName(attackName)) {
                yield return null;
            }
            _performingAttack = false;
            _kujengaMovement.StartFollowing();
        }

        public void StartAttacking()
        {
            GameObject.Find("AudioManager").GetComponent<SoundManager>().setChosen(1);
            _performingAttack = false;
        }
    }
}