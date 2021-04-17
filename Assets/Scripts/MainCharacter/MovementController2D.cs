using Assets.Scripts.Common;
using Assets.Scripts.Enemy.KujengaBoss;
using System;
using System.Collections;
using UnityEngine;

public class MovementController2D : MonoBehaviour, IPausable
{
    ///////////////// PARAMETERS /////////////////
    [SerializeField]
    private float _runSpeed = 9f;
    [SerializeField]
    private float _jumpSpeed = 10;
    [SerializeField]
    private int _jumpsAvailable = 1;
    [SerializeField]
    private float _digCoolDown;
    [SerializeField]
    private float _digDuration;
    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private float _wallClimbSlideSpeed;
    [SerializeField]
    private float _defaultGravityScale;
    [SerializeField]
    private float _onWallGravityScale;
    [SerializeField]
    private float _fallingThreshold;
    //////////////////////////////////////////////

    // Components
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider2D;
    private Animator _animator;
    private AttackController _attackController;
    private GameObject _headMarker; // Used to check whether the player has enough space to leave burrow
    private GameObject _feetMarker; // Used to detect whether the player is on the floor;
    private GameObject _rightMarker; // Used to detect if the right of the character is colliding with a wall
    private GameObject _leftMarker; // Used to detect if the left of the character is colliding with a wall

    private int _currJumpsAvailable;
    private Vector2 _runningDirection;
    private bool _facingRight;
    public bool CanJump = true;
    private bool _jumping = false;
    private bool _onGround = false;
    private bool _canFitHoleBody = true; // Indicates whether the player has enough space to fit his whole body within the level
    private bool _onWall = false;
    private bool _falling = false;
    private bool _isDigging = false;

    // Start is called before the first frame update
    void Start()
    {

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            throw new ArgumentNullException("Animator is NULL!");
        }

