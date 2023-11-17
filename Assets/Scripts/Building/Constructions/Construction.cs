using UnityEngine;

namespace Building.Constructions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Construction : MonoBehaviour
    {
        [SerializeField] private Sprite[] _gradeBuildings;
        
        private int _currentGrade;
        private SpriteRenderer _spriteRenderer;

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        public Sprite GetGrade()
        {
            if (CanUpgrade()) _currentGrade++;

            return _gradeBuildings[_currentGrade - 1];
        }

        public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;

        public bool CanUpgrade() => _currentGrade < _gradeBuildings.Length;
    }
}