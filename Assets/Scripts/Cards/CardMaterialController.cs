using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardMaterialController : MonoBehaviour
    {
        private readonly List<Material> _materials = new List<Material>();

        private int _stencilRef = 0;
        private static readonly int StencilRefId = Shader.PropertyToID("_StencilRef");


        public int StencilRef
        {
            get => _stencilRef;
            set
            {
                _stencilRef = value;
                UpdateStencilRefs();
            }
        }
        

        private void OnEnable()
        {
            UpdateMaterialList();
            
            // TODO: Call to CardService to get a stencil value and assign to StencilRef...
            // StencilRef = 
        }


        private void UpdateStencilRefs()
        {
            foreach (Material mat in _materials)
            {
                if (mat.HasProperty(StencilRefId))
                    mat.SetInt(StencilRefId, StencilRef);
            }
        }

        public void UpdateMaterialList()
        {
            _materials.Clear();
            IterateChildrenRecursive(transform, _materials);
        }

        private static void IterateChildrenRecursive(Transform parent, List<Material> materials)
        {
            foreach (Transform child in parent)
                IterateChildrenRecursive(child, materials);

            if (parent.TryGetComponent(out Renderer renderer))
                materials.Add(renderer.material);
        }
    }
}