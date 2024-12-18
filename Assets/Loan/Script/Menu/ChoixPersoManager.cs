using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoixPersoManager : MonoBehaviour
{
    [SerializeField] private Button _chasseurButton;
    [SerializeField] private Button _moineButton;
    [SerializeField] private Button _mageButton;
    [SerializeField] private Button _metronomeButton;

    [SerializeField] private RunnerData _runnerChasseur;
    [SerializeField] private RunnerData _runnerMoine;
    [SerializeField] private RunnerData _runnerMage;

    private Gamepad _assignedGamepad;
    public static int PlayerCount = 0;
    public static Dictionary<int, Gamepad> PlayerGamepads = new Dictionary<int, Gamepad>();

    
    void Start()
    {
        MainMenuManager mainMenuManager = FindObjectOfType<MainMenuManager>();
        if (mainMenuManager != null)
        {
            mainMenuManager.OnChoixPersoManagerSpawned();
        }
    }
    private void OnEnable()
    {
        
        if (Gamepad.all.Count > PlayerCount)
        {
            _assignedGamepad = Gamepad.all[PlayerCount];

            if (!MainMenuManager.AssignedGamepads.Contains(_assignedGamepad))
            {
                MainMenuManager.AssignedGamepads.Add(_assignedGamepad);
            }

            Debug.Log($"Manette assignée au joueur {PlayerCount + 1} : {_assignedGamepad.displayName}");

            _chasseurButton.onClick.AddListener(() => SelectRunner(_runnerChasseur, _assignedGamepad, 1));
            _moineButton.onClick.AddListener(() => SelectRunner(_runnerMoine, _assignedGamepad, 2));
            _mageButton.onClick.AddListener(() => SelectRunner(_runnerMage, _assignedGamepad, 3));
            _metronomeButton.onClick.AddListener(() => SelectMetronome(_assignedGamepad));

            PlayerCount++;
        }
        else
        {
            Debug.LogWarning("Pas assez de manettes connectées pour attribuer à ce joueur !");
        }
    }

    private void OnDisable()
    {
        _chasseurButton.onClick.RemoveAllListeners();
        _moineButton.onClick.RemoveAllListeners();
        _mageButton.onClick.RemoveAllListeners();
        _metronomeButton.onClick.RemoveAllListeners();
    }

    private void SelectRunner(RunnerData runner, Gamepad gamepad, int choix)
    {
        Debug.Log("Runner "+choix+" GamePad =>"+ gamepad.deviceId);
        
        PlayerGamepads[choix] = gamepad;
        switch (choix)
        {
            case 1:
                GameManager.ScriptableObjectPlayerOne = runner;
                break;
            case 2:
                GameManager.ScriptableObjectPlayerTwo = runner;
                break;
            case 3:
                GameManager.ScriptableObjectPlayerThre = runner;
                break;
        }

        DisableAllButtons();
        Debug.Log($"Runner {runner.Name} sélectionné avec la manette {gamepad.displayName} pour le slot {choix}.");
        GameManager.Instance.DisplayPlayerAssignments();
    }

    private void SelectMetronome(Gamepad gamepad)
    {
        Debug.Log("Metronome GramePad =>"+ gamepad.deviceId);
        MainMenuManager.MetronomeID = gamepad;
        Debug.Log($"Métronome instancié avec la manette {gamepad.displayName}.");
        StartCoroutine(ValidateStartGame());
        DisableAllButtons();
    }

    private void DisableAllButtons()
    {
        _chasseurButton.interactable = false;
        _moineButton.interactable = false;
        _mageButton.interactable = false;
        _metronomeButton.interactable = false;
    }
    
    private IEnumerator ValidateStartGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scene_Game");
        
    }
}