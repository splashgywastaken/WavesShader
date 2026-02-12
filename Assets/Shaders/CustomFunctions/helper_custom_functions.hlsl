#ifndef HELPER_CUSTOM_FUNCTIONS_INCLUDED
#define HELPER_CUSTOM_FUNCTIONS_INCLUDED

float4 sample_texture2d_float(UnityTexture2D tex, float2 uv) {
    return SAMPLE_TEXTURE2D(tex, tex.samplerstate, uv);
}

float3 unpack_normal_float(UnityTexture2D normal_tex, float2 uv) {
    float3 normal = float3(.0f,.0f,.0f);
    // Пересчёт значений согласно тому как это делается для нормалей
    // из-за DXTnm формата сжатия изображений
    normal.xy = sample_texture2d_float(normal_tex, uv).xy * 2 - 1;
    // Восстановление значений для Z координаты
    // согласно тому как считаются касательные к вершине
    normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
    return normal;  
}

float3 unpack_derivative_height_float(UnityTexture2D derivative_height_map, float2 uv) {
    // Получение AGB каналов текстуры
    // Getting alpha green and blue channels
    float3 dh = sample_texture2d_float(derivative_height_map, uv).wyz;
    // Пересчёт значений согласно тому как это делается для нормалей
    // из-за DXTnm формата сжатия изображений
    dh.xy = dh.xy * 2 - 1;
    return dh;
}

#endif // HELPER_CUSTOM_FUNCTIONS_INCLUDED
