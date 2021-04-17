using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.MainCharacter
{
    public class PlayerController : MonoBehaviour
    {

        private MovementController2D _movementController2D;
        private ItemsController _itemsController;
        private AttackController _attackController;
        private SanityManager _sanityManager;

        // Use this for initialization
        void Start()
        {
            _sanityManager = GetComponent<SanityManager>();
            if (_sanityManager == null)
            {
                throw new ArgumentNullException("Sanity manager is not associated with player!");
            }
            _movementController2D = GetComponent<MovementController2D>();
            if (_movementController2D == null)
            {
                throw new ArgumentNullException("Controller 2D is not associated with player!");
            }
            _attackController = GetComponent<AttackController>();
            if (_attackController == null)
            {
                throw new ArgumentNullException("Attack controller is not associated with player!");
            }
            _itemsController = GetComponent<ItemsController>();
            if (_itemsController == null)
            {
                throw new ArgumentNullException("Items controller is not associated with player!");
            }
        }

        public void PausePlayer()
        {
            ((IPausable)_movementController2D).Pause();
            ((IPausable)_itemsController).Pause();
            ((IPausable)_attackController).Pause();
            ((IPausable)_sanityManager).Pause();
        }

        public void ResumePlayer()
        {
            ((IPausable)_movementController2D).Resume();
            ((IPausable)_itemsController).Resume();
            ((IPausable)_attackController).Resume();
            ((IPausable)_sanityManager).Resume();
        }
    }
}