using UnityEngine;

public class StoryCamera : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        var scale = _speed * Time.deltaTime;

        transform.localScale += new Vector3(scale, scale, scale);
    }
}