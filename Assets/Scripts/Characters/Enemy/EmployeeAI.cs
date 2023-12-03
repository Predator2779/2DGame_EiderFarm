namespace Characters.Enemy
{
    public class EmployeeAI : PersonAI
    {
        private void Start() => SetNullableFields();
        private void OnValidate() => SetNullableFields();
        private void Update() => CheckConditions();
        private void FixedUpdate() => StateExecute();

        private void SetNullableFields()
        {
            _person ??= GetComponent<Employee>();
        }
        
        protected override void StateExecute()
        {
            throw new System.NotImplementedException();
        }

        protected override void Idle()
        {
            throw new System.NotImplementedException();
        }

        protected override void CheckConditions()
        {
            throw new System.NotImplementedException();
        }
    }
}