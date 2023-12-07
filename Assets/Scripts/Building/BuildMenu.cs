using Building.Constructions;
using Economy;
using General;
using System;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {

        private Construction _buildingPrefab;
        private Construction _curConstruction;

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
            if (_curConstruction != null) return;

            if (!Buy(_buyPrice))
                return;

            if (_triggerSprite != null)
                _triggerSprite.enabled = false;


            Build(_buildingPrefab);
            _curConstruction.SetSprite(_curConstruction.Upgrade());
            IsBuilded = true;

            CheckBtns();

            EventHandler.OnBuilded?.Invoke(_curConstruction.typeConstruction);
        }

        public void Demolition()
        {
            if (_curConstruction == null) return;

            _triggerSprite.enabled = true;
            Sell(_sellPrice);
            IsBuilded = false;

            EventHandler.OnDemolition?.Invoke(_curConstruction.typeConstruction);
            Destroy(_curConstruction.gameObject);
            
            CheckBtns();

            if (_flag != null) _flag.RemoveFlag();

        }

        public void Upgrade()
        {
            if (_curConstruction == null || !_curConstruction.CanUpgrade()) return;

            switch (_curConstruction.GetCurrentGrade())
            {
                case 1:
                    if (!Buy(_upgradePrice[0])) return; break;
                case 2:
                    if (!Buy(_upgradePrice[1])) return; break;
            }
            _curConstruction.SetSprite(_curConstruction.Upgrade());
            if (_curConstruction.GetComponent<ResourceTransmitter>() && _curConstruction.GetComponent<Machine>())
                _curConstruction.GetComponent<ResourceTransmitter>().SetGradeAnimationTrue(_curConstruction.GetCurrentGrade());

            EventHandler.OnUpgraded?.Invoke(
                    _curConstruction.typeConstruction,
                    _curConstruction.GetCurrentGrade());
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

        public Construction GetBuilding() => _curConstruction;

        private void Build(Construction building)
        {
            if (_curConstruction != null) Destroy(_curConstruction);
            _curConstruction = Instantiate(building, _buildPos, _buildRot, _parent);
        }

        public void SetFlag() => _flag.AddFlag();

        private bool Buy(int price)
        {
            _inventory.TryGetBunch(GlobalConstants.Money, out var moneyBunch);
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