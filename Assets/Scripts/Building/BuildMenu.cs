using Building.Constructions;
using Economy;
using General;
using TMPro;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        private Construction _buildingPrefab;
        private Construction _currentBuilding;

        private Transform _parent;
        private SpriteRenderer _triggerSprite;
        private Vector3 _buildPos;
        private Quaternion _buildRot;

        private GameObject _buildBtn;
        private GameObject _upgradeBtn;
        private GameObject _demolitionBtn;

        [SerializeField] private GameObject _flagBtn;
        [SerializeField] private Flag _flag;

        [Header("Стоимость постройки")]
        [SerializeField, Range(0, 1000)] private int _buyPrice;

        [Header("Стоимость улучшений")]
        [SerializeField, Range(0, 1000)] private int[] _upgradePrice = new int[1];

        [Header("Сколько возвращает при сносе (0 если 0)")]
        [SerializeField, Range(0, 1000)] private int _sellPrice;

        [SerializeField] private Inventory _inventory;
        private ItemBunch _moneyBunch;

        public bool HasFlag;
        public bool IsBuilded;

        public void SetConstruction(
                Construction prefab,
                Transform parent,
                SpriteRenderer triggerSprite,
                Vector3 buildPos,
                Quaternion buildRot)
        {
            _buildingPrefab = prefab;
            _parent = parent;
            _triggerSprite = triggerSprite;
            _buildPos = buildPos;
            _buildRot = buildRot;
        }

        public void SetButtons()
        {
            _buildBtn = transform.Find("BuildBtn").gameObject;
            _upgradeBtn = transform.Find("UpgradeBtn").gameObject;
            _demolitionBtn = transform.Find("DemolitionBtn").gameObject;
        }

        public void Build()
        {
            if (_currentBuilding != null) return;

            if (!Buy(_buyPrice)) return;

            if (_triggerSprite != null) _triggerSprite.enabled = false;

            Build(_buildingPrefab);
            _currentBuilding.SetSprite(_currentBuilding.Upgrade());

            IsBuilded = true;

            FMODUnity.RuntimeManager.PlayOneShotAttached(_buildingPrefab.GetBuildSound(), gameObject);

            CheckBtns();

            EventHandler.OnBuilded?.Invoke(_currentBuilding.typeConstruction);
        }

        public void Build(bool isFree)
        {
            if (_currentBuilding != null) return;

            if (_triggerSprite != null)
                _triggerSprite.enabled = false;

            Build(_buildingPrefab);
            _currentBuilding.SetSprite(_currentBuilding.Upgrade());
            IsBuilded = true;

            CheckBtns();
        }

        public void Demolition()
        {
            if (_currentBuilding == null) return;

            _triggerSprite.enabled = true;
            Sell(_sellPrice);
            IsBuilded = false;

            EventHandler.OnDemolition?.Invoke(_currentBuilding.typeConstruction);
            Destroy(_currentBuilding.gameObject);

            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI меню стройки домик/Снести");

            CheckBtns();

            if (_flag != null) _flag.RemoveFlag();
        }

        public void Upgrade()
        {
            if (_currentBuilding == null || !_currentBuilding.CanUpgrade()) return;
            
            if (!Buy(_upgradePrice[_currentBuilding.GetCurrentGrade() -1])) return;
            
            _currentBuilding.SetSprite(_currentBuilding.Upgrade());

            if (_currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.GagaHouse)
                _currentBuilding.GetComponent<FluffGiver>().CheckGrade();

            if (_currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.FluffCleaner ||
                _currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.ClothMachine)
                _currentBuilding.GetComponent<Machine>().CheckGrade();

            if (_currentBuilding.GetComponent<ResourceTransmitter>() && _currentBuilding.GetComponent<Machine>())
                _currentBuilding.GetComponent<ResourceTransmitter>()
                                .SetGradeAnimationTrue(_currentBuilding.GetCurrentGrade());

            if (_currentBuilding.GetComponent<ResourceTransmitter>() && _currentBuilding.GetComponent<Machine>())
                _currentBuilding.GetComponent<ResourceTransmitter>()
                                .SetGradeAnimationTrue(_currentBuilding.GetCurrentGrade());

            if (_currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.Storage)
                _currentBuilding.GetComponent<StorageSpriteChanger>().ChangeToGradeSprite();

            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI меню стройки домик/Улучшить");

            EventHandler.OnUpgraded?.Invoke(
                    _currentBuilding.typeConstruction,
                    _currentBuilding.GetCurrentGrade());

            CheckBtns();
        }

        public void Upgrade(bool isFree)
        {
            if (_currentBuilding == null || !_currentBuilding.CanUpgrade()) return;

            _currentBuilding.SetSprite(_currentBuilding.Upgrade());

            if (_currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.GagaHouse)
            {
                var giver = _currentBuilding.GetComponent<FluffGiver>();

                giver.Initialize();
                giver.CheckGrade();
            }

            if (_currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.FluffCleaner ||
                _currentBuilding.typeConstruction == GlobalTypes.TypeBuildings.ClothMachine)
            {
                var machine = _currentBuilding.GetComponent<Machine>();

                machine.Initialize();
                machine.CheckGrade();
            }

            if (_currentBuilding.GetComponent<ResourceTransmitter>() && _currentBuilding.GetComponent<Machine>())
                _currentBuilding.GetComponent<ResourceTransmitter>()
                                .SetGradeAnimationTrue(_currentBuilding.GetCurrentGrade());

            if (_currentBuilding.GetComponent<ResourceTransmitter>() && _currentBuilding.GetComponent<Machine>())
                _currentBuilding.GetComponent<ResourceTransmitter>()
                                .SetGradeAnimationTrue(_currentBuilding.GetCurrentGrade());

            EventHandler.OnUpgraded?.Invoke(
                    _currentBuilding.typeConstruction,
                    _currentBuilding.GetCurrentGrade());
        }

        public void CheckBtns()
        {
            if (_buildBtn == null || _upgradeBtn == null || _demolitionBtn == null) return;

            _buildBtn.SetActive(false);
            _upgradeBtn.SetActive(false);
            _demolitionBtn.SetActive(false);

            if (!IsBuilded)
            {
                _buildBtn.transform.Find("Price").GetComponentInChildren<TMP_Text>().text = _buyPrice + "kr";
                _buildBtn.SetActive(true);
            }
            else
            {
                if (_currentBuilding.GetCurrentGrade() < _currentBuilding.GetMaxGrade())
                {
                    _upgradeBtn.transform.Find("Price").GetComponentInChildren<TMP_Text>().text =
                            _upgradePrice[_currentBuilding.GetCurrentGrade() -1] + "kr";
                    _upgradeBtn.SetActive(true);
                }

                _demolitionBtn.SetActive(true);

                if (_flagBtn != null) _flagBtn.SetActive(HasFlag);
            }
        }

        public Construction GetBuilding() => _currentBuilding;

        private void Build(Construction building)
        {
            if (_currentBuilding != null) Destroy(_currentBuilding);
            _currentBuilding = Instantiate(building, _buildPos, _buildRot, _parent);
        }

        public void SetFlag() => _flag.AddFlag();

        private bool Buy(int price)
        {
            if (!_inventory.TryGetBunch(GlobalConstants.Money, out var moneyBunch) ||
                moneyBunch.GetCount() < price) return false;

            _inventory.RemoveItems(moneyBunch.GetItem(), price);
            
            return true;
        }

        private void Sell(int price)
        {
            if (_moneyBunch != null) _inventory.AddItems(_moneyBunch.GetItem(), price);
        }

        public void SetInventory(Inventory inv) => _inventory = inv;
    }
}