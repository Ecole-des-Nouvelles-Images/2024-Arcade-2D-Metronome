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

    private int _deviceID;
    private bool _ready ;

    private void OnEnable()
    {
        if (Gamepad.current != null)
        {
            _deviceID = Gamepad.current.deviceId;

            if (!MainMenuManager.DevicesID.Contains(_deviceID))
            {
                MainMenuManager.DevicesID.Add(_deviceID);
            }
        }
        else
        {
            Debug.LogWarning("Aucune manette détectée !");
        }
        
        _chasseurButton.onClick.AddListener(() => SelectRunner(_runnerChasseur, _deviceID,1));
        _moineButton.onClick.AddListener(() => SelectRunner(_runnerMoine, _deviceID,2));
        _mageButton.onClick.AddListener(() => SelectRunner(_runnerMage, _deviceID,3));
        _metronomeButton.onClick.AddListener(() => SelectMetronome(_deviceID));
        _readyButton.onClick.AddListener(() => SceneManager.LoadScene(1));
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
           case 1 :
               MainMenuManager.FirstRunner = runner;
               break;
           case 2 :
               MainMenuManager.SecondRunner = runner;
               break;
           case 3 :
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
        // OnReady();
    }

    // private void OnReady()
    // {
    //     SetReady(true);
    // }

    // private void SetReady(bool readyState)
    // {
    //     _ready = readyState;
    //     MainMenuManager.SetPlayerReady(_deviceID, _ready);
    // }

    public void ResetSelection()
    {
        MainMenuManager.FirstRunner = null;
        MainMenuManager.SecondRunner = null;
        MainMenuManager.ThirdRunner = null;
        MainMenuManager.MetronomeID = -1;

        foreach (int id in MainMenuManager.DevicesID)
        {
            Debug.Log($"Reset Device ID : {id}");
        }

        MainMenuManager.DevicesID.Clear();
    }
}
