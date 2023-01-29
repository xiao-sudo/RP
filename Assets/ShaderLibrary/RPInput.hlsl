﻿#ifndef RP_INPUT_INCLUDED
#define RP_INPUT_INCLUDED

CBUFFER_START(UnityPerDraw)
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObjet;

real4 unity_WorldTransformParams;
float4 unity_LODFade;
CBUFFER_END

float4x4 unity_MatrixVP;
float4x4 unity_MatrixV;
float4x4 glstate_matrix_projection;

float4x4 unity_MatrixPreviousM;
float4x4 unity_MatrixPreviousMI;

#endif
