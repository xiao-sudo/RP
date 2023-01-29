#ifndef RP_UNLIT_PASS_INCLUDED
#define RP_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/RPCommon.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
CBUFFER_END

float4 UnlitPassVertex(float3 position_os : POSITION) : SV_POSITION
{
    const float3 position_ws = TransformObjectToWorld(position_os);
    return TransformWorldToHClip(position_ws);
}

half4 UnlitPassFragment() : SV_TARGET
{
    return _BaseColor;
}

#endif
