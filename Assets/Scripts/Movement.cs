using UnityEngine;

public class Movement : MonoBehaviour, IMovable
{
    public void Move(Vector3 direction)
    {
        Vector3 movement = direction.normalized * Time.deltaTime;
        transform.Translate(movement);
    }
}