using Building.Constructions;
using UnityEngine;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        private Construction _buildingPrefab;
        private Construction _curConstruction;
        private SpriteRenderer _triggerSprite;
        private Vector3 _buildPos;
        private Quaternion _buildRot;

        public void SetConstruction(
                Construction prefab,
                SpriteRenderer triggerSprite, 
                Vector3 buildPos, 
                Quaternion buildRot)
        {
            _buildingPrefab = prefab;
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
        }

        public void Demolition()
        {
            if (_curConstruction == null) return;

            _triggerSprite.enabled = true;
            Destroy(_curConstruction.gameObject);
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

            _curConstruction = Instantiate(building, _buildPos, _buildRot);
        }
    }
}