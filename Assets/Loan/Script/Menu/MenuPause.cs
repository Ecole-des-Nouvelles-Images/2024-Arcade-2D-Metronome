using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Button _returnPlay;
    [SerializeField] Button _returnMenu;
    private MusicManager _musicManager;
    private void OnEnable()
    {
        _returnPlay.onClick.AddListener(() => RetrunPlay());
        _returnMenu.onClick.AddListener(() => ReturnMenu());
        _musicManager = MusicManager.Instance;
        _musicManager.StopMusic();
    }

    private void RetrunPlay()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _musicManager.ReprendMusic();
    }

    private void ReturnMenu()
    {
        Time.timeScale = 1;
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("MenuScene");
    }
}
