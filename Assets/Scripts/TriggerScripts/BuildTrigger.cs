using Building;
using Building.Constructions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TriggerScripts
{
    public class BuildTrigger : MenuTrigger
    {
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private Construction _buildPrefab;
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
            if (_renderer != null) _renderer.sprite = _buildPrefab.GetFirstGrade();
        }

        private void SetPosition() => transform.position = GetTilePos();

        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));

        private Quaternion GetRotation() => transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;

        public void SetPos() => _buildMenu.SetConstruction(_buildPrefab, _renderer, GetTilePos(), GetRotation());


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            _buildMenu.SetConstruction(_buildPrefab, _renderer, GetTilePos(), GetRotation());
        }
    }
}