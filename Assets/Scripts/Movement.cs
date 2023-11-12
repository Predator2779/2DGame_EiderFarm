using UnityEngine;

public class Movement : MonoBehaviour
{
    public void Move(Vector3 direction) => transform.Translate(direction);
}