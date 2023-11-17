using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Building
{
    public class BuildTrigger : MonoBehaviour
    {
        [SerializeField] private BuildMenu _menu;

        private Tilemap _map;
        private SpriteRenderer _renderer;
        
        private void Start()
        {
            _map = transform.parent.GetComponent<Tilemap>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void SetMenuActive(bool value) => _menu.gameObject.SetActive(value);

        private Vector3 GetTilePos() => _map.CellToWorld(_map.WorldToCell(transform.position));

        private void DisableMenu()
        {
            SetMenuActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            SetMenuActive(true);
            _menu.SetPosition(_renderer, GetTilePos());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) SetMenuActive(false);
        }
    }
}