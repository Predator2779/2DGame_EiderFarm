using System;
using UnityEngine;
using static Characters.AI.EmployeeAI;

namespace Other
{
    [Serializable] public class Thought
    {
        public Sprite sprite;
        public EmployeeStates state;
    }
}