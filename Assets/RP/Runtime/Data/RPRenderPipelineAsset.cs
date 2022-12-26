using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime.Data
{
    [CreateAssetMenu(menuName = "Rendering/CreateRPPipeline")]
    public class RPRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new RPRenderPipeline();
        }
    }
}