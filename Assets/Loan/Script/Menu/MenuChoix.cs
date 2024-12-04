using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuChoix : MonoBehaviour
{
    [SerializeField] GameObject _choixPersoPanel;
    [SerializeField] GameObject _choixMapPanel;

    private void Start()
    {
        CloseAllPanels();
        _choixPersoPanel.SetActive(true);
    }

    public void MapPanel()
    {
        CloseAllPanels();
        _choixMapPanel.SetActive(true);
    }

    public void PersoPanel()
    {
        CloseAllPanels();
        _choixPersoPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    private void CloseAllPanels()
    {
        _choixMapPanel.SetActive(false);
        _choixPersoPanel.SetActive(false);
    }
}
