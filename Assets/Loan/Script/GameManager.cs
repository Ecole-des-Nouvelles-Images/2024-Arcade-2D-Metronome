using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   private List<GameObject> _aliveRunners = new List<GameObject>();

   [SerializeField] private Transform _runnerSpawn1;
   [SerializeField] private Transform _runnerSpawn2;
   [SerializeField] private Transform _runnerSpawn3;
   [SerializeField] private GameObject _runnerPrefab;
   [SerializeField] private GameObject _metronomeControll;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject); 
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      SpawnRunners();
      SetupMetronome();
   }

   private void SpawnRunners()
   {
      if (MainMenuManager.FirstRunner != null)
      {
         GameObject runner1 = Instantiate(_runnerPrefab, _runnerSpawn1.position, Quaternion.identity);
         SetupRunner(runner1, MainMenuManager.FirstRunner, MainMenuManager.DevicesID[0]);
      }
      if (MainMenuManager.SecondRunner != null)
      {
         GameObject runner2 = Instantiate(_runnerPrefab, _runnerSpawn2.position, Quaternion.identity);
         SetupRunner(runner2, MainMenuManager.SecondRunner, MainMenuManager.DevicesID[1]);
      }
      if (MainMenuManager.ThirdRunner != null)
      {
         GameObject runner3 = Instantiate(_runnerPrefab, _runnerSpawn3.position, Quaternion.identity);
         SetupRunner(runner3, MainMenuManager.ThirdRunner, MainMenuManager.DevicesID[2]);
      }
   }

   void SetupRunner(GameObject runner, RunnerData runnerData, int deviceID)
   {
      RunnersControler runnersControler = runner.GetComponent<RunnersControler>();
      if (runnersControler != null)
      {
         runnersControler.Setup(runnerData, deviceID);
      }
      else
      {
         Debug.LogWarning("No runner controler found");
      }
   }

   private void SetupMetronome()
   {
      if (_metronomeControll != null && MainMenuManager.MetronomeID != -1)
      {
         PlayerInput metronomeInput = _metronomeControll.GetComponent<PlayerInput>();
         if (metronomeInput != null)
         {
            metronomeInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[MainMenuManager.MetronomeID]);
            Debug.Log("le metronome est controller par : " + MainMenuManager.MetronomeID);
         }
         else
         {
            Debug.LogError("Le GameObject Métronome n'a pas de PlayerInput !");
         }
      }
   }

   public void LoadWinRunner(string RunnerWin)
   {
      SceneManager.LoadScene(2);
   }

   public void RegisterRunner(GameObject Runner)
   {
      if (!_aliveRunners.Contains(Runner))
      {
         _aliveRunners.Add(Runner);
      }
   }

   public void UnregisterRunner(GameObject Runner)
   {
      _aliveRunners.Remove(Runner);
      CheckRunnersAlive();
   }

   private void CheckRunnersAlive()
   {
      if (_aliveRunners.Count == 0)
      {
         SceneManager.LoadScene(3);
      }
   }
   
}
