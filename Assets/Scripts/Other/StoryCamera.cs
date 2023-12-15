using UnityEngine;

public class StoryCamera : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _limit;

    private void Update()
    {
        var scale = _speed * Time.deltaTime;

        if (transform.localScale.x > _limit) return;
        
        transform.localScale += new Vector3(scale, scale, scale);
    }
}