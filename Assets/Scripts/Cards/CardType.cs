using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cards
{
    public class CardType : ScriptableObject
    {
        [SerializeField] private string description;
        [SerializeField] private List<CardLayer> layers = new List<CardLayer>();
        
        private string MaterialDirectory => "Assets/Materials/Cards/" + name + "/";
        
        private void OnEnable()
        {
            System.IO.Directory.CreateDirectory("Assets/Materials/Cards/" + name);
        }

        private void OnDestroy()
        {
            foreach (CardLayer layer in layers)
                layer.Destroy();
        }

        
        public void AddLayer()
        {
            CardLayer layer = new CardLayer(this, "Layer " + (layers.Count + 1));
            layers.Add(layer);
        }

        public void RemoveLayer(int index)
        {
            layers[index].Destroy();
            layers.RemoveAt(index);
        }

        public void AddContent(int layerIndex)
        {
            layers[layerIndex].AddContent();
        }

        public void RemoveContent(int layerIndex, int contentIndex)
        {
            layers[layerIndex].RemoveContent(contentIndex);
        }

        [Serializable]
        public class CardLayer
        {
            [SerializeField] private CardType card;
            
            [SerializeField] public string name;
            [SerializeField] private List<Content> contents = new List<Content>();
            
            public CardLayer(CardType card, string name)
            {
                this.name = name;
                this.card = card;
            }

            public void Destroy()
            {
                foreach (Content content in contents)
                    content.Destroy();
            }

            public void AddContent()
            {
                Content content = new Content();
                content.Initialize(card, contents.Count);
                contents.Add(content);
            }

            public void RemoveContent(int index)
            {
                contents[index].Destroy();
                contents.RemoveAt(index);
            }
        }
        

        [Serializable]
        public class Content
        {
            [SerializeField] private Material material;
            [SerializeField] private string name;

            [SerializeField] private string path;

            public string Name => name;
            public string Directory => path;
            
            public void Initialize(CardType card, int index)
            {
                material = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/Card/Content.shader"));
                name = material.GetHashCode().ToString();
                path = card.MaterialDirectory + "/" + name + ".mat";
                AssetDatabase.CreateAsset(material, path);
            }

            public void Destroy()
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
    }
}