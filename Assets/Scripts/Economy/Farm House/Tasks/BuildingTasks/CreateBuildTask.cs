using System.Collections.Generic;
using Building.Constructions;
using General;
using UnityEngine;

namespace Economy.Farm_House.Tasks.TypesTask.BuildingTasks
{
    [CreateAssetMenu(menuName = "Tasks/BuildTasks/CreateBuildTask", fileName = "New CreateBuildTask", order = 0)]
    public class CreateBuildTask : CollectTask
    {
        [SerializeField] protected GlobalTypes.TypeBuildings _buildType;

        protected Transform _pathBuildings;
        private List<Construction> _buildings;

        protected override void Initialize()
        {
            SetNullableFields();
            EventHandler.OnBuilded.AddListener(Build);
        }

        protected void SetNullableFields()
        {
            _pathBuildings = GameObject.Find("Tilemap-Buildings").transform;
            _buildings = GetBuildings(_buildType);
            //_currentCount = _buildings.Count;
        }

        protected override void Deinitialize() => EventHandler.OnBuilded.RemoveListener(Build);

        private void Build(GlobalTypes.TypeBuildings type) => CheckBuilding(type);

        protected void CheckBuilding(GlobalTypes.TypeBuildings type)
        {
            if (type == _buildType &&
                _stage == TaskStage.Progressing &&
                _currentCount + 1 <= _requireCount)
                AddCount(1);

            ProgressingTask();
        }

        protected List<Construction> GetBuildings(GlobalTypes.TypeBuildings type)
        {
            int length = _pathBuildings.childCount;
            string path = type.ToString().ToUpper();

            List<Construction> buildings = new();

            for (int i = 0; i < length; i++)
            {
                var child = _pathBuildings.GetChild(i);
                var count = child.childCount;

                if (child.name == path && count > 0)
                    for (int j = 0; j < count; j++)
                    {
                        var building = child.GetChild(j);

                        if (building.TryGetComponent(out Construction construction))
                            if (construction.typeConstruction == type)
                                buildings.Add(construction);
                    }
            }

            return buildings;
        }

        protected override bool SomeCondition() => _currentCount >= _requireCount;

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();
            _currentCount = 0;
        }
    }
}