using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    private void SetMenuActive(bool value) => _menu.gameObject.SetActive(value);
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
            
        SetMenuActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) SetMenuActive(false);
    }
}