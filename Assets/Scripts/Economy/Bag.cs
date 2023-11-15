using System;
using UnityEngine;

namespace Economy
{
    [Serializable] public class Bag
    {
        public BagContent content { get; }
        [SerializeField] private int _points;

        public void AddPoints(int value) => _points += value > 0 ? value : 0;

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