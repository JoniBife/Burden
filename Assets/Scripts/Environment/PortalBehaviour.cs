using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Common;
using UnityEngine.UI;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PortalBehaviour : MonoBehaviour
    {
        public Text portalText;

        private Animator _animator;
        private GameObject _player;

        [SerializeField]
        private float _triggerPortalRadius;
        [SerializeField]
        private float _usePortalRadius;

        [SerializeField]
        private Transform _teleportLocation;

        [SerializeField]
        public bool _openIfCurse = false;

        private bool _portalOpen = false;

        // Use this for initialization
        void Start()
        {
            _player = GameObject.Find("Player");
            if (_player == null) {
                throw new ArgumentNullException("Cannot start portal without player in scene");
            }
            _animator = GetComponent<Animator>();

            if (_openIfCurse && !SharedInfo.CurseStarted)
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            float distToPlayer = (transform.position - _player.transform.position).magnitude;

            if (_portalOpen)
            {
                if (distToPlayer < _usePortalRadius)
                {
                    portalText.gameObject.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.T))
                        _player.transform.position = _teleportLocation.position;
                }
                else
                {
                    portalText.gameObject.SetActive(false);
                }
            } 

            if (!_portalOpen && distToPlayer <= _triggerPortalRadius)
            {
                _animator.SetBool(AnimatorParameter.PORTAL_OPEN, true);
            }
            else if (_portalOpen && distToPlayer > _triggerPortalRadius)
            {
                _portalOpen = false;
                _animator.SetBool(AnimatorParameter.PORTAL_OPEN, false);
            }
            
        }

        // Called when the Portal is completely open
        private void OnPortalOpenAnimationComplete()
        {
            _portalOpen = true;
        }
    }
}
