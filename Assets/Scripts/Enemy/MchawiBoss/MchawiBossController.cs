using Assets.Scripts.Common;
using Assets.Scripts.Dialog;
using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.MchawiBoss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MchawiBossController : MonoBehaviour
{
    private EnemyHealthManager _healthManager;
    private MchawiMovement _mchawiMovement;
    private Interactable _interactable;
    private CapsuleCollider2D _capsuleCollider2D;

    private void Start()
    {
        // If Kujenga boss has been defeated the game object is deactivated
        if (!SharedInfo.MchawiBossDefeated)
            this.gameObject.SetActive(SharedInfo.CurseStarted);
        else
            this.gameObject.SetActive(false);

        _healthManager = GetComponent<EnemyHealthManager>();
        if (_healthManager == null)
            throw new MissingComponentException("Mchawi Boss is missing the EnemyHealthManager component!");

        _mchawiMovement = GetComponent<MchawiMovement>();
        if (_mchawiMovement == null)
            throw new MissingComponentException("Mchawi Boss is missing the KujengaMovement component!");

        _interactable = GetComponent<Interactable>();
        if (_interactable == null)
            throw new MissingComponentException("Mchawi Boss is missing the Interactable component!");

        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        if (_capsuleCollider2D == null)
            throw new MissingComponentException("Mchawi Boss is missing the capsule collider 2D component!");

        Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<CapsuleCollider2D>(), _capsuleCollider2D, true);
    }

    public void StartBossFight()
    {
        _healthManager.ActivateHealthBar();
        _mchawiMovement.StartTeleporting();
        _interactable.StopInteraction();
        GameObject.Find("AudioManager").GetComponent<SoundManager>().setChosen(1);
    }

    // Called when the boss dies
    public void OnMchawiBossDeath()
    {
        SharedInfo.MchawiBossDefeated = true;
        GameObject.Destroy(this.gameObject);
        GameObject.Find("AudioManager").GetComponent<SoundManager>().setChosen(0);
        //TODO Maybe do other stuff like play dialog
    }
}
