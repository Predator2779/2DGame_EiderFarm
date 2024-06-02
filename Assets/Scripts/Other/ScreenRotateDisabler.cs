using UnityEngine;

public class ScreenRotateDisabler : MonoBehaviour
{
    private void Update() => Screen.orientation = ScreenOrientation.Portrait;
}