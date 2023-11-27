using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks", fileName = "New BuildTask", order = 0)]
    public class CreateBuildingTask : CollectTask
    {
        [SerializeField] private GlobalTypes.TypeBuildings _buildType;

        private Transform _pathBuildings;
        private int _countBuildings;

        protected override void Initialize()
        {
            _pathBuildings = GameObject.Find("Tilemap-Buildings").transform;
            _countBuildings = GetCountBuildings();
        }

        protected override void Deinitialize() { }
        
        private int GetCountBuildings()
        {
            int countBuildings = 0;
            string name = _buildType.ToString().ToUpper();
            int length = _pathBuildings.childCount;

            for (int i = 0; i < length; i++)
            {
                Transform child = _pathBuildings.GetChild(i);

                if (child.name == name)
                    countBuildings++;
            }

            return countBuildings;
        }
        
        protected override bool SomeCondition() => GetCountBuildings() == _countBuildings + _requireCount;

        [ContextMenu("Reset Task")]
        public override void ResetTask()
        {
            _countBuildings = GetCountBuildings();

            SetStage(!SomeCondition() ? TaskStage.NotStarted : TaskStage.Completed);
        }
    }
}