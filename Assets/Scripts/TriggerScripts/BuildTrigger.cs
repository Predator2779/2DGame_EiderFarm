using Building;
using Building.Constructions;
using General;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TriggerScripts
{
    public class BuildTrigger : MenuTrigger
    {
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private Construction _buildPrefab;
        [SerializeField] private SpriteRenderer _renderer;
        
        private Transform _parentBuildings;
        private Tilemap _map;

        private void Awake() => Initialize();

        private void Initialize()
        {
            _map = transform.GetComponentInParent<Tilemap>();
            
            SetParent(_buildPrefab.typeConstruction);
            SetSprite();
            SetPosition();
        }

        private void SetSprite()
        {
            if (_renderer != null) _renderer.sprite = _buildPrefab.GetFirstGrade();
        }

        private void SetPosition() => transform.position = GetTilePos();

        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));

        private Quaternion GetRotation() => transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;

        private void SetParent(GlobalTypes.TypeBuildings type)
        {
            Transform parent = transform.parent;
            string name = type.ToString().ToUpper();
            int length = parent.childCount;

            for (int i = 0; i < length; i++)
            {
                Transform child = parent.GetChild(i);
                
                if (child.name == name)
                {
                    _parentBuildings = child;
                    _parentBuildings.name = name;
                    return;
                }
            }
            
            _parentBuildings = Instantiate(new GameObject(), parent).transform;
            _parentBuildings.name = name;
        }
        
        public void SetConstruction() => 
                _buildMenu.SetConstruction(
                _buildPrefab, 
                _parentBuildings,
                _renderer, 
                GetTilePos(), 
                GetRotation());


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            SetConstruction();
        }
    }
}