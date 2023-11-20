using Building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TriggerScripts
{
    public class BuildTrigger : MenuTrigger
    {
        [SerializeField] private BuildMenu _buildMenu;
        private Tilemap _map;
        private SpriteRenderer _renderer;

        private void Awake() => Initialize();
        private void OnValidate() => Initialize();

        private void Initialize()
        {
            _map = transform.parent.GetComponent<Tilemap>();
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = _buildMenu.buildingPrefab.GetFirstGrade();
            
            SetPosition();
        }
        
        private void SetPosition() => transform.position = GetTilePos();
        
        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            _buildMenu.SetPosition(_renderer, GetTilePos());
        }
    }
}