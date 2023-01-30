using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public partial class CameraRenderer
    {
        private const string kBufferName = "RP Render Camera";
        private static readonly ShaderTagId UNLIT_SHADER_TAG_ID = new ShaderTagId("SRPDefaultUnlit");
        private static readonly ShaderTagId LIT_SHADER_TAG_ID = new ShaderTagId("RPLit");

        private ScriptableRenderContext m_Context;
        private Camera m_Camera;
        private CullingResults m_CullingResults;
        private DrawingSettings m_DrawingSettings;
        private SortingSettings m_SortingSettings;

        private readonly CommandBuffer m_CommandBuffer = new CommandBuffer()
        {
            name = kBufferName
        };

        public void Render(ScriptableRenderContext context, Camera camera, bool use_gpu_instancing,
            bool use_dynamic_batching)
        {
            m_Context = context;
            m_Camera = camera;

            PrepareBuffer();

            PrepareForSceneWindow();

            if (!Cull())
                return;

            Setup(use_gpu_instancing, use_dynamic_batching);

            DrawOpaque();

            DrawSky();

            DrawTransparent();

            DrawUnsupportedShaders();

            DrawGizmos();

            Submit();
        }

        private void Setup(bool use_gpu_instancing, bool use_dynamic_batching)
        {
            m_SortingSettings = new SortingSettings(m_Camera);

            m_DrawingSettings = new DrawingSettings(UNLIT_SHADER_TAG_ID, m_SortingSettings)
            {
                enableInstancing = use_gpu_instancing,
                enableDynamicBatching = use_dynamic_batching
            };

            m_DrawingSettings.SetShaderPassName(1, LIT_SHADER_TAG_ID);

            // Main Camera RT Clear, Second Camera GL Draw Clear
            m_Context.SetupCameraProperties(m_Camera);

            var clear_flags = m_Camera.clearFlags;
            var clear_depth = clear_flags <= CameraClearFlags.Depth;
            var clear_color = clear_flags == CameraClearFlags.Color;

            m_CommandBuffer.ClearRenderTarget(clear_depth, clear_color,
                clear_color ? m_Camera.backgroundColor.linear : Color.clear);

            m_CommandBuffer.BeginSample(kBufferName);

            ExecuteBuffer();
        }

        private bool Cull()
        {
            if (m_Camera.TryGetCullingParameters(out var p))
            {
                m_CullingResults = m_Context.Cull(ref p);
                return true;
            }

            return false;
        }

        private void DrawOpaque()
        {
            m_SortingSettings.criteria = SortingCriteria.CommonOpaque;
            m_DrawingSettings.sortingSettings = m_SortingSettings;

            var filtering_settings = new FilteringSettings(RenderQueueRange.opaque);

            m_Context.DrawRenderers(m_CullingResults, ref m_DrawingSettings, ref filtering_settings);
        }

        private void DrawSky()
        {
            m_Context.DrawSkybox(m_Camera);
        }

        private void DrawTransparent()
        {
            m_SortingSettings.criteria = SortingCriteria.CommonTransparent;
            m_DrawingSettings.sortingSettings = m_SortingSettings;

            var filtering_settings = new FilteringSettings(RenderQueueRange.transparent);

            m_Context.DrawRenderers(m_CullingResults, ref m_DrawingSettings, ref filtering_settings);
        }


        private void ExecuteBuffer()
        {
            m_Context.ExecuteCommandBuffer(m_CommandBuffer);
            m_CommandBuffer.Clear();
        }

        private void Submit()
        {
            m_CommandBuffer.EndSample(kBufferName);
            ExecuteBuffer();
            m_Context.Submit();
        }
    }
}