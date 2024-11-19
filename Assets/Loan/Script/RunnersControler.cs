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
    
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private LayerMask _groundMask;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpForce;

    private void Awake()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _rb = GetComponent<Rigidbody2D>();
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
