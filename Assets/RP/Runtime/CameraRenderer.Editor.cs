using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RP.Runtime
{
    public partial class CameraRenderer
    {

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

        [Conditional("UNITY_EDITOR")]
        void DrawUnsupportedShaders()
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

        [Conditional("UNITY_EDITOR")]
        void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                m_Context.DrawGizmos(m_Camera, GizmoSubset.PreImageEffects);
                m_Context.DrawGizmos(m_Camera, GizmoSubset.PostImageEffects);
            }
        }

        [Conditional("UNITY_EDITOR")]
        void PrepareForSceneWindow()
        {
            if (CameraType.SceneView == m_Camera.cameraType)
                ScriptableRenderContext.EmitWorldGeometryForSceneView(m_Camera);
        }

        [Conditional("UNITY_EDITOR")]
        void PrepareBuffer()
        {
            Profiler.BeginSample("Editor Only");
            m_CommandBuffer.name = SampleName = m_Camera.name;
            Profiler.EndSample();
        }

        private string SampleName { get; set; }
#else
        const string SampleName = kBufferName;
#endif
    }
}