using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

   private List<GameObject> _aliveRunners = new List<GameObject>();
   private MainMenuManager _mainManager;

   [SerializeField] private Transform _runnerSpawn1;
   [SerializeField] private Transform _runnerSpawn2;
   [SerializeField] private Transform _runnerSpawn3;
   [SerializeField] private GameObject _runnerPrefab;
   [SerializeField] private GameObject _metronomeControll;
   [SerializeField] private HealthHUD _healthHudsprefabs;
   [SerializeField] private POwerUPHUD _powerUpHUDPrefabs;
   

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
      StartCoroutine(ValidateDevicesCoroutine());
      ValidateDeviceIDs();
   }
   
   void Update()
   {
      if (Gamepad.all.Count == 0)
      {
         Debug.LogWarning("Aucune manette connectée.");
      }
   }
   private IEnumerator ValidateDevicesCoroutine()
   {
      yield return new WaitForSeconds(0.5f);  
      SpawnRunners();
      SetupMetronome();
   }

   private void SpawnRunners()
   {
      if (MainMenuManager.FirstRunner != null && MainMenuManager.ChasseurID != null
          )
      {
         GameObject runner1 = Instantiate(_runnerPrefab, _runnerSpawn1.position, Quaternion.identity);
         SetupRunner(runner1, MainMenuManager.FirstRunner, MainMenuManager.ChasseurID);
         RunnersControler runnersControler = runner1.GetComponent<RunnersControler>();
         HealthHUD newHealthHUD = Instantiate(_healthHudsprefabs);
         newHealthHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newHealthHUD.transform.localPosition = new Vector3(-576, -492, 0);
         runnersControler.healthHUD = newHealthHUD;
         POwerUPHUD newPowerUPHUD = Instantiate(_powerUpHUDPrefabs);
         newPowerUPHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newPowerUPHUD.transform.localPosition = new Vector3(-390, -492, 0);
         runnersControler.PowerUPHUD = newPowerUPHUD;
      }
      if (MainMenuManager.SecondRunner != null && MainMenuManager.MoineID != null
          )
      {
         GameObject runner2 = Instantiate(_runnerPrefab, _runnerSpawn2.position, Quaternion.identity);
         SetupRunner(runner2, MainMenuManager.SecondRunner, MainMenuManager.MoineID);
         RunnersControler runnersControler = runner2.GetComponent<RunnersControler>();
         HealthHUD newHealthHUD = Instantiate(_healthHudsprefabs);
         newHealthHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newHealthHUD.transform.localPosition = new Vector3(1, -492, 0);
         runnersControler.healthHUD = newHealthHUD;
         POwerUPHUD newPowerUPHUD = Instantiate(_powerUpHUDPrefabs);
         newPowerUPHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newPowerUPHUD.transform.localPosition = new Vector3(185, -492, 0);
         runnersControler.PowerUPHUD = newPowerUPHUD;
      }
      if (MainMenuManager.ThirdRunner != null && MainMenuManager.MageID != null
          )
      {
         GameObject runner3 = Instantiate(_runnerPrefab, _runnerSpawn3.position, Quaternion.identity);
         SetupRunner(runner3, MainMenuManager.ThirdRunner, MainMenuManager.MageID);
         RunnersControler runnersControler = runner3.GetComponent<RunnersControler>();
         HealthHUD newHealthHUD = Instantiate(_healthHudsprefabs);
         newHealthHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newHealthHUD.transform.localPosition = new Vector3(575, -492, 0);
         runnersControler.healthHUD = newHealthHUD;
         POwerUPHUD newPowerUPHUD = Instantiate(_powerUpHUDPrefabs);
         newPowerUPHUD.transform.SetParent(GameObject.Find("Canvas").transform, false);
         newPowerUPHUD.transform.localPosition = new Vector3(755, -492, 0);
         runnersControler.PowerUPHUD = newPowerUPHUD;
      }
   }

   void SetupRunner(GameObject runner, RunnerData runnerData, Gamepad gamepad)
   {
      RunnersControler runnersControler = runner.GetComponent<RunnersControler>();

      if (runnersControler == null)
      {
         Debug.LogError($"Contrôleur pour {runnerData.Name} non trouvé.");
         return;
      }

      if (gamepad != null)
      {
         runnersControler.Setup(runnerData, gamepad);
         Debug.Log($"Runner {runnerData.Name} configuré avec la manette {gamepad.displayName} Id {Gamepad.current.deviceId}");
      }
      else
      {
         Debug.LogWarning($"Aucune manette trouvée pour {runnerData.Name}. Assignation au premier Gamepad disponible.");
         runnersControler.Setup(runnerData, Gamepad.all.Count > 0 ? Gamepad.all[0] : null);
      }
   }

   private void SetupMetronome()
   {
      if (MainMenuManager.MetronomeID == null)
      {
         Debug.LogError("Le Métronome n'a pas été configuré avec un Device ID valide.");
         return;
      }
      
      if (_metronomeControll != null)
      {
         PlayerInput metronomeInput = _metronomeControll.GetComponent<PlayerInput>();
         if (metronomeInput != null)
         {
            metronomeInput.SwitchCurrentControlScheme(MainMenuManager.MetronomeID);
            Debug.Log($"Le métronome est contrôlé par : {MainMenuManager.MetronomeID.displayName} ID{MainMenuManager.MetronomeID.deviceId}");
         }
         else
         {
            Debug.LogError("Le GameObject Métronome n'a pas de PlayerInput !");
         }
      }
      else
      {
         Debug.LogError("Métronome non configuré correctement.");
      }
   }

   public void LoadWinRunner(string RunnerWin)
   {
      SceneManager.LoadScene("WinRunner");
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
         SceneManager.LoadScene("WinMetronome");
      }
   }
   
   private void ValidateDeviceIDs()
   {
      for (int i = 0; i < Gamepad.all.Count; i++)
      {
         Debug.Log($"Manette {i}: {Gamepad.all[i].displayName} (ID: {Gamepad.all[i].deviceId})");
      }
      
      foreach (Gamepad assignedGamepad in MainMenuManager.AssignedGamepads)
      {
         if (Gamepad.all.Contains(assignedGamepad))
         {
            Debug.Log($"Manette assignée valide : {assignedGamepad.displayName} (ID: {assignedGamepad.deviceId})");
         }
         else
         {
            Debug.LogWarning($"La manette assignée {assignedGamepad.displayName} n'est plus connectée.");
         }
      }
   }
}
