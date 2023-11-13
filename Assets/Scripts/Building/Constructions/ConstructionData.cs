using UnityEngine;

namespace Building.Constructions
{
    [CreateAssetMenu(fileName = "New ConstrctnData", menuName = "ConstrctnDatas", order = 0)]
    public class ConstructionData : ScriptableObject
    {
        [SerializeField] public GameObject[] gradeBuildings;

        public bool isBuilded { get; set; }
        public int currentGrade;
        
        public bool CanBuild() => currentGrade < gradeBuildings.Length;
    }
}