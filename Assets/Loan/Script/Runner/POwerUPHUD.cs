using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POwerUPHUD : MonoBehaviour
{
    [SerializeField] private Image powerUpBar;
    [SerializeField] private Sprite _powerSprite;
    
    public void UpdatePowerUpUI(float progress)
    {
        powerUpBar.sprite = _powerSprite;
        if (powerUpBar != null)
        {
            powerUpBar.fillAmount = progress;
        }
    }
}
