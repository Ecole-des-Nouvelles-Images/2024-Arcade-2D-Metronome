using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel;
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] GameObject _creditPanel;
    
    public static RunnerData FirstRunner;
    public static RunnerData SecondRunner;
    public static RunnerData ThirdRunner;
    
    public static int MetronomeID;
    
    public static List<int> DevicesID = new List<int>();
    private static Dictionary<int, bool> playerReadyStates = new Dictionary<int, bool>();

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

    private static void StartGame()
    {
        SceneManager.LoadScene(1);
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

    public void RetrunMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    // public static void SetPlayerReady(int playerID, bool isReady)
    // {
    //     if (DevicesID.Contains(playerID))
    //     {
    //         playerReadyStates[playerID] = isReady;
    //         CheckAllPlayersReady();
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"Player ID {playerID} non trouvé dans la liste des DevicesID.");
    //     }
    // }
    
    // private static void CheckAllPlayersReady()
    // {
    //     foreach (int playerID in DevicesID)
    //     {
    //         if (!playerReadyStates.ContainsKey(playerID) || !playerReadyStates[playerID])
    //         {
    //             return;
    //         }
    //     }
    //     StartGame();
    // }
}
