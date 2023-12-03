using System.Collections.Generic;
using Building.Constructions;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/CreateBuildTask", fileName = "New CreateBuildTask", order = 0)]
    public class CreateBuildTask : CollectTask
    {
        [SerializeField] protected GlobalTypes.TypeBuildings _buildType;

        protected Transform _pathBuildings;
        protected List<Construction> _buildings;
        protected int _countBuildings;

        protected override void Initialize()
        {
            _pathBuildings = GameObject.Find("Tilemap-Buildings").transform;
            _buildings = GetBuildings(_buildType);
            _countBuildings = _buildings.Count;

            EventHandler.OnBuilded.AddListener(Build);
        }

        protected override void Deinitialize()
        {
            EventHandler.OnBuilded.RemoveListener(Build);
        }

        private void Build(GlobalTypes.TypeBuildings type)
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

        protected override bool SomeCondition() => _currentCount >= _requireCount - _countBuildings;

        [ContextMenu("Reset Task")] public override void ResetTask()
        {
            base.ResetTask();
            _currentCount = 0;
        }
    }
}