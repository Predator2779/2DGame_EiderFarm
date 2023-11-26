using UnityEngine;

namespace Economy.Farm_House
{
    public class ComplexTasks : Task
    {
        [SerializeField] private Task[] _subTasks;
        protected override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        protected override void Deinitialize()
        {
            throw new System.NotImplementedException();
        }

        protected override bool SomeCondition() => throw new System.NotImplementedException();

        public override void CheckProgressing()
        {
            throw new System.NotImplementedException();
        }

        public override void ResetTask()
        {
            throw new System.NotImplementedException();
        }
    }
}