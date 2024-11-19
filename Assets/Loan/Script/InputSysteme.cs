using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSysteme : MonoBehaviour
{
    public static event Action<bool> OnInputDeviceChanged;

    public Vector2 Move;
    public float Jump;

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
}