using System.Linq;
using Building.Constructions;
using General;
using UnityEngine;

namespace Economy.Farm_House.Tasks.TypesTask.BuildingTasks
{
    [CreateAssetMenu(menuName = "Tasks/BuildTasks/UpgradeBuildTask", fileName = "New UpgradeBuildTask", order = 0)]
    public class UpgradeBuildTask : CreateBuildTask
    {
        [SerializeField] private int _requireGrade;

        protected override void Initialize()
        {
            _pathBuildings = GameObject.Find("Tilemap-Grids").transform;
            _currentCount = GetUpgradeBuildingsCount(GetBuildings(_buildType).ToArray());

            EventHandler.OnUpgraded.AddListener(Upgrade);
        }

        protected override void Deinitialize() => EventHandler.OnUpgraded.RemoveListener(Upgrade);

        private void Upgrade(GlobalTypes.TypeBuildings type, int grade)
        {
            if (type == _buildType &&
                _stage == TaskStage.Progressing &&
                grade == _requireGrade)
                AddCount(1);

            ProgressingTask();
        }

        private int GetUpgradeBuildingsCount(Construction[] buildings) =>
                buildings.Count(building => building.GetCurrentGrade() >= _requireGrade);

        protected override bool SomeCondition() => _currentCount >= _requireCount;
    }
}