using Economy.Farm_House;
using UnityEngine;
using UnityEngine.Events;
using EventHandler = General.EventHandler;

public class TaskEvent : MonoBehaviour
{
    [SerializeField] private Task _task;
    public UnityEvent myEvent;

    private void Start()
    {
        myEvent?.Invoke();
        EventHandler.OnTaskStageChanged.AddListener(CheckTask);
    }

    private void CheckTask(Task task, TaskStage stage)
    {
        if (task == _task && stage == TaskStage.Completed)
            myEvent?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        myEvent?.Invoke();
    }
}