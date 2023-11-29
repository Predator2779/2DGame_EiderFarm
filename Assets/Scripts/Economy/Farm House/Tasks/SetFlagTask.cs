using Economy.Farm_House;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/CollectTasks/BuildTask", fileName = "New BuildTask", order = 0)]
public class SetFlagTask : CollectTask
{
    [SerializeField] private int _count;

    private int _current;

    protected override void Initialize()
    {
        _currentCount = 0;

        EventHandler.OnFlagSet.AddListener(SetFlag);
    }

    protected override void Deinitialize()
    {
        EventHandler.OnFlagSet.RemoveListener(SetFlag);
    }

    private void SetFlag()
    {
        _current++;
        CheckProgressing();
    }

    protected override bool SomeCondition() => _current >= _count;
}
