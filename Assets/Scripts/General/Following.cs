using UnityEngine;

public class Following : MonoBehaviour
{
    [SerializeField] private Transform _followObject;
    [SerializeField] private float _speed;

    private void LateUpdate()
    {
        Vector3 direction = GetDirection();

        transform.position = new Vector3(direction.x, direction.y, transform.position.z);
    }
            
    
    private Vector3 GetDirection() => Vector3.MoveTowards(
            transform.position,
            _followObject.position,
            _speed * Time.deltaTime);
}