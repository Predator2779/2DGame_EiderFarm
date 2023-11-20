using Building.Constructions;
using UnityEngine;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        public Construction buildingPrefab;
        
        private Construction _construction;
        private SpriteRenderer _triggerSprite;
        private Vector3 _buildPos;
        
        public void SetPosition(SpriteRenderer triggerSprite, Vector3 buildPos)
        {
            _triggerSprite = triggerSprite;
            _buildPos = buildPos;
        }

        public void Build()
        {
            if (_construction != null) return;
            
            _triggerSprite.enabled = false;
            Build(buildingPrefab);
            _construction.SetSprite(_construction.Upgrade());
        }

        public void Demolition()
        {
            if (_construction == null) return;

            _triggerSprite.enabled = true;
            Destroy(_construction.gameObject);
        }

        public void Upgrade()
        {
            if (_construction == null || !_construction.CanUpgrade()) return;

            _construction.SetSprite(_construction.Upgrade());
        }
        
        private void Build(Construction building)
        {
            if (_construction != null) Destroy(_construction);

            _construction = Instantiate(building, _buildPos, Quaternion.identity);
        }
    }
}