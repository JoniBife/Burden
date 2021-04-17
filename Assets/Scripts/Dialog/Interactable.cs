using UnityEngine;
using UnityEditor;
using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.Dialog
{
    public class Interactable : MonoBehaviour, IPausable
    {
        [SerializeField]
        private KeyCode _interactKey;

        [SerializeField]
        private bool _singleInteraction;

        [SerializeField]
        private bool _triggerAutomatically;

        [SerializeField]
        private float _interactionRadius;

        [SerializeField]
        private GameObject _interactionIcon;

        [SerializeField]
        private GameEvent _onDialogEnd;

        public Dialog[] dialog;

        private GameObject _player;

        private DialogManager _dialogManager;

        private bool _paused = false;

        private void Start()
        {
            _player = GameObject.Find("Player");
            _dialogManager = FindObjectOfType<DialogManager>();
        }

        private void Update()
        {
            if (!PauseMenuController.gameIsPaused)
            {
                if (!_paused)
                {
                    Vector3 direction = this.transform.position - _player.transform.position;

                    // Is the user within the interaction radius
                    if (!_dialogManager.PlayingDialog && direction.magnitude <= _interactionRadius)
                    {
                        ShowInteractionIcon();
                        if (Input.GetKeyDown(_interactKey) || _triggerAutomatically)
                        {
                            _dialogManager.PlayDialog(this, _onDialogEnd, _singleInteraction);
                            if (_singleInteraction)
                            {
                                _paused = true;
                                HideInteractionIcon();
                            }
                        }
                    }
                    else
                    {
                        // User is outside the interaction radius so we hide the icon
                        HideInteractionIcon();
                    }
                }
            }

        }

        private void ShowInteractionIcon()
        {
            if (_interactionIcon != null)
            {
                _interactionIcon.SetActive(true);
            }
        }
        private void HideInteractionIcon()
        {
            if (_interactionIcon != null)
            {
                _interactionIcon.SetActive(false);
            }
        }

        public void StopInteraction()
        {
            HideInteractionIcon();
            enabled = false;
        }

        void IPausable.Pause()
        {
            _paused = true;
        }

        void IPausable.Resume()
        {
            if (!_singleInteraction)
                _paused = false;
        }
    }
}