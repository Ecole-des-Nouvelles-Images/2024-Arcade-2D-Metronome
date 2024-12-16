using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MetronomeControler : MonoBehaviour
{
    [SerializeField] NoteSpawner _noteSpawner;
    [SerializeField] PiegeData _notePiegeData;
    [SerializeField] PiegeData _clePiegeData;
    [SerializeField] Transform _piegeSpawnPoint;
    
    private InputSysteme _inputSysteme;
    private PlayerInput _playerInput;
    private Image _image;
    private int _score = 5;
    private bool _canPress = true;
    private Gamepad _assignedGamepad;

    private void Start()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _image = GetComponent<Image>();
        _assignedGamepad = MainMenuManager.MetronomeID;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_assignedGamepad != null && _assignedGamepad == Gamepad.current)
        {
            if (_inputSysteme.Active == 1 && _canPress)
            {
                HandleButtonPress();
            }
        
            //test
            if (_inputSysteme.PiegeLeft == 1)
            {
                Debug.Log("Piege gauche activé !");
            }

            if (_inputSysteme.PiegeUp == 1 && _score >= 5)
            {
                // Debug.Log("Piege Haut activé !");
                SpawnPiege(_notePiegeData);
                _score = _score - 5;
            }
        
            if (_inputSysteme.PiegeRight == 1 && _score >= 6)
            {
                SpawnPiege(_clePiegeData);
                _score = _score - 6;
            }
        
            if (_inputSysteme.PiegeDown == 1)
            {
                Debug.Log("Piege Bas activé !");
            }
        }
        
    }

    private void HandleButtonPress()
    {
        _canPress = false;
        if (_noteSpawner.CheckNoteUnderImage())
        {
            AddScore(1);
            _image.color = Color.green; 
            Invoke(nameof(Reset),0.5f);
        }
        else
        {
            SubtractionScore(1);
            _image.color = Color.red;
            Invoke(nameof(Reset),0.5f);
        }

        StartCoroutine(ResetCanPress());
    }

    private System.Collections.IEnumerator ResetCanPress()
    {
        yield return new WaitForSeconds(0.18f);
        _canPress = true;
    }

    private void SubtractionScore(int points)
    {
        _score = Mathf.Max(0,_score - points);
        Debug.Log("Score : " + _score);
    }

    private void AddScore(int points)
    {
        _score += points;
        Debug.Log("Score : " + _score);
    }

    private void Reset()
    {
        _image.color = Color.white;
    }

    public void SpawnPiege(PiegeData piegeData)
    {
        if (piegeData == null)
        {
            Debug.LogError("Piege is null!");
            return;
        }

        GameObject trap = new GameObject("Trap");
        trap.transform.position = _piegeSpawnPoint.position;
        
        Piege piegeScript = trap.AddComponent<Piege>();
        piegeScript.PiegeData = piegeData;

        piegeScript.Initialize(_inputSysteme);
    }
}
