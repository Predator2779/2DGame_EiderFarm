using UnityEngine;

public class Following : MonoBehaviour
{
    public Transform followObject;
    
    [SerializeField] private float _speed = 100;

    private void LateUpdate()
    {
        Vector3 direction = GetDirection();
        transform.position = new Vector3(direction.x, direction.y, transform.position.z);
    }
            
    
    private Vector3 GetDirection() => Vector3.MoveTowards(
            transform.position,
            followObject.position,
            _speed * Time.deltaTime);
}