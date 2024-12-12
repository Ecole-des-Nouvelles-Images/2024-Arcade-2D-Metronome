using System.Collections;
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
    private static int _playerCount = 0;

    
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
        
        if (Gamepad.all.Count > _playerCount)
        {
            _assignedGamepad = Gamepad.all[_playerCount];

            if (!MainMenuManager.AssignedGamepads.Contains(_assignedGamepad))
            {
                MainMenuManager.AssignedGamepads.Add(_assignedGamepad);
            }

            Debug.Log($"Manette assignée au joueur {_playerCount + 1} : {_assignedGamepad.displayName}");

            _chasseurButton.onClick.AddListener(() => SelectRunner(_runnerChasseur, _assignedGamepad, 1));
            _moineButton.onClick.AddListener(() => SelectRunner(_runnerMoine, _assignedGamepad, 2));
            _mageButton.onClick.AddListener(() => SelectRunner(_runnerMage, _assignedGamepad, 3));
            _metronomeButton.onClick.AddListener(() => SelectMetronome(_assignedGamepad));

            _playerCount++;
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
        switch (choix)
        {
            case 1:
                MainMenuManager.FirstRunner = runner;
                MainMenuManager.ChasseurID = gamepad;
                break;
            case 2:
                MainMenuManager.SecondRunner = runner;
                MainMenuManager.MoineID = gamepad;
                break;
            case 3:
                MainMenuManager.ThirdRunner = runner;
                MainMenuManager.MageID = gamepad;
                break;
        }

        DisableAllButtons();
        Debug.Log($"Runner {runner.Name} sélectionné avec la manette {gamepad.displayName} pour le slot {choix}.");
    }

    private void SelectMetronome(Gamepad gamepad)
    {
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
        SceneManager.LoadScene(1);
        
    }
}