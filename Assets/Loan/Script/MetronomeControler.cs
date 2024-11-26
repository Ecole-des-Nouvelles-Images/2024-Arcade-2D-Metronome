using UnityEngine;
using UnityEngine.UI;

public class MetronomeControler : MonoBehaviour
{
    [SerializeField] NoteSpawner _noteSpawner;
    
    private InputSysteme _inputSysteme;
    private Image _image;
    private int _score = 0;
    private bool _canPress = true;

    private void Start()
    {
        _inputSysteme = GetComponent<InputSysteme>();
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_inputSysteme.Active == 1 && _canPress)
        {
            HandleButtonPress();
        }
        
        //test
        if (_inputSysteme.PiegeLeft == 1)
        {
            Debug.Log("Piege gauche activé !");
        }

        if (_inputSysteme.PiegeUp == 1 && _score >= 5)
        {
            Debug.Log("Piege Haut activé !");
            _score = 0;
        }
        
        if (_inputSysteme.PiegeRight == 1)
        {
            Debug.Log("Piege Droit activé !");
        }
        
        if (_inputSysteme.PiegeDown == 1)
        {
            Debug.Log("Piege Bas activé !");
        }
    }

    private void HandleButtonPress()
    {
        _canPress = false;
        if (_noteSpawner.CheckNoteUnderImage())
        {
            AddScore(1);
            _image.color = Color.green; 
            Invoke(nameof(Reset),0.5f);
        }
        else
        {
            SubtractionScore(1);
            _image.color = Color.red;
            Invoke(nameof(Reset),0.5f);
        }

        StartCoroutine(ResetCanPress());
    }

    private System.Collections.IEnumerator ResetCanPress()
    {
        yield return new WaitForSeconds(0.1f);
        _canPress = true;
    }

    private void SubtractionScore(int points)
    {
        _score = Mathf.Max(0,_score - points);
        Debug.Log("Score : " + _score);
    }

    private void AddScore(int points)
    {
        _score += points;
        Debug.Log("Score : " + _score);
    }

    private void Reset()
    {
        _image.color = Color.white;
    }
}
