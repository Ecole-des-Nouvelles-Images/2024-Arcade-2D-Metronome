using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MetronomeControler : MonoBehaviour
{
    [SerializeField] NoteSpawner _noteSpawner;
    [SerializeField] PiegeData _notePiegeData;
    [SerializeField] PiegeData _clePiegeData;
    [SerializeField] Transform _piegeSpawnPoint;
    [SerializeField] TextMeshProUGUI _scoreText;
    
    private InputSysteme _inputSysteme;
    private PlayerInput _playerInput;
    private Image _image;
    private int _score = 20;
    private bool _canPress = true;
    private Gamepad _assignedGamepad;

    public bool PiegeEnCours;

    private void Start()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _image = GetComponent<Image>();
        Debug.Log($"Gamepad assigné to Metronome : {MainMenuManager.MetronomeID}");

        _assignedGamepad = MainMenuManager.MetronomeID;
        _playerInput = GetComponent<PlayerInput>();
        UpdateScore();
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

            if (_inputSysteme.PiegeUp == 1 && _score >= 5 && _canPress && !PiegeEnCours)
            {
                _canPress = false;
                PiegeEnCours = true;
                SpawnPiege(_notePiegeData);
                _score = _score - 5;
                UpdateScore();
                StartCoroutine(ResetCanPress());
            }
        
            if (_inputSysteme.PiegeRight == 1 && _score >= 6 && _canPress && !PiegeEnCours)
            {
                _canPress = false;
                PiegeEnCours = true;
                SpawnPiege(_clePiegeData);
                _score = _score - 6;
                UpdateScore();
                StartCoroutine(ResetCanPress());
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
            Invoke(nameof(Reset),0.15f);
        }
        else
        {
            SubtractionScore(1);
            _image.color = Color.red;
            Invoke(nameof(Reset),0.15f);
        }

        StartCoroutine(ResetCanPress());
    }

    private System.Collections.IEnumerator ResetCanPress()
    {
        yield return new WaitForSeconds(0.15f);
        _canPress = true;
    }

    private void SubtractionScore(int points)
    {
        _score = Mathf.Max(0,_score - points);
        UpdateScore();
    }

    private void AddScore(int points)
    {
        _score += points;
        UpdateScore();
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

        piegeScript.Initialize(_inputSysteme, this);
    }

    private void UpdateScore()
    {
        _scoreText.text = "X " + _score.ToString();
    }
}
