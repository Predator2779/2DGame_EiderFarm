using General;
using UnityEngine;

namespace Building.Constructions
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Construction : MonoBehaviour
    {
        public GlobalTypes.TypeBuildings typeConstruction;

        [SerializeField] private Transform _entryPoint;
        [SerializeField] private Sprite[] _gradeBuildings;
        [SerializeField] private int _currentGrade;

        private SpriteRenderer _spriteRenderer;
        private ResourceTransmitter _transmitter;

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

        public Sprite Upgrade()
        {
            if (CanUpgrade()) _currentGrade++;

            return _gradeBuildings[_currentGrade - 1];
        }

        public Sprite GetFirstGrade() => _gradeBuildings[0];

        public Sprite[] GetGradeBuildings() => _gradeBuildings;

        public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;

        public bool CanUpgrade() => _currentGrade < _gradeBuildings.Length;

        public int GetCurrentGrade() => _currentGrade;

        public ResourceTransmitter GetTransmitter() => _transmitter;

        public Transform GetEntryPoint() => _entryPoint;
        
        public Sprite GetCurrentGradeSprite(Sprite[] sprites)
        {
            switch (GetCurrentGrade())
            {
                case 1: return sprites[0];
                case 2: return sprites[1];
                case 3: return sprites[2];
            }
            return sprites[0];
        }
    }
}