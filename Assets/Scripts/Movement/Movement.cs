using UnityEngine;

public class Movement : MonoBehaviour
{
    public void Move(Vector2 direction) => transform.Translate(direction);
}