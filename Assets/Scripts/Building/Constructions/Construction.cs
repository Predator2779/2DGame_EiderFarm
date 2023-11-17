using UnityEngine;

namespace Building.Constructions
{
    public class Construction : MonoBehaviour
    {
        [SerializeField] private Sprite[] _gradeBuildings;
        [SerializeField] private int _currentGrade;
        
        public bool isBuilded;

        private void Start() => Reset();

        public Sprite GetGrade()
        {
            if (CanBuild()) _currentGrade++;

            return _gradeBuildings[_currentGrade - 1];
        }
        
        public void Upgrade()
        {
            /// повышение характеристик...
        }
        
        public bool CanBuild() => _currentGrade < _gradeBuildings.Length;

        public void Reset()
        {
            isBuilded = false;
            _currentGrade = 0;
        }
    }
}