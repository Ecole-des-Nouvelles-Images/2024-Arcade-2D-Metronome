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


    public void Initialize(InputSysteme inputSysteme)
    {
        _inputSysteme = inputSysteme;
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
        gameObject.AddComponent<BoxCollider2D>();
        _assignedGamepad = MainMenuManager.MetronomeID;
    }

    private void Update()
    {
        if (_assignedGamepad != null && _assignedGamepad == Gamepad.current)
        {
            if (!_isFalling)
            {
                float horizontalInput = _inputSysteme.PiegeMove.x;
                Vector2 position = _rb.velocity;
                position.x = horizontalInput * Time.deltaTime * 5000f;
                position.x = Mathf.Clamp(position.x,-Camera.main.orthographicSize, Camera.main.orthographicSize);
                _rb.velocity = position;
            }

            if (_inputSysteme.PiegeActive > 0.5f && PiegeData.CanFall)
            {
                _isFalling = true;
            
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = PiegeData.Mass;
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

        if (PiegeData.HasExploded && collision.gameObject.CompareTag("Sol"))
        {
            _cleTrapp.Explode(transform.position);
        }
    }
}
