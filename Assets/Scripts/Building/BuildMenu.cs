using System;
using Building.Constructions;
using Economy;
using General;
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

        private Inventory _inventory;

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

            if (!Buy(_buyPrice))
                return;

            if (_triggerSprite != null)
                _triggerSprite.enabled = false;


            Build(_buildingPrefab);
            _currentBuilding.SetSprite(_currentBuilding.Upgrade());
            IsBuilded = true;

            CheckBtns();

            EventHandler.OnBuilded?.Invoke(_currentBuilding.typeConstruction);
        }

        public void Demolition()
        {
            if (_currentBuilding == null) return;

            _triggerSprite.enabled = true;
            Sell(_sellPrice);
            IsBuilded = false;

            EventHandler.OnDemolition?.Invoke(_currentBuilding.typeConstruction);
            Destroy(_currentBuilding.gameObject);

            CheckBtns();

            if (_flag != null) _flag.RemoveFlag();
        }

        public void Upgrade()
        {
            if (_currentBuilding == null || !_currentBuilding.CanUpgrade()) return;

            switch (_currentBuilding.GetCurrentGrade())
            {
                case 1:
                    if (!Buy(_upgradePrice[0])) return;
                    break;
                case 2:
                    if (!Buy(_upgradePrice[1])) return;
                    break;
            }

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

            EventHandler.OnUpgraded?.Invoke(
                    _currentBuilding.typeConstruction,
                    _currentBuilding.GetCurrentGrade());
        }

        private void UpgradeSounded()
        {
            switch (_currentBuilding.typeConstruction)
            {
                case GlobalTypes.TypeBuildings.GagaHouse:
                    FMODUnity.RuntimeManager.PlayOneShot("");
                    break;
                case GlobalTypes.TypeBuildings.FluffCleaner:
                    FMODUnity.RuntimeManager.PlayOneShot("");
                    break;
                case GlobalTypes.TypeBuildings.ClothMachine:
                    FMODUnity.RuntimeManager.PlayOneShot("");
                    break;
                case GlobalTypes.TypeBuildings.Storage:
                    FMODUnity.RuntimeManager.PlayOneShot("");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CheckBtns()
        {
            if (!IsBuilded)
            {
                _buildBtn.SetActive(true);
                _upgradeBtn.SetActive(false);
                _demolitionBtn.SetActive(false);
            }
            else
            {
                _buildBtn.SetActive(false);
                _upgradeBtn.SetActive(true);
                _demolitionBtn.SetActive(true);

                if (_flagBtn == null) return;

                _flagBtn.SetActive(HasFlag);
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
            if (!_inventory.TryGetBunch(GlobalConstants.Money, out var moneyBunch)) return false;

            if (moneyBunch.GetCount() < price)
                return false;
            moneyBunch.RemoveItems(price);
            EventHandler.OnBunchChanged.Invoke(GlobalConstants.Money, moneyBunch.GetCount());
            return true;
        }

        private void Sell(int price)
        {
            _inventory.TryGetBunch(GlobalConstants.Money, out var moneyBunch);

            moneyBunch.AddItems(price);
            EventHandler.OnBunchChanged.Invoke(GlobalConstants.Money, moneyBunch.GetCount());
        }

        public void SetInventory(Inventory inv) => _inventory = inv;
    }
}