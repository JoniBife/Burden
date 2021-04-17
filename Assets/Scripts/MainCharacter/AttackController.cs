using Assets.Scripts.Attacks;
using Assets.Scripts.Common;
using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour, IPausable
{
    private GameObject _attacksContainer;

    [SerializeField]
    private float _slashAnimationSpeed = 1.3f;
    [SerializeField]
    private float _slashStartDelay = 0.3f; // The delay in seconds of the slash woosh
    [SerializeField]
    private Vector3 _slashOffset = new Vector3(-1.6f, 0.2303254f, 0); // The offset from the player where the slash starts
    public SlashAttack slashAttackPrefab;

    private Animator _characterAnimator;

    private MovementController2D _movementController2D;

    private bool _paused = false;

    [SerializeField]
    private float _attacksCooldown;
    private float _attackStartTime;

    // Start is called before the first frame update
    void Start()
    {
        _attacksContainer = GameObject.Find("AttacksContainer");
        _characterAnimator = this.GetComponent<Animator>();
        _characterAnimator.SetFloat(AnimatorParameter.SLASH_SPEED, _slashAnimationSpeed);
        _movementController2D = this.GetComponent<MovementController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuController.gameIsPaused)
        {
            if (!_paused)
            {
                // Is the attack not in cooldown and was an attack performed?
                if (_attackStartTime == 0)
                {
                    if (HandleInputAndPerformAttack())
                    {
                        _attackStartTime = Time.time;
                    }
                }
                else
                {
                    // An attack is being performed so we check if cooldown time has passed
                    float timeSinceLastAttack = Time.time - _attackStartTime;
                    if (timeSinceLastAttack >= _attacksCooldown)
                    {
                        _attackStartTime = 0;
                    }
                }
            }
        }
    }

    private bool HandleInputAndPerformAttack()
    {
        if (Input.GetKeyDown(KeyCode.O) && _movementController2D.OnGround())
        {
            
            _characterAnimator.SetTrigger(AnimatorParameter.SLASH);
            StartCoroutine(PerformSlashWithDelay(_slashStartDelay));
            return true;
        }

        return false;
    }

    IEnumerator PerformSlashWithDelay(float delayS)
    {
        // Waiting delayS seconds to start the attack
        yield return new WaitForSeconds(delayS);

        float cr = GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        // Calculating the offset relative to the player for attack initial position
        Vector3 ajustedSlashOffSet = _slashOffset;
        float signCharacterLocalScale = Mathf.Sign(this.transform.localScale.x);
        ajustedSlashOffSet.x *= signCharacterLocalScale * cr;

        // Instantiating the attack prefab
        SlashAttack slashAttack = Instantiate(
            slashAttackPrefab,
            this.transform.position + ajustedSlashOffSet,
            Quaternion.Euler(new Vector3(0, 0, 0)
        ));

        // Updating the parent
        slashAttack.transform.parent = _attacksContainer.transform;

        // Setting the correct animation speed
        slashAttack.GetComponent<Animator>().SetFloat(AnimatorParameter.SLASH_SPEED, _slashAnimationSpeed);

        // Finally ajusting the local scale so that the attack is facing the characters movement direction     
        Vector3 currentSlashScale = slashAttack.transform.localScale;   
        if (Mathf.Sign(currentSlashScale.x) != signCharacterLocalScale * cr)
        {
            currentSlashScale.x *= signCharacterLocalScale * cr;
            slashAttack.transform.localScale = currentSlashScale;
        }
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
