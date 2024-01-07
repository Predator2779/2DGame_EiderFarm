using Building;
using Building.Constructions;
using Economy;
using Characters;
using General;
using Other;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TriggerScripts
{
    [SelectionBase]
    public class BuildTrigger : MenuTrigger
    {
        [SerializeField] private Tilemap _map;
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private Construction _buildPrefab;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Person _person;
        
        private Transform _parentBuildings;

        private void Start() => Initialize();

        public void Initialize()
        {
            _map = transform.GetComponentInParent<Tilemap>();
            _buildMenu.SetButtons();
            
            SetParent(_buildPrefab.typeConstruction);
            SetSprite();
            SetPosition();
            AddToPull();
        }

        private void SetSprite()
        {
            if (_renderer != null) _renderer.sprite = _buildPrefab.GetFirstGrade();
        }

        private void SetPosition() => transform.position = GetTilePos();
        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));
        private Quaternion GetRotation() => transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;
        public void AddToPull() => EventHandler.OnAddedBuildPull.Invoke(this, GetTypeBuilding());
        public void RemoveFromPull() => EventHandler.OnRemovedBuildPull?.Invoke(this, GetTypeBuilding());
        private GlobalTypes.TypeBuildings GetTypeBuilding() => _buildPrefab.typeConstruction;
        public bool IsOccupied() => _person != null;

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


        protected override void OnTriggerEnter2D(Collider2D other) /// to Physics.Overlap
        {
            base.OnTriggerEnter2D(other);
            
            if (other.TryGetComponent(out Person person) && _person == null) _person = person;
            else return;

            if (person.TryGetComponent(out InputHandler inputHandler))
                _person = person;
            
            if (_person.GetName() != GlobalConstants.PlayerName) return;

            SetConstruction();

            var inventory = _person.GetComponent<Inventory>();
            
            _buildMenu.SetInventory(inventory);

            if (inventory.TryGetBunch(GlobalConstants.Flag, out ItemBunch bunch))
                _buildMenu.HasFlag = bunch.GetCount() > 0;

            _buildMenu.CheckBtns();
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);

            if (other.TryGetComponent(out Person person) && person.GetName() == _person.GetName()) // employee стоящие в триггере зависнут.
                _person = null;
        }

        public void RemovePlace()
        {
            _buildMenu.Demolition();
            gameObject.SetActive(false);
        }

        public BuildMenu GetBuildMenu() => _buildMenu;
    }
}