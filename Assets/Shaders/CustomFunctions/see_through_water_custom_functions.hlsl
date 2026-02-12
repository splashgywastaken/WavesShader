#ifndef SEE_THROUGH_WATER_CUSTOM_FUNCTIONS
#define SEE_THROUGH_WATER_CUSTOM_FUNCTIONS

// TEXTURE2D_FLOAT(_CameraDepthTexture);
// float4 _CameraDepthTexture_TexelSize;
// SAMPLER(sampler_CameraDepthTexture);
// TEXTURE2D_FLOAT(_GrabPassTransparent);
// SAMPLER(sampler_GrabPassTransparent);
//
// float LinearEyeDepth( float rawdepth )
// {
//     float x, y, z, w;
//     #if SHADER_API_GLES3 // instead of UNITY_REVERSED_Z
//     x = -1.0 + _ProjectionParams.y/ _ProjectionParams.z;
//     y = 1;
//     z = x / _ProjectionParams.y;
//     w = 1 / _ProjectionParams.y;
//     #else
//     x = 1.0 - _ProjectionParams.y/ _ProjectionParams.z;
//     y = _ProjectionParams.y / _ProjectionParams.z;
//     z = x / _ProjectionParams.y;
//     w = y / _ProjectionParams.y;
//     #endif
//
//     return 1.0 / (z * rawdepth + w);
// }
//
// void color_below_water_surface_float(
//     float4 screen_position,
//     float4 water_fog_color,
//     float water_fog_density,
//     float alpha,
//     out float3 color_below,
//     out float3 emission
//     ) {
//     float2 uv = screen_position.xy / screen_position.w;
//     #if UNITY_UV_STARTS_AT_TOP
//     if (_CameraDepthTexture_TexelSize.y < 0) {
//         uv.y = 1 - uv.y;
//     }
//     #endif
//     float background_depth =
//         LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r);
//     float surface_depth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screen_position.z);
//
//     float depth_difference = (background_depth - surface_depth) * 0.1;
//     float3 background_color = SAMPLE_TEXTURE2D(_GrabPassTransparent, sampler_GrabPassTransparent, uv).rgb;
//     float fog_factor = exp2(- water_fog_density * depth_difference);
//     
//     color_below = lerp(water_fog_color, background_color, fog_factor);
//     emission = color_below * (1 - alpha);
// }

#endif // SEE_THROUGH_WATER_CUSTOM_FUNCTIONS
