using Building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TriggerScripts
{
    public class BuildTrigger : MenuTrigger
    {
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private SpriteRenderer _renderer;
        private Tilemap _map;

        private void Awake() => Initialize();

        private void Initialize()
        {
            _map = transform.parent.GetComponent<Tilemap>();

            SetSprite();
            SetPosition();
        }

        private void SetSprite()
        {
            if (_renderer != null) _renderer.sprite = _buildMenu.buildingPrefab.GetFirstGrade();
        }

        private void SetPosition() => transform.position = GetTilePos();

        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));

        private Quaternion GetRotation() => transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            _buildMenu.SetPosition(_renderer, GetTilePos(), GetRotation());
        }
    }
}