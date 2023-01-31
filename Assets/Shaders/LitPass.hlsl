#ifndef RP_LIT_PASS_INCLUDED
#define  RP_LIT_PASS_INCLUDED

#include "../ShaderLibrary/RPCommon.hlsl"
#include "../ShaderLibrary/RPSurface.hlsl"
#include "../ShaderLibrary/RPLight.hlsl"
#include "../ShaderLibrary/RPLighting.hlsl"

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes
{
    float3 position_os : POSITION;
    float3 normal_os : NORMAL;
    float2 base_uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 position_cs : SV_POSITION;
    float3 normal_ws : VAR_NORMAL;
    float2 base_uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings LitPassVertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);

    const float3 position_ws = TransformObjectToWorld(input.position_os);
    output.position_cs = TransformWorldToHClip(position_ws);

    float4 base_st = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
    output.base_uv = input.base_uv * base_st.xy + base_st.zw;

    output.normal_ws = TransformObjectToWorldNormal(input.normal_os);

    return output;
}

half4 LitPassFragment(Varyings input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input);
    const float4 base_map = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.base_uv);
    const float4 base_color = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);

    float4 base = base_map * base_color;

    #if defined(_CLIPPING)
    clip(final_color.w - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
    #endif

    Surface surface;
    surface.normal = normalize(input.normal_ws);
    surface.color = base.xyz;
    surface.alpha = base.w;

    float3 color = GetLighting(surface);
    return float4(color, surface.alpha);
}

#endif
