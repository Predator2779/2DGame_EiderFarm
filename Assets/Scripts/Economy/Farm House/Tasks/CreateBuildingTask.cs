using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/BuildTask", fileName = "New BuildTask", order = 0)]
    public class CreateBuildingTask : CollectTask
    {
        [SerializeField] private GlobalTypes.TypeBuildings _buildType;

        private Transform _pathBuildings;
        private int _countBuildings;

        protected override void Initialize()
        {
            _pathBuildings = GameObject.Find("Tilemap-Buildings").transform;
            _countBuildings = GetCountBuildings();

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
                _currentCount++;

            CheckProgressing();
        }

        private int GetCountBuildings()
        {
            string name = _buildType.ToString().ToUpper();
            int length = _pathBuildings.childCount;

            Transform requirePath = null;

            for (int i = 0; i < length; i++)
            {
                Transform child = _pathBuildings.GetChild(i);

                if (child.name == name) requirePath = child;
            }
            
            return requirePath == null ? 0 : requirePath.childCount;
        }

        protected override bool SomeCondition() => GetCountBuildings() >= _countBuildings + _requireCount;
    }
}