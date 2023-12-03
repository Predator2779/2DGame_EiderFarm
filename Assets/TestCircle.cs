using UnityEngine;

public class TestCircle : MonoBehaviour
{
    public void Test()
    {
        print("Tested");
        GetComponent<SpriteRenderer>().color = Random.ColorHSV();
    }
}