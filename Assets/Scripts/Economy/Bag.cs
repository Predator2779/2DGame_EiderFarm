using System;
using UnityEngine;

namespace Economy
{
    public class Bag : MonoBehaviour
    {
        public enum BagContent
        {
            Money,
            CleanedFluff,
            UncleanedFluff,
            Clothes
        }

        public BagContent content;

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
}