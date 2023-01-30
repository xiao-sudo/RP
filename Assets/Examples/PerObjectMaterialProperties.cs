using UnityEngine;

namespace Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static readonly int BASE_COLOR_ID = Shader.PropertyToID("_BaseColor");
        private static readonly int CUTOFF_ID = Shader.PropertyToID("_Cutoff");

        private static MaterialPropertyBlock MATERIAL_PROPERTY_BLOCK;

        [SerializeField]
        private Color m_BaseColor = Color.white;

        [SerializeField, Range(0, 1f)]
        private float m_Cutoff = 0.5f;

        private void OnValidate()
        {
            var renderer_instance = GetComponent<Renderer>();

            if (renderer_instance)
            {
                if (null == MATERIAL_PROPERTY_BLOCK)
                    MATERIAL_PROPERTY_BLOCK = new MaterialPropertyBlock();

                MATERIAL_PROPERTY_BLOCK.SetColor(BASE_COLOR_ID, m_BaseColor);
                MATERIAL_PROPERTY_BLOCK.SetFloat(CUTOFF_ID, m_Cutoff);

                renderer_instance.SetPropertyBlock(MATERIAL_PROPERTY_BLOCK);
            }
        }
    }
}