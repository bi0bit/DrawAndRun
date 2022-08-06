using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Counter", menuName = "Assets/Counter", order = 0)]
    public class Counter : ScriptableObject
    {
        public uint Count { get; private set; }

        public event Action<uint> EventUpdateCount;

        public void Add()
        {
            Count += 1;
            EventUpdateCount.Invoke(Count);
        }
    }
}