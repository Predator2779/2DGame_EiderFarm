using Economy.Farm_House;
using UnityEngine;
using UnityEngine.Events;
using EventHandler = General.EventHandler;

public class TaskEvent : MonoBehaviour
{
    [SerializeField] private Task _task;
    [SerializeField] private UnityEvent _myEvent;

    private void Start() => EventHandler.OnTaskStageChanged.AddListener(CheckTask);

    private void CheckTask(Task task, TaskStage stage)
    {
        if (task == _task && _task.GetStage() == TaskStage.Passed)
            _myEvent?.Invoke();
    }
}