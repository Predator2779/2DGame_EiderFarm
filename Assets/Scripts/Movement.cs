using UnityEngine;

public class Movement : MonoBehaviour, IMovable
{
    public void Move(Vector3 direction, float speed)
    {
        Vector3 movement = direction.normalized * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}