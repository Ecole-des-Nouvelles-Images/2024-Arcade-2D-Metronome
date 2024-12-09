using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSysteme : MonoBehaviour
{
    public static event Action<bool> OnInputDeviceChanged;

    //Runners
    public Vector2 Move;
    public float Jump;
    public float PowerUp;
    
    //Métronome
    public float Active;
    public Vector2 PiegeMove;
    public float PiegeUp;
    public float PiegeRight;
    public float PiegeDown;
    public float PiegeLeft;
    public float PiegeActive;
    

    private PlayerInput _playerInput;
    private MetronomeControler _metronomeControler;
    private RunnersControler _runnersControler;
    private bool _isControllerConnected;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (_runnersControler)
        {
            _runnersControler = GetComponent<RunnersControler>();
        }
        else
        {
            _metronomeControler = GetComponent<MetronomeControler>();
        }
        if (_playerInput == null) throw new NullReferenceException("PlayerInputManager is null");
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        // Bind input actions
        //Runners
        _playerInput.actions["Mouvement"].performed += OnMove;
        _playerInput.actions["Mouvement"].canceled += OnMove;

        _playerInput.actions["Jump"].performed += OnJump;
        _playerInput.actions["Jump"].canceled += OnJump;
        
        _playerInput.actions["PowerUp"].performed += OnPowerUp;
        _playerInput.actions["PowerUP"].canceled += OnPowerUp;
        
        //Métronome
        _playerInput.actions["Active"].performed += OnActive;
        _playerInput.actions["Active"].canceled += OnActive;
        
        _playerInput.actions["PiegeMove"].performed += OnPiegeMove;
        _playerInput.actions["PiegeMove"].canceled += OnPiegeMove;
        
        _playerInput.actions["PiegeUp"].performed += OnPiegeUp;
        _playerInput.actions["PiegeUp"].canceled += OnPiegeUp;
        
        _playerInput.actions["PiegeRight"].performed += OnPiegeRight;
        _playerInput.actions["PiegeRight"].canceled += OnPiegeRight;
        
        _playerInput.actions["PiegeDown"].performed += OnPiegeDown;
        _playerInput.actions["PiegeDown"].canceled += OnPiegeDown;
        
        _playerInput.actions["PiegeLeft"].performed += OnPiegeLeft;
        _playerInput.actions["PiegeLeft"].canceled += OnPiegeLeft;
        
        _playerInput.actions["PiegeActive"].performed += OnPiegeActive;
        _playerInput.actions["PiegeActive"].canceled += OnPiegeActive;
        
        

        DetectCurrentInputDevice();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        // Unbind input actions
        //Runners
        _playerInput.actions["Mouvement"].performed -= OnMove;
        _playerInput.actions["Mouvement"].canceled -= OnMove;
        
        _playerInput.actions["Jump"].performed -= OnJump;
        _playerInput.actions["Jump"].canceled -= OnJump;
        
        _playerInput.actions["PowerUp"].performed -= OnPowerUp;
        _playerInput.actions["PowerUP"].canceled -= OnPowerUp;
        
        //Métronome
        _playerInput.actions["Active"].performed -= OnActive;
        _playerInput.actions["Active"].canceled -= OnActive;
        
        _playerInput.actions["PiegeMove"].performed -= OnPiegeMove;
        _playerInput.actions["PiegeMove"].canceled -= OnPiegeMove;
        
        _playerInput.actions["PiegeUp"].performed -= OnPiegeUp;
        _playerInput.actions["PiegeUp"].canceled -= OnPiegeUp;
        
        _playerInput.actions["PiegeRight"].performed -= OnPiegeRight;
        _playerInput.actions["PiegeRight"].canceled -= OnPiegeRight;
        
        _playerInput.actions["PiegeDown"].performed -= OnPiegeDown;
        _playerInput.actions["PiegeDown"].canceled -= OnPiegeDown;
        
        _playerInput.actions["PiegeLeft"].performed -= OnPiegeLeft;
        _playerInput.actions["PiegeLeft"].canceled -= OnPiegeLeft;
        
        _playerInput.actions["PiegeActive"].performed -= OnPiegeActive;
        _playerInput.actions["PiegeActive"].canceled -= OnPiegeActive;
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

    //Runners
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!_metronomeControler)
        {
            Move = context.ReadValue<Vector2>();   
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
            Jump = context.ReadValue<float>();   
    }
    
    private void OnPowerUp(InputAction.CallbackContext context)
    {
            PowerUp = context.ReadValue<float>();
    }

    //Métronome
    private void OnActive(InputAction.CallbackContext context)
    {
        if (_metronomeControler)
        {
            Active = context.ReadValue<float>();   
        }
    }
    
    private void OnPiegeMove(InputAction.CallbackContext context)
    {
        if (!_runnersControler)
        {
            PiegeMove = context.ReadValue<Vector2>();
        }
    }
    
    private void OnPiegeUp(InputAction.CallbackContext context)
    {
        if (_metronomeControler)
        {
            PiegeUp = context.ReadValue<float>();   
        }
    }
    
    private void OnPiegeRight(InputAction.CallbackContext context)
    {
        if (_metronomeControler)
        {
            PiegeRight = context.ReadValue<float>();   
        }
    }
    
    private void OnPiegeDown(InputAction.CallbackContext context)
    {
        if (_metronomeControler)
        {
            PiegeDown = context.ReadValue<float>();   
        }
    }
    
    private void OnPiegeLeft(InputAction.CallbackContext context)
    {
        if (_metronomeControler)
        {
            PiegeLeft = context.ReadValue<float>();   
        }
    }
    
    private void OnPiegeActive(InputAction.CallbackContext context)
    {
        if (!_runnersControler)
        {
            PiegeActive = context.ReadValue<float>();   
        }
    }
    
    public void SwitchCurrentControlScheme(Gamepad gamepad)
    {
        if (_playerInput != null)
        {
            if (gamepad != null)
            {
                _playerInput.SwitchCurrentControlScheme(gamepad);
                Debug.Log("Contrôleur associé : " + gamepad.displayName);
            }
            else
            {
                Debug.LogError("Le Gamepad fourni est nul. Impossible de changer de schéma de contrôle.");
            }
        }
        else
        {
            Debug.LogError("PlayerInput est nul. Impossible de changer de schéma de contrôle.");
        }
    }
}