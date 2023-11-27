using Building.Constructions;
using System;
using UnityEngine;

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

        public void Build()
        {
            if (_curConstruction != null) return;

            if (_triggerSprite != null)
                _triggerSprite.enabled = false;
            
            Build(_buildingPrefab);
            _curConstruction.SetSprite(_curConstruction.Upgrade());
            IsBuilded = true;
        }

        public void Demolition()
        {
            if (_curConstruction == null) return;

            _triggerSprite.enabled = true;
            Destroy(_curConstruction.gameObject);
            IsBuilded = false;
        }

        public void Upgrade()
        {
            if (_curConstruction == null || !_curConstruction.CanUpgrade()) return;

            _curConstruction.SetSprite(_curConstruction.Upgrade());
        }

        public Construction GetConstruction() => _curConstruction;

        private void Build(Construction building)
        {
            if (_curConstruction != null) Destroy(_curConstruction);

            _curConstruction = Instantiate(building, _buildPos, _buildRot, _parent);
        }
    }
}