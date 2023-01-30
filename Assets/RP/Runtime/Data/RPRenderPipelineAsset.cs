using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime.Data
{
    [CreateAssetMenu(menuName = "Rendering/CreateRPPipeline")]
    public class RPRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private bool m_UseSrpBatcher = true, m_UseGPUInstancing = true, m_UseDynamicBatching = false;

        protected override RenderPipeline CreatePipeline()
        {
            return new RPRenderPipeline(m_UseSrpBatcher, m_UseGPUInstancing, m_UseDynamicBatching);
        }
    }
}