using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardMaterialController : MonoBehaviour
    {
        [SerializeField] private Card card;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject contentPrefab;

        private readonly List<Material> _materials = new List<Material>();

        private StencilService _service;
        private int _stencilRef;

        private Collider _collider;
        
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

        public Card Card
        {
            get => card;
            set
            {
                card = value;
                ApplyCard();
            }
        }
        

        #region Unity Event Functions
        
        private void Start()
        {
            _service = ServiceLocator.ServiceLocator.Get<StencilService>();
            UpdateMaterialList();

            StencilRef = _service.GetStencilRef();

            if (!TryGetComponent(out _collider))
                Debug.Log("No Collider attached.");
        }

        #endregion
        
        
        #region Material Management

        private void ApplyCard()
        {
            foreach (Card.Content content in card.content)
            {
                GameObject newContent = Instantiate(contentPrefab, contentParent.position, contentParent.rotation,
                    contentParent);
                
                if (!newContent.TryGetComponent(out Renderer rend))
                {
                    DestroyImmediate(newContent);
                    continue;
                }
                
                rend.material = content.material;
                newContent.transform.Translate(transform.forward * content.depth, Space.World);
            }
            
            UpdateMaterialList();
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
        
        #endregion
        
        
        #region Collision Management

        public bool Intersects(Ray ray)
        {
            return _collider.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        }
        
        #endregion
    }
}