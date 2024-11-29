using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel;
    [SerializeField] GameObject _optionsPanel;
    [SerializeField] GameObject _creditPanel;
    [SerializeField] GameObject _charactersPanel;

    private void Start()
    {
        CloseAllPanels();
        _menuPanel.SetActive(true);
    }

    public void OpenCharactersPanel()
    {
        CloseAllPanels();
        _charactersPanel.SetActive(true);
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
    
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void CloseAllPanels()
    {
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        _charactersPanel.SetActive(false);
        _creditPanel.SetActive(false);
    }
}
