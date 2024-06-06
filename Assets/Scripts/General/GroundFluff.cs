using System;
using System.Collections;
using Economy;
using Other;
using UnityEngine;

namespace General
{
    public class GroundFluff : MonoBehaviour
    {
        [SerializeField] private BubbleWrap _bubbles;
        [SerializeField] private Item _item;
        [SerializeField] private float _lifeTime;

        private SpriteRenderer _renderer;
        
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            StartCoroutine(RemoveItem(_lifeTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Inventory inventory) && inventory.IsPlayerInventory())
            {
                inventory.AddItems(_item, 1);
                _renderer.enabled = false;
                _bubbles.StartBubble();
                StopCoroutine(RemoveItem(1));
                StartCoroutine(RemoveItem(1));
            }
        }

        private IEnumerator RemoveItem(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}