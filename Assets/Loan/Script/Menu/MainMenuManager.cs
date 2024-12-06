using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditPanel;

    private ChoixPersoManager _choixPersoManager;

    public static RunnerData FirstRunner;
    public static RunnerData SecondRunner;
    public static RunnerData ThirdRunner;

    public static int MetronomeID = -1;
    public static int ChasseurID;
    public static int MoineID;
    public static int MageID;

    public static List<int> DevicesID = new List<int>();

    private void Start()
    {
        InitializeDevices();
        
        UpdateChoixPersoManager();
    }

    public void OpenCharactersScene()
    {
        SceneManager.LoadScene(4);
    }

    public void OptionPanel()
    {
        CloseAllPanels();
        _optionsPanel.SetActive(true);
    }

    public void CreditPanel()
    {
        CloseAllPanels();
        _creditPanel.SetActive(true);
    }

    public void MainMenu()
    {
        CloseAllPanels();
        _menuPanel.SetActive(true);
    }

    public void RetrunMenu()
    {
        ResetStaticData();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void CloseAllPanels()
    {
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        _creditPanel.SetActive(false);
    }

    
    private void InitializeDevices()
    {
        DevicesID.Clear(); 

        foreach (Gamepad gamepad in Gamepad.all)
        {
            DevicesID.Add(gamepad.deviceId); 
        }

        Debug.Log($"DevicesID initialized: {string.Join(", ", DevicesID)}");
        Debug.Log($"Manettes connectées : {Gamepad.all.Count}");
    }
    
    public static bool IsDeviceIDValid(int deviceID)
    {
        return Gamepad.all.Any(g => g.deviceId == deviceID);
    }
    
    private void ResetStaticData()
    {
        FirstRunner = null;
        SecondRunner = null;
        ThirdRunner = null;
        MetronomeID = -1;
        DevicesID.Clear();
        Debug.Log("Données statiques réinitialisées.");
    }
    
    private void UpdateChoixPersoManager()
    {
        _choixPersoManager = FindObjectOfType<ChoixPersoManager>();
        if (_choixPersoManager == null)
        {
            Debug.LogWarning("ChoixPersoManager non encore instancié. Il sera trouvé dynamiquement plus tard.");
        }
        else
        {
            Debug.Log("ChoixPersoManager trouvé avec succès !");
        }
    }
    
    public void OnChoixPersoManagerSpawned()
    {
        UpdateChoixPersoManager();
    }
}