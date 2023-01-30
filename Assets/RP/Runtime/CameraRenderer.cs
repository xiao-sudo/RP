using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public partial class CameraRenderer
    {
        private const string kBufferName = "RP Render Camera";
        private static readonly ShaderTagId UNLIT_SHADER_TAG_ID = new ShaderTagId("SRPDefaultUnlit");

        private ScriptableRenderContext m_Context;
        private Camera m_Camera;
        private CullingResults m_CullingResults;

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

            Setup();

            DrawOpaque(use_gpu_instancing, use_dynamic_batching);

            DrawSky();

            DrawTransparent(use_gpu_instancing, use_dynamic_batching);

            DrawUnsupportedShaders();

            DrawGizmos();

            Submit();
        }

        private void Setup()
        {
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

        private void DrawOpaque(bool use_gpu_instancing, bool use_dynamic_batching)
        {
            var drawing_settings = new DrawingSettings(UNLIT_SHADER_TAG_ID,
                new SortingSettings(m_Camera) { criteria = SortingCriteria.CommonOpaque })
            {
                enableInstancing = use_gpu_instancing,
                enableDynamicBatching = use_dynamic_batching
            };

            var filtering_settings = new FilteringSettings(RenderQueueRange.opaque);

            m_Context.DrawRenderers(m_CullingResults, ref drawing_settings, ref filtering_settings);
        }

        private void DrawSky()
        {
            m_Context.DrawSkybox(m_Camera);
        }

        private void DrawTransparent(bool use_gpu_instancing, bool use_dynamic_batching)
        {
            var drawing_settings = new DrawingSettings(UNLIT_SHADER_TAG_ID,
                new SortingSettings(m_Camera) { criteria = SortingCriteria.CommonTransparent })
            {
                enableInstancing = use_gpu_instancing,
                enableDynamicBatching = use_dynamic_batching
            };

            var filtering_settings = new FilteringSettings(RenderQueueRange.transparent);

            m_Context.DrawRenderers(m_CullingResults, ref drawing_settings, ref filtering_settings);
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