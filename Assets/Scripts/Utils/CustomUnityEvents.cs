using System;
using Gameplay;
using UnityEngine.Events;

namespace Utils
{
    [Serializable]
    public class StringUnityEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class FloatUnityEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class CellUnityEvent : UnityEvent<Cell>
    {
    }
}