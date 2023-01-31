using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace RP.Runtime
{
    public class Lighting
    {
        private const string kBufferName = "Lighting";
        private const int kMaxDirLightCount = 4;

        private static readonly int DIR_LIGHT_COUNT_ID = Shader.PropertyToID("_DirectionalLightCount");
        private static readonly int DIR_LIGHT_COLOR_IDS = Shader.PropertyToID("_DirectionalLightColors");
        private static readonly int DIR_LIGHT_DIRECTION_IDS = Shader.PropertyToID("_DirectionalLightDirections");

        private static readonly Vector4[] DIR_LIGHT_COLORS = new Vector4[kMaxDirLightCount];
        private static readonly Vector4[] DIR_LIGHT_DIRECTIONS = new Vector4[kMaxDirLightCount];

        private readonly CommandBuffer m_CommandBuffer = new CommandBuffer() { name = kBufferName };

        private CullingResults m_CullingResults;

        public void Setup(ScriptableRenderContext context, CullingResults culling_results)
        {
            m_CullingResults = culling_results;

            m_CommandBuffer.BeginSample(kBufferName);
            SetupLights();
            m_CommandBuffer.EndSample(kBufferName);

            context.ExecuteCommandBuffer(m_CommandBuffer);
            m_CommandBuffer.Clear();
        }

        void SetupLights()
        {
            NativeArray<VisibleLight> visible_lights = m_CullingResults.visibleLights;

            var dir_light_count = 0;

            for (var i = 0; i < visible_lights.Length; ++i)
            {
                var visible_light = visible_lights[i];
                if (LightType.Directional == visible_light.lightType)
                {
                    SetupDirectionalLight(dir_light_count++, ref visible_light);

                    if (dir_light_count >= kMaxDirLightCount)
                        break;
                }
            }

            m_CommandBuffer.SetGlobalInt(DIR_LIGHT_COUNT_ID, visible_lights.Length);
            m_CommandBuffer.SetGlobalVectorArray(DIR_LIGHT_COLOR_IDS, DIR_LIGHT_COLORS);
            m_CommandBuffer.SetGlobalVectorArray(DIR_LIGHT_DIRECTION_IDS, DIR_LIGHT_DIRECTIONS);
        }

        void SetupDirectionalLight(int index, ref VisibleLight visible_light)
        {
            DIR_LIGHT_COLORS[index] = visible_light.finalColor;
            DIR_LIGHT_DIRECTIONS[index] = -visible_light.localToWorldMatrix.GetColumn(2);
        }
    }
}