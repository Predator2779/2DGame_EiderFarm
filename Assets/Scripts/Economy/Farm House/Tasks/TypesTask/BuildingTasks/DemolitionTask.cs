using TriggerScripts;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/BuildTasks/DemolitionTask", fileName = "New DemolitionTask", order = 0)]
    public class DemolitionTask : CollectTask
    {
        [SerializeField] private BuildTrigger[] _buildTriggers;
        
        protected override void Initialize() { }
        protected override void Deinitialize() { }
        protected override bool SomeCondition()
        {
            _requireCount = _buildTriggers.Length;
            _currentCount = 0;

            for (int i = 0; i < _requireCount; i++)
            {
                if (!_buildTriggers[i].GetBuildMenu().IsBuilded)
                    _currentCount++;
            }
            
            return _currentCount >= _requireCount;
        }
    }
}