        _rb = gameObject.GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            throw new ArgumentNullException("Regidbody is NULL!");
        }

        _capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        if (_capsuleCollider2D == null)
        {
            throw new ArgumentNullException("CapsuleCollider is NULL!");
        }

        _attackController = gameObject.GetComponent<AttackController>();
        if (_attackController == null)
        {
            throw new ArgumentNullException("Attack controller is NULL");
        }

        _headMarker = GameObject.Find("HeadMarker");
        if (_headMarker == null)
        {
            throw new ArgumentNullException("HeadMarker is not associated with the player");
        }

        _feetMarker = GameObject.Find("FeetMarker");
        if (_feetMarker == null)
        {
            throw new ArgumentNullException("FeetMarker is not associated with the player");
        }

        _leftMarker = GameObject.Find("LeftMarker");
        if (_headMarker == null)
        {
            throw new ArgumentNullException("LeftMarker is not associated with the player");
        }

        _rightMarker = GameObject.Find("RightMarker");
        if (_feetMarker == null)
        {
            throw new ArgumentNullException("RightMarker is not associated with the player");
        }

        _facingRight = GetComponent<SpriteRenderer>().flipX;

        _currJumpsAvailable = _jumpsAvailable;

        _animator.SetFloat(AnimatorParameter.RUNNING_SPEED, _runSpeed);

        CanJump = true;

        if (SharedInfo.PlayerStartingPosition != Vector3.zero)
            transform.position = SharedInfo.PlayerStartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuController.gameIsPaused)
        {
            if (!_isPaused)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    Dig();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                Move();
            }
        }
    }

    public bool OnGround()
    {
        return _onGround;
    }

    public bool OnWall()
    {
        return _onWall;
    }

    private void Dig()
    {
        if (_onGround && !_falling)
        {

            CanJump = false;
            _attackController.Pause(); // Player cannot attack while digging

            // If the KujangaBoss is active we need to ignore collisions with it when digging
            GameObject kujengaBoss = GameObject.Find("KujengaBoss");
            if (kujengaBoss != null && kujengaBoss.GetComponent<KujengaBossController>().StartedBossFight)
            {
                Physics2D.IgnoreCollision(kujengaBoss.GetComponent<CapsuleCollider2D>(), _capsuleCollider2D, true);
            }

            _animator.SetBool(AnimatorParameter.DIG, true);
            _isDigging = true;
            ResizeColliderToDig();
            StartCoroutine(StopDigAfterCooldown());
        }
    }

    public IEnumerator StopDigAfterCooldown()
    {
        float endTime = Time.time + _digDuration;

        // Only stop digging if the duration is over, the character is no longer on the ground and it has to have enough space to regrow
        while ((Time.time < endTime && _onGround) || !_canFitHoleBody) { yield return null; }

        _animator.SetBool(AnimatorParameter.DIG, false);

        // If the KujangaBoss is active we need to stop ignoring collisions with it when we stop digging
        GameObject kujengaBoss = GameObject.Find("KujengaBoss");
        if (kujengaBoss != null && kujengaBoss.GetComponent<KujengaBossController>().StartedBossFight)
        {
            Physics2D.IgnoreCollision(GameObject.Find("KujengaBoss").GetComponent<CapsuleCollider2D>(), _capsuleCollider2D, false);
        }

        ResizeColliderToNormal();

        CanJump = true;
        _isDigging = false;
        _attackController.Resume();
    }

    private void ResizeColliderToDig()
    {
        _capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
        _capsuleCollider2D.offset = new Vector2(0.0f, -0.4084032f);
        _capsuleCollider2D.size = new Vector2(0.2837055f, 0.1186805f);
    }
    private void ResizeColliderToJump()
    {
        _capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
        _capsuleCollider2D.offset = new Vector2(-0.01597548f, 0.2990408f);
        _capsuleCollider2D.size = new Vector2(0.5832944f, 1.003672f);
    }

    private void ResizeColliderToNormal()
    {
        _capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
        _capsuleCollider2D.offset = new Vector2(-0.01597548f, 0.04847789f);
        _capsuleCollider2D.size = new Vector2(0.5832944f, 1.003672f);
    }

    private void Jump()
    {
        if (CanJump && _currJumpsAvailable > 0)
        {
            _currJumpsAvailable -= 1;
            _jumping = true;
        }
    }

    // When the character jump animation starts we can set the animator parameter to false
    private void InMidAir()
    {
        _animator.SetBool(AnimatorParameter.JUMP, false);
    }

    private void Move()
    { 
        // Any horizontal Input (arrows, A,D..)
        float horizontalInput = Input.GetAxis("Horizontal");

        _runningDirection = new Vector2(horizontalInput, 0);

        // Face the character sprite to the movement direction
        if (horizontalInput != 0)
        {
            _animator.SetBool(AnimatorParameter.RUNNING, true);

            if (horizontalInput > 0 && !_facingRight)
            {
                _facingRight = true;
                Flip();

            }
            else if (horizontalInput < 0 && _facingRight)
            {
                _facingRight = false;
                Flip();
            }

        }
        else
        {
            _animator.SetBool(AnimatorParameter.RUNNING, false);
        }
    }

    private bool _isJumping = false;

    /**
     * The velocity vector of the rigidbody represents the rate of change of the Rigidbody's position.
     * Since we are affecting the rigidbody it has to be done in the FixedUpdate as mentioned in docs.
     */
    private void FixedUpdate()
    {
        if (_jumping)
        {
            // Its important to mention this is ignoring all other physics updates, but only for a single update
            // so it should not be a problem
            _rb.gravityScale = _defaultGravityScale;
            _rb.velocity = Vector2.up * _jumpSpeed;
            _jumping = false;
            _isJumping = true;
            _animator.SetBool(AnimatorParameter.JUMP, true);
            ResizeColliderToJump();
        }

        _canFitHoleBody = !Physics2D.OverlapCircle(_headMarker.transform.position, 0.2f, _whatIsGround);

        bool onGround = Physics2D.OverlapCircle(_feetMarker.transform.position, 0.2f, _whatIsGround);

        // Update the animations and _onground state only if it changed 
        if (onGround != _onGround)
        {
            _onGround = onGround;
            _currJumpsAvailable = _onGround ? _jumpsAvailable : _currJumpsAvailable;
            _animator.SetBool(AnimatorParameter.ON_GROUND, _onGround);
            if (_onGround)
                ResizeColliderToNormal();
        }

        bool isFalling = _rb.velocity.y < _fallingThreshold;

        // Update the animations and _isFalling state only if it changed 
        if (isFalling != _falling)
        {
            // If its falling but really close to the floor there is no need to play the animation
            if ((isFalling && !_onGround) || !isFalling)
            {
                _falling = isFalling;
                _animator.SetBool(AnimatorParameter.FALLING, _falling);
            }
        }

        // We ignore walls checks when digging
        if (!_isDigging)
        {
            bool onWall = !_onGround && Physics2D.OverlapCircle(_leftMarker.transform.position, 0.2f, _whatIsGround)
                || Physics2D.OverlapCircle(_rightMarker.transform.position, 0.2f, _whatIsGround);

            // Update the animations and _onWall state only if it changed 
            if (onWall != _onWall)
            {
                _onWall = onWall;
                _rb.gravityScale = !_onWall ? _defaultGravityScale : _rb.gravityScale;
                _animator.SetBool(AnimatorParameter.ON_WALL, _onWall);
            }
        }

        _rb.velocity = new Vector2(_runningDirection.x * _runSpeed, _rb.velocity.y);
    }

    /**
     * Ensures that when the player collides
     * with a wall it not only resets the jumps
     * but also slides up
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_onWall && _isJumping && !_falling)
        {
            _rb.gravityScale = _onWallGravityScale;
            _rb.velocity = Vector2.up * _wallClimbSlideSpeed;
            _currJumpsAvailable = _jumpsAvailable;
            _isJumping = false;
        }
    }

    /**
     * Flips the character's sprite direction
     */
    private void Flip()
    {
        Vector3 currLocalScale = transform.localScale;
        currLocalScale.x *= -1;
        transform.localScale = currLocalScale;
    }

    private bool _isPaused = false;
    void IPausable.Pause()
    {
        _animator.SetBool(AnimatorParameter.RUNNING, false);
        _runningDirection = Vector2.zero;
        _runningDirection = new Vector2(0, 0);
        _currJumpsAvailable = _jumpsAvailable;
        _jumping = false;
        _isPaused = true;
    }
    void IPausable.Resume()
    {
        _isPaused = false;
    }
}
