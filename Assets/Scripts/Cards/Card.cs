using System;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public Content[] content;

    [Serializable]
    public struct Content
    {
        public Material material;
        public float depth;
    }
}
