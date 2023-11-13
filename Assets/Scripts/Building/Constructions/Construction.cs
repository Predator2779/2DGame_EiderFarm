using UnityEngine;

namespace Building.Constructions
{
    public class Construction : MonoBehaviour
    {
        [SerializeField] private GameObject[] _gradeBuildings;
        [SerializeField] private int _currentGrade;
        
        public bool isBuilded;

        private void Awake() => Reset();

        public GameObject GetBuilding()
        {
            if (CanBuild()) _currentGrade++;
            
            return _gradeBuildings[_currentGrade - 1];
        }
        
        public bool CanBuild() => _currentGrade < _gradeBuildings.Length;

        public void Reset()
        {
            isBuilded = false;
            _currentGrade = 0;
        }
    }
}