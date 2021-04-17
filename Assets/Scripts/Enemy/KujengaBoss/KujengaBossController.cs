using Assets.Scripts.Common;
using Assets.Scripts.Dialog;
using UnityEngine;

namespace Assets.Scripts.Enemy.KujengaBoss
{
    public class KujengaBossController : MonoBehaviour
    {
        private EnemyHealthManager _healthManager;
        private KujengaMovement _kujengaMovement;
        private KujengaAttackController _attackController;
        private Interactable _interactable;

        public GameEvent onBossDeath;

        public bool StartedBossFight = false;

        // Use this for initialization
        void Start()
        {
            // If Kujenga boss has been defeated the game object is deactivated
            if (!SharedInfo.KujengaBossDefeated)
                this.gameObject.SetActive(SharedInfo.CurseStarted);
            else
                this.gameObject.SetActive(false);

            _healthManager = GetComponent<EnemyHealthManager>();
            if (_healthManager == null)
                throw new MissingComponentException("Kujenga Boss is missing the EnemyHealthManager component!");

            _kujengaMovement = GetComponent<KujengaMovement>();
            if (_healthManager == null)
                throw new MissingComponentException("Kujenga Boss is missing the KujengaMovement component!");

            _attackController = GetComponent<KujengaAttackController>();
            if (_healthManager == null)
                throw new MissingComponentException("Kujenga Boss is missing the KujengaAttackController component!");

            _interactable = GetComponent<Interactable>();
            if (_healthManager == null)
                throw new MissingComponentException("Kujenga Boss is missing the Interactable component!");
        }

        public void StartBossFight()
        {
            _healthManager.ActivateHealthBar();
            _kujengaMovement.StartFollowing();
            _attackController.StartAttacking();
            _interactable.StopInteraction();
            StartedBossFight = true;
        }

        // Called when the boss dies
        public void OnKunjengaBossDeath()
        {
            SharedInfo.KujengaBossDefeated = true;
            GameObject.Destroy(this.gameObject);
            //TODO Maybe do other stuff like play dialog
        }
    }
}