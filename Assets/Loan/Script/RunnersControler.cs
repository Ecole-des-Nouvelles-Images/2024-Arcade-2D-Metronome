using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnersControler : MonoBehaviour
{
    private bool _isGrounded; 
    private float _groundCheckRadius = 0.2f;
    private Rigidbody2D _rb;
    private InputSysteme _inputSysteme;
    private MultiplePlayerCamera _cameraScript;
    private int _currentPower = 0;
    private bool _canUsePower = false;
    private Renderer _characterRenderer;
    
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;
    [SerializeField] 
    private int _maxPower;
    [SerializeField]
    private float _chargeInterval = 1f;

    private void Awake()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _rb = GetComponent<Rigidbody2D>();
        _cameraScript = FindObjectOfType<MultiplePlayerCamera>();
    }

    private void Start()
    {
        if (_cameraScript != null )
        {
            _cameraScript.AddPlayer(transform);
        }
        _characterRenderer = GetComponent<Renderer>();
      
        InvokeRepeating(nameof(ChargePowerUp),0f,_chargeInterval);
    }

    private void FixedUpdate()
    {
        
       float horizontal = _inputSysteme.Move.x;  
       Vector2 velocity = _rb.velocity;
       velocity.x = horizontal * _speed;
       
       _rb.velocity = velocity;
       
       _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
       

       if (_isGrounded && _inputSysteme.Jump > 0)
       {
           _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
       }

       if (_canUsePower && _inputSysteme.PowerUp == 1)
       {
           ActivatePowerUp();
       }
    }

    private void ChargePowerUp()
    {
        if (_currentPower < _maxPower)
        {
            _currentPower++;
            Debug.Log("Charge actuelle : " + _currentPower);

            if (_currentPower == _maxPower)
            {
                _canUsePower = true;
                Debug.Log("Power Up prêt !");
            }
        }
    }

    private void ActivatePowerUp()
    {
        if (_canUsePower)
        {
            Debug.Log("Power Up activé !!");
            _characterRenderer.material.color = Color.red;
            
            _currentPower = 0;
            _canUsePower = false;
            
            Invoke(nameof(ResetPowerUp),2f);
        }
    }

    private void ResetPowerUp()
    {
        _characterRenderer.material.color = Color.white;
    }

    private void OnDestroy()
    {
        if (_cameraScript != null)
        {
            _cameraScript.RemovePlayer(transform);
        }
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
