using UnityEngine;

namespace TriggerScripts
{
    public class MenuTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _panelUI;
        private void SetMenuActive(bool value) => _panelUI.gameObject.SetActive(value);
    
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) SetMenuActive(true);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) SetMenuActive(false);
        }
    }
}