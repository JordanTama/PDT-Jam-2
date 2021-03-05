using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardMaterialController : MonoBehaviour
    {
        private readonly List<Material> _materials = new List<Material>();

        private StencilService _service;
        private int _stencilRef;
        
        private static readonly int StencilRefId = Shader.PropertyToID("_StencilRef");

        

        private int StencilRef
        {
            get => _stencilRef;
            set
            {
                _stencilRef = value;
                UpdateStencilRefs();
            }
        }
        

        private void Start()
        {
            _service = ServiceLocator.ServiceLocator.Get<StencilService>();
            UpdateMaterialList();

            StencilRef = _service.GetStencilRef();
        }


        private void UpdateStencilRefs()
        {
            foreach (var mat in _materials.Where(mat => mat.HasProperty(StencilRefId)))
                mat.SetInt(StencilRefId, StencilRef);
        }

        private void UpdateMaterialList()
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