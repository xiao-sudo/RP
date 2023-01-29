using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public class RPRenderPipeline : RenderPipeline
    {
        private readonly CameraRenderer m_CameraRenderer = new CameraRenderer();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
                m_CameraRenderer.Render(context, camera);
        }
    }
}