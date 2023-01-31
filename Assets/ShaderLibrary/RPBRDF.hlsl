#ifndef RP_BRDF_INCLUDED
#define RP_BRDF_INCLUDED

struct BRDF
{
    float3 diffuse;
    float3 specular;
    float roughness;
};

#define MIN_REFLECTIVITY 0.04

float OneMinusReflectivity(float metallic)
{
    const float range = 1.0 - MIN_REFLECTIVITY;
    return range - metallic * range;
}

BRDF GetBRDF(Surface surface)
{
    BRDF brdf;

    const float one_minus_reflectivity = OneMinusReflectivity(surface.metallic);

    brdf.diffuse = surface.color * one_minus_reflectivity;
    brdf.specular = lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);
    brdf.roughness = PerceptualRoughnessToRoughness(PerceptualSmoothnessToRoughness(surface.smoothness));

    return brdf;
}

#endif
