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

        public void SetPosition(SpriteRenderer triggerPlace, Vector3 buildPos)
        {
            _triggerPlace = triggerPlace;
            _buildPos = buildPos;
        }

        public void Build()
        {
            if (_construction.GetData().isBuilded) return;

            _construction.GetData().isBuilded = true;
            _triggerPlace.enabled = false;
            Build(_construction.GetBuilding());
        }

        public void Demolition()
        {
            if (!_construction.GetData().isBuilded) return;

            _triggerPlace.enabled = true;
            _construction.GetData().isBuilded = false;
            _construction.GetData().currentGrade = 0;
            Destroy(_building);
        }

        public void Upgrade()
        {
            Build(_construction.GetBuilding());
        }

        private void Build(GameObject building)
        {
            if (_building != null) Destroy(_building);
            
            _building = Instantiate(building, _buildPos, Quaternion.identity);
        }
    }
}