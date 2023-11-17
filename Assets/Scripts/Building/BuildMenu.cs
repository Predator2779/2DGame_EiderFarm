using Building.Constructions;
using UnityEngine;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _building;
        
        private Construction _construction;
        private SpriteRenderer _triggerPlace;
        private Vector3 _buildPos;
        
        public void SetPosition(SpriteRenderer triggerPlace, Vector3 buildPos)
        {
            _triggerPlace = triggerPlace;
            _buildPos = buildPos;
        }

        public void Build()
        {
            if (_construction != null) return;
            
            _triggerPlace.enabled = false;
            Build(_building);
            _construction.SetSprite(_construction.GetGrade());
        }

        public void Demolition()
        {
            if (_construction == null) return;

            _triggerPlace.enabled = true;
            Destroy(_construction.gameObject);
        }

        public void Upgrade()
        {
            if (_construction == null || !_construction.CanUpgrade()) return;

            _construction.SetSprite(_construction.GetGrade());
        }

        private void Build(GameObject building)
        {
            if (_construction != null) Destroy(_construction);

            _construction = Instantiate(building, _buildPos, Quaternion.identity).GetComponent<Construction>();
        }
    }
}