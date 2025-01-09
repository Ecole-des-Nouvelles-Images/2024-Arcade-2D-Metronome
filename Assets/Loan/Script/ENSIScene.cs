using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ENSIScene : MonoBehaviour
{
    [SerializeField] private float _speed = 0.38f;
    [SerializeField] private GameObject _slashScreen;
    private Image _image;
    private bool _isActive;

    private void Awake()
    {
        _image = _slashScreen.GetComponent<Image>();
    }

    private void Update()
    {
        float Alpha = _image.color.a;
        if (Alpha <= 1 && _isActive == false)
        {
            var Incress = Alpha += _speed * Time.deltaTime;
        
            _image.color = new Color(0,0,0,Incress);
            transform.DOScale(1, 2f);
            if (Alpha >= 1)
            {
                _isActive = true;
            }
        }
        if (_isActive)
        {
            var Incress = Alpha -= _speed * Time.deltaTime;
        
            _image.color = new Color(0,0,0,Incress);
            if (Alpha <= 0)
            {
                StartCoroutine(OpenMainMenu());
            }
        }
    }

    private System.Collections.IEnumerator OpenMainMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
