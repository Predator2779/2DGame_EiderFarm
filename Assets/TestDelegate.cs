using UnityEngine;

public class TestDelegate : MonoBehaviour
{
    public delegate void Test();

    public Test testEvent;

    private void Start()
    {
        testEvent += Test2;
    }

    private void Test2()
    {
        print("tested");
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}