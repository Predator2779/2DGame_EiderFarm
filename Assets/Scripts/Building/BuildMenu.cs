using Building.Constructions;
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

            if (_triggerSprite != null)
                _triggerSprite.enabled = false;

            Build(_buildingPrefab);
            _curConstruction.SetSprite(_curConstruction.Upgrade());
            IsBuilded = true;
            CheckBtns();

            EventHandler.OnBuilded?.Invoke(
                    _curConstruction.typeConstruction);
        }

        public void Demolition()
        {
            if (_curConstruction == null) return;

            _triggerSprite.enabled = true;
            Destroy(_curConstruction.gameObject);
            IsBuilded = false;
            if (_flag != null)
                _flag.RemoveFlag();
                CheckBtns();
        }

        public void Upgrade()
        {
            if (_curConstruction == null || !_curConstruction.CanUpgrade()) return;

            _curConstruction.SetSprite(_curConstruction.Upgrade());

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

                if (HasFlag && _flagBtn != null)
                    _flagBtn.SetActive(true);
                else if (!HasFlag && _flagBtn != null) _flagBtn.SetActive(false);
            }
        }

        public Construction GetConstruction() => _curConstruction;

        private void Build(Construction building)
        {
            if (_curConstruction != null) Destroy(_curConstruction);

            _curConstruction = Instantiate(building, _buildPos, _buildRot, _parent);
        }


    }
}