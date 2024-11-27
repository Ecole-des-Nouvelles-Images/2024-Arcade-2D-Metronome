using UnityEngine;

public class RunnersControler : MonoBehaviour
{
    private bool _isGrounded; 
    private float _groundCheckRadius = 0.2f;
    private float _health = 1f;
    private Rigidbody2D _rb;
    private InputSysteme _inputSysteme;
    private MultiplePlayerCamera _cameraScript;
    private int _currentPower = 0;
    private bool _canUsePower = false;
    private SpriteRenderer _sR;
    

    [SerializeField] private RunnerData _runnerData;
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private float _chargeInterval = 1f;

    public float Speed =5f;
    public float JumpForce = 6f;
    public float OriginalSpeed;
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
    }

    private void Update()
    {
        
       float horizontal = _inputSysteme.Move.x;  
       Vector2 velocity = _rb.velocity;
       velocity.x = horizontal * Speed;

       _rb.velocity = velocity;
       
       _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);

       if (_inputSysteme.Move.x > 0 )
       {
           _sR.flipX = true;
       }

       if (_inputSysteme.Move.x < 0 )
       {
           _sR.flipX = false;
       }

       if (_isGrounded && _inputSysteme.Jump > 0)
       {
           _rb.velocity = new Vector2(_rb.velocity.x, JumpForce);
       }

       if (_canUsePower && _inputSysteme.PowerUp == 1)
       {
           ActivatePowerUp();
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

    private void OnDrawGizmos()
    {
        // Pour visualiser le point de vérification au sol dans l'éditeur
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}