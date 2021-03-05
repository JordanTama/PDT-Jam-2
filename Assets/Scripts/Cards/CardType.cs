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

        public string MaterialDirectory => Application.dataPath + "Assets/Materials/Cards/" + name + "/";
        
        private void OnEnable()
        {
            description = "Write a description here...";
            System.IO.Directory.CreateDirectory(MaterialDirectory);
        }

        private void OnDestroy()
        {
            System.IO.Directory.Delete(MaterialDirectory, true);
        }

        public void AddLayer()
        {
            CardLayer layer = new CardLayer(this, "Layer " + (layers.Count + 1));
            layers.Add(layer);
        }

        public void RemoveLayer(int index)
        {
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
        private class Content
        {
            [SerializeField] private Material material;
            [SerializeField] private string name;

            [SerializeField] private string path;

            public void Initialize(CardType card, int index)
            {
                material = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/Shaders/Card/Content.shader"));
                path = card.MaterialDirectory + index;
                AssetDatabase.CreateAsset(material, path);
            }

            public void Destroy()
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
    }
}