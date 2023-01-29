﻿#ifndef RP_COMMON_INCLUDED
#define RP_COMMON_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "RPInput.hlsl"

#define UNITY_MATRIX_M unity_ObjectToWorld
#define UNITY_MATRIX_I_M unity_WorldToObjet
#define UNITY_MATRIX_V unity_MatrixV
#define UNITY_MATRIX_P glstate_matrix_projection
#define UNITY_MATRIX_VP unity_MatrixVP
#define UNITY_PREV_MATRIX_M   unity_MatrixPreviousM
#define UNITY_PREV_MATRIX_I_M unity_MatrixPreviousMI

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

#endif
