using System;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Economy
{
    [Serializable] public class Bag
    {
        public BagContent content;
        [SerializeField] private int _points;
        [SerializeField] private bool _isPlayerBag;

        public void AddPoints(int value)
        {
            _points += value > 0 ? value : 0;
            
            if (_isPlayerBag) EventHandler.OnBagAdd?.Invoke(content, _points);
        }

        private void RemovePoints(int value) => AddPoints(-value);

        public void Give(Bag bag, int value)
        {
            if (!IsCorrect(bag)) return;

            bag.AddPoints(GetPoints(_points, value));
            RemovePoints(value);
        }

        private bool IsCorrect(Bag bag) => bag.content == content;

        private int GetPoints(int points, int value) => points >= value ? value : 0;
    }

    public enum BagContent
    {
        Money,
        CleanedFluff,
        UncleanedFluff,
        Clothes
    }
}