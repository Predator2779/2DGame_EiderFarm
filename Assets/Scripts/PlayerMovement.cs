using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    [Header("Скорость движения персонажа.")]
    [SerializeField, Range(0, 100)] private float _speed;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_joystick.Horizontal, _joystick.Vertical, 0);
        
        if(direction.magnitude > 0.1f)
        {
            Vector3 movement = direction.normalized * _speed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
}




