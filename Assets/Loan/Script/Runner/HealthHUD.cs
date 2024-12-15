using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Image[] _healthImage;
    [SerializeField] private Sprite _healthFull;
    [SerializeField] private Sprite _healthEmpty;
    
    public void UpdateHealth(float health)
    {
        for (int i = 0; i < _healthImage.Length; i++)
        {
            if (health > i)
            {
                _healthImage[i].sprite = _healthFull;
            }
            else
            {
                _healthImage[i].sprite = _healthEmpty;
            }
        }
    }
}
