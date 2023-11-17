using Building.Constructions;
using UnityEngine;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private Construction _construction;
        
        private GameObject _building;
        private SpriteRenderer _triggerPlace;
        private Vector3 _buildPos;

        private void Start() => _construction.Reset();
        public void SetPosition(SpriteRenderer triggerPlace, Vector3 buildPos)
        {
            _triggerPlace = triggerPlace;
            _buildPos = buildPos;
        }

        public void Build()
        {
            if (_construction.isBuilded) return;
            
            _construction.isBuilded = true;
            _triggerPlace.enabled = false;
            Build(_construction.gameObject);
            
            _building.GetComponent<SpriteRenderer>().sprite = _construction.GetGrade();
        }

        public void Demolition()
        {
            if (!_construction.isBuilded) return;

            _triggerPlace.enabled = true;
            _construction.isBuilded = false;
            _construction.Reset();
            
            Destroy(_building);
        }

        public void Upgrade()
        {
            if (!_construction.isBuilded) return;

            _building.GetComponent<SpriteRenderer>().sprite = _construction.GetGrade();
        }

        private void Build(GameObject building)
        {
            if (_building != null) Destroy(_building);

            _building = Instantiate(building.gameObject, _buildPos, Quaternion.identity);
        }
    }
}