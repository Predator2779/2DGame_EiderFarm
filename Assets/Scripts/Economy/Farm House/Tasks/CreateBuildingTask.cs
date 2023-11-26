using Building;
using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/BuildTask", fileName = "New Build Task", order = 1)]
    public class CreateBuildingTask : Task
    {
        [SerializeField] private int _requiredBuildings;
        private int _countBuildings;
        [SerializeField] private int _currentCount;

        [SerializeField] private GameObject[] _buildMenus;

        protected override void Initialize()
        {
            _countBuildings = GetActiveBuildMenus();
        }

        protected override void Deinitialize()
        {
        }

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


        public int GetRequireCount() => _requiredBuildings;
        public int GetCurrentCount() => _currentCount;
        protected override bool SomeCondition() => GetActiveBuildMenus() == _countBuildings + _requiredBuildings;

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
