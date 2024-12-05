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
    [SerializeField] private Button _readyButton;

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
            int deviceID = _assignedGamepad.deviceId;

            if (!MainMenuManager.DevicesID.Contains(deviceID))
            {
                MainMenuManager.DevicesID.Add(deviceID);
            }

            Debug.Log($"Manette assignée au joueur {_playerCount + 1} : {_assignedGamepad.displayName}, ID : {deviceID}");

            _chasseurButton.onClick.AddListener(() => SelectRunner(_runnerChasseur, deviceID, 1));
            _moineButton.onClick.AddListener(() => SelectRunner(_runnerMoine, deviceID, 2));
            _mageButton.onClick.AddListener(() => SelectRunner(_runnerMage, deviceID, 3));
            _metronomeButton.onClick.AddListener(() => SelectMetronome(deviceID));
            _readyButton.onClick.AddListener(() => SceneManager.LoadScene(1));

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

    private void SelectRunner(RunnerData runner, int deviceID, int choix)
    {
        switch (choix)
        {
            case 1:
                MainMenuManager.FirstRunner = runner;
                break;
            case 2:
                MainMenuManager.SecondRunner = runner;
                break;
            case 3:
                MainMenuManager.ThirdRunner = runner;
                break;
        }

        DisableAllButtons();
        Debug.Log($"Runner {runner.Name} sélectionné avec Device ID {deviceID} pour le slot {choix}.");
    }

    private void SelectMetronome(int deviceID)
    {
        MainMenuManager.MetronomeID = deviceID;
        Debug.Log($"Métronome instancié avec Device ID {deviceID}.");
        DisableAllButtons();
    }

    private void DisableAllButtons()
    {
        _chasseurButton.interactable = false;
        _moineButton.interactable = false;
        _mageButton.interactable = false;
        _metronomeButton.interactable = false;
    }
    
}