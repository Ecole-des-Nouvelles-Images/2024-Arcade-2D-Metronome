using UnityEngine;
using UnityEngine.SceneManagement;

public class ENSIScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(OpenMainMenu());
    }

    private System.Collections.IEnumerator OpenMainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
