using General;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysMovement : Movement
{
    private Rigidbody2D _rbody;
    private void Awake() => _rbody = GetComponent<Rigidbody2D>();

    public override void Move(Vector2 direction)
    {
        if (_rbody != null) _rbody.AddForce(direction * GlobalConstants.CoefPhysPlayerSpeed);
    }
}