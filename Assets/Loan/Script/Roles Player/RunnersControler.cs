using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunnersControler : MonoBehaviour
{
    [SerializeField] private RunnerData _runnerData;

    [SerializeField] private Transform _groundCheck;

    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private float _chargeInterval = 1f;

    [SerializeField] private float _fallMultiplier;
    [SerializeField] private LayerMask _winMask;

    public float Speed = 7f;
    public float JumpForce = 13f;
    public float OriginalSpeed;
    public float OriginalJump;
    private Animator _animator;
    private RuntimeAnimatorController _animatorController;
    private MultiplePlayerCamera _cameraScript;
    private bool _canUsePower;
    private float _currentJumpHeight;
    private int _currentPower;
    private Vector2 _gravity;
    private float _health = 3f;
    private readonly float _holdJumpForce = 7f;
    private InputSysteme _inputSysteme;
    private bool _isGrounded;
    private bool _isHoldingJump;
    private float _jumpHolderTime;
    private readonly float _maxHoldTime = 0.5f;
    private readonly float _maxJumpHeight = 20f;
    private Rigidbody2D _rb;
    private SpriteRenderer _sR;
    private Gamepad _assignedGamepad;
    private PlayerInput _playerInput;
    [SerializeField]private string _name;
    private float MaxPower => _runnerData.MaxPower;
    private Sprite _spriteRenderer => _runnerData.Sprite;

    private void Awake()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _rb = GetComponent<Rigidbody2D>();
        _cameraScript = FindObjectOfType<MultiplePlayerCamera>();
        _sR = GetComponent<SpriteRenderer>();
        _sR.sprite = _spriteRenderer;
        _playerInput = GetComponent<PlayerInput>();
        // _animator = GetComponent<Animator>();
        // _animatorController = _animator.runtimeAnimatorController;
        // _animatorController = _runnerData.AnimatorController;
        // _animator.runtimeAnimatorController = _animatorController;
    }

    private void Start()
    {
        if (_cameraScript != null) _cameraScript.AddPlayer(transform);

        // GameManager.Instance.RegisterRunner(gameObject);

        InvokeRepeating(nameof(ChargePowerUp), 0f, _chargeInterval);

        _name = _runnerData.Name;
        OriginalSpeed = Speed;
        OriginalJump = JumpForce;
        _gravity = new Vector2(0, -Physics2D.gravity.y);
        switch (_name)
        {
            case "Moine":
                _assignedGamepad = MainMenuManager.MoineID;
                break;
            case "Chasseur":
                _assignedGamepad = MainMenuManager.ChasseurID;
                break;
            case "Mage":
                _assignedGamepad = MainMenuManager.MageID;
                break;
        }

        if (_assignedGamepad == null)
        {
            Debug.LogError($"Aucun Gamepad assigné pour le Runner {_name}.");
        }
    }

    private void Update()
    {
        if (_assignedGamepad != null && _assignedGamepad == Gamepad.current)
        {
            var horizontal = _inputSysteme.Move.x;
            var velocity = _rb.velocity;
            velocity.x = horizontal * Speed;

            _rb.velocity = velocity;

            _isGrounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1f, 0.13f),
                CapsuleDirection2D.Horizontal, 0, _groundMask);

            if (_inputSysteme.Move.x > 0) _sR.flipX = true;
            // _animator.SetFloat("isWalking", Mathf.Abs(_rb.velocity.x));
            if (_inputSysteme.Move.x < 0) _sR.flipX = false;
            // _animator.SetFloat("isWalking", Mathf.Abs(_rb.velocity.x));
            HandleJump();

            if (_rb.velocity.y <= 1) _rb.velocity -= _gravity * _fallMultiplier * Time.deltaTime;
            // _animator.SetTrigger("isFalling");
            if (_canUsePower && _inputSysteme.PowerUp == 1) ActivatePowerUp();
        }
    }

    private void OnDestroy()
    {
        if (_cameraScript != null) _cameraScript.RemovePlayer(transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInWinLayer(collision.gameObject.layer)) GameManager.Instance.LoadWinRunner("RunnerWin");
    }


    public void Setup(RunnerData data, Gamepad gamepad)
    {
        _runnerData = data;
        _sR.sprite = _runnerData.Sprite;
        _assignedGamepad = gamepad;

        // if (_runnerData.AnimatorController != null)
        // {
        //     _animator.runtimeAnimatorController = _runnerData.AnimatorController;
        // }

        if (_assignedGamepad != null)
        {
            Debug.Log($"Gamepad assigné : {_assignedGamepad.displayName}");
            _inputSysteme.SwitchCurrentControlScheme(_assignedGamepad);
        }
        else
        {
            Debug.LogError("Aucun Gamepad assigné lors du Setup.");
        }
    }

    private void HandleJump()
    {
        var jumpForceTime = _inputSysteme.Jump;

        if (jumpForceTime > 0 && _isGrounded && !_isHoldingJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
            _isHoldingJump = true;
            _jumpHolderTime = 0f;
            _currentJumpHeight = 0f;
            // _animator.SetTrigger("isJump");
        }

        if (jumpForceTime > 0 && _isHoldingJump && _jumpHolderTime < _maxHoldTime &&
            _currentJumpHeight < _maxJumpHeight)
        {
            _jumpHolderTime += Time.deltaTime;
            _currentJumpHeight += _holdJumpForce * Time.deltaTime;
            _rb.velocity = new Vector2(_rb.velocity.x,
                Mathf.Clamp(_rb.velocity.y + _holdJumpForce * Time.deltaTime, 0, _maxJumpHeight));
        }

        if (jumpForceTime <= 0) _isHoldingJump = false;
    }

    private void ChargePowerUp()
    {
        if (_currentPower < MaxPower)
        {
            _currentPower++;
            Debug.Log("Charge actuelle : " + _currentPower);
        }

        if (_currentPower == MaxPower)
        {
            _canUsePower = true;
            Debug.Log("Power Up prêt !");
        }
    }

    private void ActivatePowerUp()
    {
        if (_canUsePower)
        {
            Debug.Log("Power Up activé !!");
            _runnerData.ApplyPowerUp(this);
            // _animator.SetBool("isPowerUP", true);
            _sR.color = Color.red;
            Invoke(nameof(ResetPowerUp), 4f);
        }
    }

    private void ResetPowerUp()
    {
        _runnerData.RemovePowerUp(this);
        _sR.color = Color.white;
        _currentPower = 0;
        _canUsePower = false;
        // _animator.SetBool("isPowerUP", false);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        Debug.Log($"Runner a pris {damage} damage. Vie restantes : {_health}");

        if (_health > 0)
        {
            // _animator.SetTrigger("isHit");
        }

        if (_health <= 0)
            // _animator.SetTrigger("isDead");
            Invoke(nameof(Die), 2f);
    }

    private void Die()
    {
        GameManager.Instance.UnregisterRunner(gameObject);
        Debug.Log("Runner mort.");
        Destroy(gameObject);
    }

    private bool IsInWinLayer(int layer)
    {
        return ((1 << layer) & _winMask) != 0;
    }
}