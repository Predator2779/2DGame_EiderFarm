using General;
using UnityEngine;

namespace Economy.Farm_House
{
    [CreateAssetMenu(menuName = "Tasks/CollectTasks/ItemTask", fileName = "New ItemTask", order = 0)]
    public class ItemTask : CollectTask
    {
        [SerializeField] protected Item _requiredItem;
        
        protected override void Initialize()
        {
            EventHandler.OnItemPickUp.AddListener(PickUpItem);
            EventHandler.OnItemPut.AddListener(PutItem);
        }

        protected override void Deinitialize()
        {
            EventHandler.OnItemPickUp.RemoveListener(PickUpItem);
            EventHandler.OnItemPut.RemoveListener(PutItem);
        }
        
        private void PickUpItem(Item item, int count)
        {
            if (_requiredItem == item) 
                AddCount(count);
            
            ProgressingTask();
        }

        private void PutItem(Item item, int count)
        {
            if (_requiredItem == item)
                RemoveCount(count);
            
            ProgressingTask();
        }
    }
}