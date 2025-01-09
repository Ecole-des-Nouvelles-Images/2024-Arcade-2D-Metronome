using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piege : MonoBehaviour
{
    public PiegeData PiegeData;
    private InputSysteme _inputSysteme;
    private bool _isFalling = false;
    private float _destroyTime = 3f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Cle _cleTrapp;
    private Gamepad _assignedGamepad;
    private MetronomeControler _metronomeControler;
    private PlayerInput _playerInput;


    public void Initialize(InputSysteme inputSysteme, MetronomeControler metronomeControler, PlayerInput playerInput)
    {
        _inputSysteme = inputSysteme;
        _metronomeControler = metronomeControler;
        _playerInput = playerInput;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>(); 
        }
        _spriteRenderer.sprite = PiegeData.PiegeSprite;

        if (PiegeData.CanFall)
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
            {
                _rb = gameObject.AddComponent<Rigidbody2D>();
            }
            _rb.mass = PiegeData.Mass;
            _rb.gravityScale = 0f;
        }

        if (_cleTrapp == null)
        {
            _cleTrapp = ScriptableObject.CreateInstance<Cle>();
        }
        gameObject.AddComponent<BoxCollider2D>();
        _assignedGamepad = MainMenuManager.MetronomeID;
    }

    private void Update()
    {
        Vector2 leftStickValue = _assignedGamepad.leftStick.ReadValue();
        float horizontalInput = leftStickValue.x;
        Vector2 position = _rb.velocity;
        if (_assignedGamepad != null )
        {
            if (!_isFalling)
            {
                if (leftStickValue.magnitude > 0.2f)
                {
                    if (Mathf.Abs(horizontalInput) > 0.2f) // Si l'entrée est significative
                    {
                        position.x = horizontalInput * Time.deltaTime * 5000f;
                        position.x = Mathf.Clamp(position.x, -Camera.main.orthographicSize, Camera.main.orthographicSize);
                    }
                }

                if (leftStickValue.magnitude == 0)
                {
                    position.x = 0f;
                }
                _rb.velocity = position;
            }

            if (_assignedGamepad.buttonNorth.isPressed)
            {
                if (_inputSysteme.PiegeActive > 0.5f && PiegeData.CanFall)
                {
                    _isFalling = true;
            
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.gravityScale = PiegeData.Mass;
                    }

                    _metronomeControler.PiegeEnCours = false;
                }
            }

            if (_isFalling)
            {
                _destroyTime -= Time.deltaTime;
                if (_destroyTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RunnersControler runner = collision.gameObject.GetComponent<RunnersControler>();
        Barrier barrier = collision.gameObject.GetComponentInChildren<Barrier>();
        
        if (runner != null && barrier == null && PiegeData.Damage > 0)
        {
            runner.TakeDamage(PiegeData.Damage);
            Destroy(gameObject);
        }
        
        if (barrier != null)
        {
            Destroy(gameObject);
        }

        if (PiegeData.HasExploded && collision.gameObject.CompareTag("Sol") && _cleTrapp != null)
        {
            _cleTrapp.Explode(transform.position);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _cleTrapp.ExplosionRadius);
    }
}
