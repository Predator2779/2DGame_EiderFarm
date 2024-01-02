using System.Collections;
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
        [SerializeField] private float _buildSpeed = 0.1f;
        [SerializeField] private string _buildSound;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
        private void Build()
        {
            if (gameObject.TryGetComponent(out PolygonCollider2D col) &&
                col.CompareTag("Obstacle"))
                Destroy(col);
            
            gameObject.AddComponent<PolygonCollider2D>().tag = "Obstacle";
            transform.localScale = Vector3.zero;
            StartCoroutine(SmoothlyBuild());
        }

        public void Upgrade(int currentGrade)
        {
            _currentGrade = currentGrade;
            if (CanUpgrade()) _currentGrade++;
            
            SetSprite(_gradeBuildings[_currentGrade - 1]); 
            Build();
        }
        
        public Sprite GetFirstGrade() => _gradeBuildings[0];
        public Sprite[] GetGradeBuildings() => _gradeBuildings;
        public string GetBuildSound() => _buildSound;
        public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;
        public bool CanUpgrade() => _currentGrade < _gradeBuildings.Length;
        public int GetCurrentGrade() => _currentGrade;
        public int GetMaxGrade() => _gradeBuildings.Length;
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

        private IEnumerator SmoothlyBuild()
        {
            while (transform.localScale != Vector3.one)
            {
                transform.localScale += new Vector3(_buildSpeed, _buildSpeed, _buildSpeed);

                if (transform.localScale.x > 1)
                    transform.localScale = Vector3.one;

                yield return null;
            }
        }
    }
}