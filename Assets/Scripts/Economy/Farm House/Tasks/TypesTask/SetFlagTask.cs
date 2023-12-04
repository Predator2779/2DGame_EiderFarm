using Economy.Farm_House;
using General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/CollectTasks/SetFlagTasks", fileName = "New SetFlagTask", order = 0)]
public class SetFlagTask : CollectTask
{
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
        _currentCount++;
        CheckProgressing();
    }

    protected override bool SomeCondition() => _currentCount >= _requireCount;
}
