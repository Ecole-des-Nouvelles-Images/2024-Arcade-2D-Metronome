using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   private List<GameObject> _aliveRunners = new List<GameObject>();
   private int _metronomePlayerIndex;
   private Dictionary<int, RunnerData> chosenRunners;

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

   public void SetChoices(int metronomeIndex, Dictionary<int, RunnerData> runners)
   {
      _metronomePlayerIndex = metronomeIndex;
      chosenRunners = runners;
   }
}
