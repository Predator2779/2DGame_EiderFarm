using UnityEngine;

public class Movement : MonoBehaviour
{
    public virtual void Move(Vector2 direction) => transform.Translate(direction);
}