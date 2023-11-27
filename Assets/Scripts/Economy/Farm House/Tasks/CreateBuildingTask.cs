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
            _countBuildings = FindBuildings(_buildType);
        }

        protected override void Deinitialize() { }

        private int GetActiveBuildMenus()
        {
            int count = 0;
            
            // for (int i = 0; i < _buildMenus.Length; i++)
            // {
            //     if (_buildMenus[i].GetComponent<BuildMenu>())
            //     {
            //         count++;
            //     }
            // }
            //
            // _currentCount = _countBuildings + count;
            
            
            return count;
        }
        
        private int FindBuildings(GlobalTypes.TypeBuildings type)
        {
            int countBuildings = 0;
            string name = type.ToString().ToUpper();
            int length = _pathBuildings.childCount;

            for (int i = 0; i < length; i++)
            {
                Transform child = _pathBuildings.GetChild(i);

                if (child.name == name)
                    countBuildings++;
            }

            return countBuildings;
        }
        
        protected override bool SomeCondition() => GetActiveBuildMenus() == _countBuildings + _requireCount;

        [ContextMenu("Reset Task")]
        public override void ResetTask()
        {
            _countBuildings = GetActiveBuildMenus();

            SetStage(!SomeCondition() ? TaskStage.NotStarted : TaskStage.Completed);
        }
    }
}