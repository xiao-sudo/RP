using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    [CreateAssetMenu(menuName = "Rendering/CreateRPPipeline")]
    public class RPRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return null;
        }
    }
}