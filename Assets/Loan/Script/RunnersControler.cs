using UnityEngine;

public class RunnersControler : MonoBehaviour
{
    private bool _isGrounded;
    private float _health = 1f;
    private Rigidbody2D _rb;
    private InputSysteme _inputSysteme;
    private MultiplePlayerCamera _cameraScript;
    private int _currentPower = 0;
    private bool _canUsePower = false;
    private SpriteRenderer _sR;
    private Vector2 _gravity;
    private bool _isHoldingJump;
    private float _jumpHolderTime;
    private float _currentJumpHeight;
    private float _maxHoldTime = 0.5f;
    private float _maxJumpHeight = 20f;
    private float _holdJumpForce = 7f;
    

    [SerializeField] private RunnerData _runnerData;
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private float _chargeInterval = 1f;
    [SerializeField] private float _fallMultiplier;

    public float Speed =7f;
    public float JumpForce = 13f;
    public float OriginalSpeed;
    public float OriginalJump;
    private float MaxPower => _runnerData.MaxPower;
    private Sprite _spriteRenderer => _runnerData.Sprite;
    
    private void Awake()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _rb = GetComponent<Rigidbody2D>();
        _cameraScript = FindObjectOfType<MultiplePlayerCamera>();
       _sR = GetComponent<SpriteRenderer>();
       _sR.sprite = _spriteRenderer;
    }

    private void Start()
    {
        if (_cameraScript != null )
        {
            _cameraScript.AddPlayer(transform);
        }
      
        InvokeRepeating(nameof(ChargePowerUp),0f,_chargeInterval);

        OriginalSpeed = Speed;
        OriginalJump = JumpForce;
        _gravity = new Vector2(0,-Physics2D.gravity.y);
    }

    private void Update()
    {
        
       float horizontal = _inputSysteme.Move.x;  
       Vector2 velocity = _rb.velocity;
       velocity.x = horizontal * Speed;

       _rb.velocity = velocity;
       
       _isGrounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(1f, 0.13f),CapsuleDirection2D.Horizontal,0, _groundMask);

       if (_inputSysteme.Move.x > 0 )
       {
           _sR.flipX = true;
       }

       if (_inputSysteme.Move.x < 0 )
       {
           _sR.flipX = false;
       }
       
       HandleJump();

       // if (_isGrounded && _inputSysteme.Jump > 0)
       // {
       //     // _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
       //     HandleJump();
       // }

       if (_rb.velocity.y <= 1)
       {
           _rb.velocity -= _gravity * _fallMultiplier * Time.deltaTime;
       }

       if (_canUsePower && _inputSysteme.PowerUp == 1)
       {
           ActivatePowerUp();
       }
    }

    private void HandleJump()
    {
       float jumpForceTime = _inputSysteme.Jump;

       if (jumpForceTime > 0 && _isGrounded && !_isHoldingJump)
       {
           _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
           _isHoldingJump = true;
           _jumpHolderTime = 0f;
           _currentJumpHeight = 0f;
       }

       if (jumpForceTime > 0 && _isHoldingJump && _jumpHolderTime < _maxHoldTime && _currentJumpHeight < _maxJumpHeight )
       {
           _jumpHolderTime += Time.deltaTime;
           _currentJumpHeight += _holdJumpForce * Time.deltaTime;
           _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y + _holdJumpForce * Time.deltaTime, 0, _maxJumpHeight));
       }

       if (jumpForceTime <= 0)
       {
           _isHoldingJump = false;
       }
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
            
            Invoke(nameof(ResetPowerUp),4f);
        }
    }

    private void ResetPowerUp()
    {
        _runnerData.RemovePowerUp(this);
        _currentPower = 0;
        _canUsePower = false;
    }

    private void OnDestroy()
    {
        if (_cameraScript != null)
        {
            _cameraScript.RemovePlayer(transform);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        Debug.Log($"Runner a pris {damage} damage. Vie restantes : {_health}");

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Runner mort.");
        Destroy(gameObject);
    }
}