using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSysteme : MonoBehaviour
{
    public static event Action<bool> OnInputDeviceChanged;

    public Vector2 Move;
    public float Jump;
    public float PowerUp;
    public float Active;
    

    private PlayerInput _playerInput;
    private bool _isControllerConnected;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (_playerInput == null) throw new NullReferenceException("PlayerInputManager is null");
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        // Bind input actions
        _playerInput.actions["Mouvement"].performed += OnMove;
        _playerInput.actions["Mouvement"].canceled += OnMove;

        _playerInput.actions["Jump"].performed += OnJump;
        _playerInput.actions["Jump"].canceled += OnJump;
        
        _playerInput.actions["PowerUp"].performed += OnPowerUp;
        _playerInput.actions["PowerUP"].canceled += OnPowerUp;
        
        _playerInput.actions["Active"].performed += OnActive;
        _playerInput.actions["Active"].canceled += OnActive;
        
        

        DetectCurrentInputDevice();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        // Unbind input actions
        _playerInput.actions["Mouvement"].performed -= OnMove;
        _playerInput.actions["Mouvement"].canceled -= OnMove;
        
        _playerInput.actions["Jump"].performed -= OnJump;
        _playerInput.actions["Jump"].canceled -= OnJump;
        
        _playerInput.actions["PowerUp"].performed -= OnPowerUp;
        _playerInput.actions["PowerUP"].canceled -= OnPowerUp;
        
        _playerInput.actions["Active"].performed -= OnActive;
        _playerInput.actions["Active"].canceled -= OnActive;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed)
        {
            DetectCurrentInputDevice();
        }
    }

    private void DetectCurrentInputDevice()
    {
        _isControllerConnected = Gamepad.all.Count > 0;
        OnInputDeviceChanged?.Invoke(_isControllerConnected);

        Debug.Log(_isControllerConnected
            ? "Controller connected: Switching to Gamepad controls."
            : "No controller connected: Switching to Keyboard/Mouse controls.");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Jump = context.ReadValue<float>();
    }
    
    private void OnPowerUp(InputAction.CallbackContext context)
    {
        PowerUp = context.ReadValue<float>();
    }

    private void OnActive(InputAction.CallbackContext context)
    {
        
        Active = context.ReadValue<float>();
    }
}