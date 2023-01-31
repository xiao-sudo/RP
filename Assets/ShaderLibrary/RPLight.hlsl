#ifndef RP_LIGHT_INCLUDED
#define RP_LIGHT_INCLUDED

struct Light
{
    float3 color;
    float3 direction;
};

Light GetDirectionalLight()
{
    Light light;
    
    light.color = 1.0;
    light.direction = float3(0, 1, 0);

    return light;
}

#endif
