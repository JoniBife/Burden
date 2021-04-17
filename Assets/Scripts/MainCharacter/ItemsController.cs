using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
using Assets.Scripts.Common;
using System;

namespace Assets.Scripts.MainCharacter
{
    public class ItemsController : MonoBehaviour, IPausable
    {

        [SerializeField]
        private float _playInstrumentCooldown;
        [SerializeField]
        private float _playinstrumentSanityIncrease;

        private float _playInstrumentCooldownTime;
        private SanityManager _sanityManager;

        private MovementController2D _controller2D;
        private AttackController _attackController;

        private Light2D _torch;
        private bool _torchActive = false;

        private bool _torchDisabled = false;
        private bool _instrumentDisabled = false;

        private Animator _animator;

        private bool _paused;

        // Use this for initialization
        void Start()
        {
            Light2D torch = GameObject.Find("Torch").GetComponent<Light2D>();
            if (torch == null)
            {
                throw new ArgumentNullException("Torch is not attached to player!");
            }
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                throw new ArgumentNullException("Animator is not associated with player!");
            }
            _sanityManager = GetComponent<SanityManager>();
            if (_sanityManager == null)
            {
                throw new ArgumentNullException("Sanity manager is not associated with player!");
            }
            _controller2D = GetComponent<MovementController2D>();
            if (_controller2D == null)
            {
                throw new ArgumentNullException("Controller 2D is not associated with player!");
            }
            _attackController = GetComponent<AttackController>();
            if (_attackController == null)
            {
                throw new ArgumentNullException("Attack controller is not associated with player!");
            }

            _instrumentDisabled = SharedInfo.LostInstrument;
            _torchDisabled = SharedInfo.LostTorch;

            _torch = torch;
            DeactivateTorch();
        }

        // Update is called once per frame
        void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                if (!_paused)
                {
                    if (!_torchDisabled && Input.GetKeyDown(KeyCode.Y))
                    {
                        if (_torchActive)
                            DeactivateTorch();
                        else
                            ActivateTorch();
                    }

                    if (!_instrumentDisabled && Input.GetKeyDown(KeyCode.P) && _controller2D.OnGround())
                    {
                        PlayInstrument();
                    }
                }
            }
        }

        public void ActivateTorch()
        {
            _torchActive = true;
            _torch.enabled = true;
            _animator.SetBool(AnimatorParameter.TORCH, true);
        }

        private void DeactivateTorch()
        {
            _torchActive = false;
            _torch.enabled = false;
            _animator.SetBool(AnimatorParameter.TORCH, false);
        }

        public void RemoveTorch()
        {
            _torchDisabled = true;
            /*if (_torchActive)
                DeactivateTorch();*/
            SharedInfo.LostTorch = true;
        }

        private void PlayInstrument()
        {
            if (Time.time > _playInstrumentCooldownTime)
            {
                _animator.SetTrigger(AnimatorParameter.PLAY_INSTRUMENT);
                _playInstrumentCooldownTime = Time.time + _playInstrumentCooldown;
                ((IPausable)_controller2D).Pause(); // This cast will always succeed, so no need to use as operator
                ((IPausable)_attackController).Pause(); // This cast will always succeed, so no need to use as operator
                Pause();
            }
        }

        private void FinishPlayingInstrument()
        {
            _sanityManager.IncreaseSanity(_playinstrumentSanityIncrease);
            ((IPausable)_controller2D).Resume(); // This cast will always succeed, so no need to use as operator
            ((IPausable)_attackController).Resume(); // This cast will always succeed, so no need to use as operator
            Resume();
        }

        public void RemoveInstrument()
        {
            _instrumentDisabled = true;
            SharedInfo.LostInstrument = true;
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

    }
}