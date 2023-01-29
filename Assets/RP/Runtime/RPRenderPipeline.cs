using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public class RPRenderPipeline : RenderPipeline
    {
        private readonly CameraRenderer m_CameraRenderer = new CameraRenderer();

        public RPRenderPipeline()
        {
            GraphicsSettings.useScriptableRenderPipelineBatching = true;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
                m_CameraRenderer.Render(context, camera);
        }
    }
}