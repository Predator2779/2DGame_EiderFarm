using Economy.Farm_House;
using UnityEngine;
using EventHandler = General.EventHandler;

public class TestEvents : MonoBehaviour
{
    [SerializeField] private Task _task;
    [SerializeField] private TestDelegate _testDelegate;
    
    private void Start()
    {
        EventHandler.OnTaskStageChanged.AddListener(CheckTask);
    }

    private void CheckTask(Task task, TaskStage stage)
    {
        if (task == _task && stage == TaskStage.Completed)
            _testDelegate.testEvent?.Invoke();
    }
}