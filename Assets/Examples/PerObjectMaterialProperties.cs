using UnityEngine;

namespace Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static readonly int BASE_COLOR_ID = Shader.PropertyToID("_BaseColor");
        private static MaterialPropertyBlock MATERIAL_PROPERTY_BLOCK;

        [SerializeField]
        private Color m_BaseColor = Color.white;

        private void OnValidate()
        {
            var renderer_instance = GetComponent<Renderer>();

            if (renderer_instance)
            {
                if (null == MATERIAL_PROPERTY_BLOCK)
                    MATERIAL_PROPERTY_BLOCK = new MaterialPropertyBlock();

                MATERIAL_PROPERTY_BLOCK.SetColor(BASE_COLOR_ID, m_BaseColor);
                renderer_instance.SetPropertyBlock(MATERIAL_PROPERTY_BLOCK);
            }
        }
    }
}