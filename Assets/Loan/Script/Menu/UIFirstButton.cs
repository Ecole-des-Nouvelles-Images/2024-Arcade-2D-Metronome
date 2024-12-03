using UnityEngine;
using UnityEngine.EventSystems;

namespace Loan.Script.Menu
{
    public class UIFirstButton : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject Button;
        private void OnEnable()
        {
            eventSystem.SetSelectedGameObject(Button.gameObject);
        }
    }
}
