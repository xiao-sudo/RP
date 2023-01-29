using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public class CameraRenderer
    {
        private const string kBufferName = "RP Render Camera";

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

            DrawSky();

            Submit();
        }

        private void Setup()
        {
            m_CommandBuffer.BeginSample(kBufferName);

            // set rt and clear
            m_CommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
            m_CommandBuffer.ClearRenderTarget(true, true, Color.clear);

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

        private void DrawSky()
        {
            m_Context.DrawSkybox(m_Camera);
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