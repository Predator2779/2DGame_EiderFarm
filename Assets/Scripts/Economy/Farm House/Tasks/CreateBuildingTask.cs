using Building;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/BuildTask", fileName = "New BuildTask", order = 0)]
    public class CreateBuildingTask : CollectTask
    {
        [SerializeField] private GameObject[] _buildMenus;
        private int _countBuildings;

        protected override void Initialize()
        {
            _countBuildings = GetActiveBuildMenus();
        }

        protected override void Deinitialize() { }

        private int GetActiveBuildMenus()
        {
            int count = 0;
            for (int i = 0; i < _buildMenus.Length; i++)
            {
                if (_buildMenus[i].GetComponent<BuildMenu>())
                {
                    count++;
                }
            }

            _currentCount = _countBuildings + count;
            return count;
        }
        
        protected override bool SomeCondition() => GetActiveBuildMenus() == _countBuildings + _requireCount;

        [ContextMenu("Reset Task")]
        public override void ResetTask()
        {
            _countBuildings = GetActiveBuildMenus();

            if (!SomeCondition())
            {
                SetStage(TaskStage.NotStarted);
            }
            else
            {
                SetStage(TaskStage.Completed);
            }
        }
    }
}