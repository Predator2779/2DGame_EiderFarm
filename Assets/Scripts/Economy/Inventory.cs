using UnityEngine;

namespace Economy
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Bag[] _bags;

        public Bag[] GetAllBags() => _bags;

        public bool TryGetBag(BagContent content, out Bag relevantBag)
        {
            Bag[] bags = GetAllBags();
            
            foreach (var bag in bags)
                if (bag.content == content)
                {
                    relevantBag = bag;
                    return true;
                }

            relevantBag = null;
            return false;
        }
    }
}