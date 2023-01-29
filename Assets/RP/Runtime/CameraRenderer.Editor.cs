using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public partial class CameraRenderer
    {
        partial void DrawUnsupportedShaders();
        
#if UNITY_EDITOR
        private static readonly ShaderTagId[] LEGACY_SHADER_TAG_IDS =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        private static readonly Material ERROR_MATERIAL = new Material(Shader.Find("Hidden/InternalErrorShader"));

        partial void DrawUnsupportedShaders()
        {
            var drawing_settings = new DrawingSettings(LEGACY_SHADER_TAG_IDS[0], new SortingSettings(m_Camera))
            {
                overrideMaterial = ERROR_MATERIAL
            };

            for (int w = 0; w < LEGACY_SHADER_TAG_IDS.Length; w++)
                drawing_settings.SetShaderPassName(w, LEGACY_SHADER_TAG_IDS[w]);

            var filtering_settings = FilteringSettings.defaultValue;
            m_Context.DrawRenderers(m_CullingResults, ref drawing_settings, ref filtering_settings);
        }
#endif
    }
}