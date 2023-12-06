using Building;
using Building.Constructions;
using Economy;
using Characters;
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

        private string _personName;
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

        public bool IsOccupied() => _personName != "";

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
                SetConstruction();
                _buildMenu.SetInventory(other.gameObject.GetComponent<Inventory>());
                if (other.gameObject.GetComponent<Inventory>().
                          TryGetBunch(GlobalConstants.Flag, out var bunch)) 
                    _buildMenu.HasFlag = bunch.GetCount() > 0;

                _buildMenu.CheckBtns();
            }

            if (other.TryGetComponent(out Person person) &&
                person.GetName() == _personName)
                _personName = person.GetName();

            base.OnTriggerEnter2D(other);
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            _buildMenu.SetInventory(null);
            if (other.TryGetComponent(out Person person) &&
                person.GetName() == _personName) _personName = "";
        }

        public void RemovePlace()
        {
            _buildMenu.Demolition();
            gameObject.SetActive(false);
        }

        public BuildMenu GetBuildMenu() => _buildMenu;
    }
}