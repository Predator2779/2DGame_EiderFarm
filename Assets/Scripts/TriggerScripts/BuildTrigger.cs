using Building;
using Building.Constructions;
using Economy;
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
            _buildMenu.SetButtons();

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

                if (child.name != name) continue;

                _parentBuildings = child;
                _parentBuildings.name = name;
                return;
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
            if (other.gameObject.GetComponent<InputHandler>())
            {
                base.OnTriggerEnter2D(other);
                SetConstruction();
                if (other.gameObject.GetComponent<Inventory>().TryGetBunch(GlobalConstants.Flag, out var bunch))
                    if (bunch.GetCount() > 0)
                        _buildMenu.HasFlag = true;
                else
                        _buildMenu.HasFlag = false;
                _buildMenu.CheckBtns();
            }
        }

        public void RemovePlace()
        {
            _buildMenu.Demolition();
            gameObject.SetActive(false);
        }
        
        public BuildMenu GetBuildMenu() => _buildMenu;
    }
}