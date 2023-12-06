using Building;
using Characters.AI;
using UnityEngine;

namespace Characters.Enemy
{
    public class EmployeeAI : PatrollerAI
    {
        [SerializeField] private EmployeeStates _currentState;
        [SerializeField] private BuildingsPull _pull;
        
        
        private void Start() => SetNullableFields();
        private void OnValidate() => SetNullableFields();
        private void Update() => CheckConditions();
        private void FixedUpdate() => StateExecute();

        private void SetNullableFields()
        {
            _walker ??= GetComponent<Employee>();
        }
        
        protected override void StateExecute()
        {
            switch (_currentState)
            {
                case EmployeeStates.Idle:
                    Idle();
                    break;
                case EmployeeStates.Picking:
                    Picking();
                    break;
                case EmployeeStates.Recycling:
                    Recycling();
                    break;
                case EmployeeStates.Transportation:
                    Transportation();
                    break;
            }
        }

        protected override void CheckConditions()
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Idle()
        {
            
        }  
        
        private void Picking()
        {
            throw new System.NotImplementedException();
        }  
        
        private void Recycling()
        {
            throw new System.NotImplementedException();
        }  
        
        private void Transportation()
        {
            throw new System.NotImplementedException();
        }

        private enum EmployeeStates
        {
            Idle, Picking, Recycling, Transportation
        }
    }
}