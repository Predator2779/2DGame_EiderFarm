using Building.Constructions;
using UnityEngine;

namespace Building
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField] private Construction _construction;
        [SerializeField] private Field _field;

        [SerializeField] private GameObject _prefabGaga;



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
            if (_construction.isBuilded) return;

            _construction.isBuilded = true;
            _triggerPlace.enabled = false;
            Build(_construction.GetBuilding());

            if (_prefabGaga != null)
                Instantiate(_prefabGaga, _field.GetRandomSpawnPlace().transform.position, Quaternion.identity).GetComponent<Gaga>().Initialize(this.gameObject, _field.GetRandomSpawnPlace());

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

            Build(_construction.GetBuilding());
        }

        private void Build(GameObject building)
        {
            if (_building != null) Destroy(_building);

            _building = Instantiate(building, _buildPos, Quaternion.identity);
        }
    }
}