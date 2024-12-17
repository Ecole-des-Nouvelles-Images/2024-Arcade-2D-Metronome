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

    public static Gamepad MetronomeID;
    public static Gamepad ChasseurID;
    public static Gamepad MoineID;
    public static Gamepad MageID;

    
    public static List<Gamepad> AssignedGamepads = new List<Gamepad>();

    private void Start()
    {
        ResetStaticData();
        InitializeDevices();
        UpdateChoixPersoManager();
        ChoixPersoManager.PlayerCount = 0;
    }

    public void OpenCharactersScene()
    {
        SceneManager.LoadScene("Scene_Choix");
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
        SceneManager.LoadScene("MenuScene");
        Destroy(GameManager.Instance.gameObject);
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
        AssignedGamepads.Clear(); 

        foreach (Gamepad gamepad in Gamepad.all)
        {
            AssignedGamepads.Add(gamepad); 
        }

        Debug.Log($"DevicesID initialized: {string.Join(", ", AssignedGamepads)}");
        Debug.Log($"Manettes connectées : {Gamepad.all.Count}");
    }
    
    public static bool IsDeviceIDValid(int deviceID)
    {
        return Gamepad.all.Any(g => g.deviceId == deviceID);
    }
    
    public void ResetStaticData()
    {
        FirstRunner = null;
        SecondRunner = null;
        ThirdRunner = null;
        MetronomeID = null;
        AssignedGamepads.Clear();
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