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

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            m_Context = context;
            m_Camera = camera;

            if (!Cull())
                return;

            Setup();

            DrawOpaque();

            DrawSky();

            DrawTransparent();
            
            DrawUnsupportedShaders();

            Submit();
        }

        private void Setup()
        {
            // set rt and clear
            m_CommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            m_CommandBuffer.ClearRenderTarget(true, true, Color.clear);

            m_CommandBuffer.BeginSample(kBufferName);
            
            ExecuteBuffer();
            m_Context.SetupCameraProperties(m_Camera);
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
            var drawing_settings = new DrawingSettings(UNLIT_SHADER_TAG_ID, new SortingSettings(m_Camera));
            var filtering_settings = new FilteringSettings(RenderQueueRange.opaque);

            m_Context.DrawRenderers(m_CullingResults, ref drawing_settings, ref filtering_settings);
        }

        private void DrawSky()
        {
            m_Context.DrawSkybox(m_Camera);
        }

        private void DrawTransparent()
        {
            var sorting_settings = new SortingSettings
            {
                criteria = SortingCriteria.CommonTransparent
            };
            
            var drawing_settings = new DrawingSettings(UNLIT_SHADER_TAG_ID, sorting_settings);
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