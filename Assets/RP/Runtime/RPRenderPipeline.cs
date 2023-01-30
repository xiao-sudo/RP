using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public class RPRenderPipeline : RenderPipeline
    {
        private readonly CameraRenderer m_CameraRenderer = new CameraRenderer();

        private readonly bool m_UseGpuInstancing;
        private readonly bool m_UseDynamicBatching;

        public RPRenderPipeline(bool use_srp_batcher, bool use_gpu_instancing, bool use_dynamic_batching)
        {
            m_UseGpuInstancing = use_gpu_instancing;
            m_UseDynamicBatching = use_dynamic_batching;

            GraphicsSettings.useScriptableRenderPipelineBatching = use_srp_batcher;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
                m_CameraRenderer.Render(context, camera, m_UseGpuInstancing, m_UseDynamicBatching);
        }
    }
}