using System.Collections.Generic;
using System.Linq;
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
      ValidateDeviceIDs();
      SpawnRunners();
      SetupMetronome();
   }

   private void SpawnRunners()
   {
      Debug.Log("DevicesID List: " + string.Join(", ", MainMenuManager.DevicesID));

      if (MainMenuManager.FirstRunner != null && MainMenuManager.DevicesID.Count > 0)
      {
         GameObject runner1 = Instantiate(_runnerPrefab, _runnerSpawn1.position, Quaternion.identity);
         SetupRunner(runner1, MainMenuManager.FirstRunner, MainMenuManager.DevicesID[0]);
      }
      if (MainMenuManager.SecondRunner != null && MainMenuManager.DevicesID.Count > 1)
      {
         GameObject runner2 = Instantiate(_runnerPrefab, _runnerSpawn2.position, Quaternion.identity);
         SetupRunner(runner2, MainMenuManager.SecondRunner, MainMenuManager.DevicesID[1]);
      }
      if (MainMenuManager.ThirdRunner != null && MainMenuManager.DevicesID.Count > 2)
      {
         GameObject runner3 = Instantiate(_runnerPrefab, _runnerSpawn3.position, Quaternion.identity);
         SetupRunner(runner3, MainMenuManager.ThirdRunner, MainMenuManager.DevicesID[2]);
      }
   }

   void SetupRunner(GameObject runner, RunnerData runnerData, int deviceID)
   {
      if (deviceID >= 0 && deviceID < Gamepad.all.Count)
      {
         RunnersControler runnersControler = runner.GetComponent<RunnersControler>();
         if (runnersControler != null)
         {
            runnersControler.Setup(runnerData, deviceID);
         }
         else
         {
            Debug.LogWarning("No runner controller found on the GameObject.");
         }
      }
      else
      {
         Debug.LogError($"Invalid deviceID: {deviceID}. Cannot assign a Gamepad. Runner will use default controls.");
      }
   }

   private void SetupMetronome()
   {
      if (MainMenuManager.MetronomeID == -1)
      {
         Debug.LogError("Le Métronome n'a pas été configuré avec un Device ID valide.");
         return;
      }
      
      if (_metronomeControll != null && MainMenuManager.MetronomeID >= 0 && MainMenuManager.DevicesID.Contains(MainMenuManager.MetronomeID))
      {
         PlayerInput metronomeInput = _metronomeControll.GetComponent<PlayerInput>();
         if (metronomeInput != null)
         {
            Gamepad metronomeDevice = Gamepad.all.FirstOrDefault(g => g.deviceId == MainMenuManager.MetronomeID);
            if (metronomeDevice != null)
            {
               metronomeInput.SwitchCurrentControlScheme("Gamepad", metronomeDevice);
               Debug.Log($"Le métronome est contrôlé par : {metronomeDevice.displayName} (ID : {MainMenuManager.MetronomeID})");
            }
            else
            {
               Debug.LogError($"Métronome : Aucune manette trouvée avec l'ID {MainMenuManager.MetronomeID}.");
            }
         }
         else
         {
            Debug.LogError("Le GameObject Métronome n'a pas de PlayerInput !");
         }
      }
      else
      {
         Debug.LogError("Métronome non configuré ou ID de manette invalide.");
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
   
   private void ValidateDeviceIDs()
   {
      for (int i = 0; i < MainMenuManager.DevicesID.Count; i++)
      {
         int deviceID = MainMenuManager.DevicesID[i];
         if (!Gamepad.all.Any(g => g.deviceId == deviceID))
         {
            Debug.LogWarning($"Device ID {deviceID} invalide. Retiré de la liste.");
            MainMenuManager.DevicesID.RemoveAt(i);
            i--; 
         }
      }
      Debug.Log($"Valid Device IDs: {string.Join(", ", MainMenuManager.DevicesID)}");
   }
}
