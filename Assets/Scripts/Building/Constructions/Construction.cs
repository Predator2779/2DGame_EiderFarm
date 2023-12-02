using General;
using UnityEngine;

namespace Building.Constructions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Construction : MonoBehaviour
    {
        public GlobalTypes.TypeBuildings typeConstruction;
        
        [SerializeField] private Sprite[] _gradeBuildings;
        [SerializeField] private int _currentGrade;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        public Sprite Upgrade()
        {
            if (CanUpgrade()) _currentGrade++;

            return _gradeBuildings[_currentGrade - 1];
        }

        public Sprite GetFirstGrade() => _gradeBuildings[0];
        
        public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;

        public bool CanUpgrade() => _currentGrade < _gradeBuildings.Length;

        public int GetCurrentGrade() => _currentGrade;
    }
}