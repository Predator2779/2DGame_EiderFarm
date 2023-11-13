using UnityEngine;

namespace Building.Constructions
{
    public class Construction : MonoBehaviour
    {
        [SerializeField] private ConstructionData _data;

        public GameObject GetBuilding()
        {
            Upgrade();
            
            return _data.gradeBuildings[_data.currentGrade - 1];
        }

        public ConstructionData GetData() => _data;

        private void Upgrade()
        {
            if (_data.CanBuild()) _data.currentGrade++;
        }
    }
}