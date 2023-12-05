using General;
using UnityEngine;

namespace Economy.Farm_House.Tasks.TypesTask.BuildingTasks
{
    [CreateAssetMenu(menuName = "Tasks/BuildTasks/DemolitionTask", fileName = "New DemolitionTask", order = 0)]
    public class DemolitionTask : CreateBuildTask
    {
        protected override void Initialize()
        {
            SetNullableFields();
            EventHandler.OnDemolition.AddListener(CheckBuilding);
        }

        protected override void Deinitialize() =>
                EventHandler.OnDemolition.RemoveListener(CheckBuilding);
        
        protected override bool SomeCondition() => _currentCount >= _requireCount;
    }
}