#ifndef GERSTNER_CUSTOM_FUNCTIONS_INCLUDED
#define GERSTNER_CUSTOM_FUNCTIONS_INCLUDED

#include "Assets/Shaders/CustomFunctions/Math/Math.hlsl"

void get_k_float(float wavelength, out float Out) {
    Out =  2 * MATH_PI / wavelength;
}

float get_k_float(float wavelength) {
    return  2 * MATH_PI / wavelength;
}

void get_f_float(float wavelength, float c, float3 p, float2 direction, float time, out float Out) {
    float k;
    get_k_float(wavelength, k);
    Out = k * (dot(direction, p.xz) - c * time);
}

float get_f_float(float wavelength, float c, float3 p, float2 direction, float time) {
    float k;
    get_k_float(wavelength, k);
    return k * (dot(direction, p.xz) - c * time);
}

void get_gerstner_wave_float(float steepness, float k, float f, float3 p, float2 direction, out float3 Out) {
    float a = steepness / k;
    float multiplier = a * cos(f);
    
    Out = float3(
            p.x + direction.x * multiplier,
            a * sin(f),
            p.z + direction.y * multiplier
        );
}

float3 get_gerstner_wave_float(float steepness, float k, float f, float3 p, float2 direction) {
    float a = steepness / k;
    float multiplier = a * cos(f);
    
    return float3(
            p.x + direction.x * multiplier,
            a * sin(f),
            p.z + direction.y * multiplier
        );
}

void get_tangent_float(float steepness, float f, float2 direction, out float3 Out) {
    Out = normalize(float3(
            1 - direction.x * direction.x * steepness * sin(f),
            direction.x * steepness * cos(f),
            - direction.x * direction.y * steepness * sin(f)
        ));
}

float3 get_tangent_float(float steepness, float f, float2 direction) {
    return normalize(float3(
            1 - direction.x * direction.x * steepness * sin(f),
            direction.x * steepness * cos(f),
            - direction.x * direction.y * steepness * sin(f)
        ));
}

void get_binormal_float(float steepness, float f, float2 direction, out float3 Out) {
    Out = normalize(float3(
            - direction.x * direction.x * steepness * sin(f),
            direction.y * steepness * cos(f),
            1 - direction.y * direction.y * steepness * sin(f)
        ));
}

float3 get_binormal_float(float steepness, float f, float2 direction) {
    return normalize(float3(
            - direction.x * direction.x * steepness * sin(f),
            direction.y * steepness * cos(f),
            1 - direction.y * direction.y * steepness * sin(f)
        ));
}

float4 sample_texture2d_lod_float(UnityTexture2D tex, float2 uv) {
    return SAMPLE_TEXTURE2D_LOD(tex, tex.samplerstate, uv, 0);
}

void evaluate_gerstner_waves_float(
    float wave_count,
    UnityTexture2D wave_settings_packed,
    float3 position,
    float time,
    out float3 out_position,
    out float3 out_normal
) {
    float wave_data_step =  1 / wave_count;
    float3 binormal = float3(0.0f,0.0f,0.0f);
    float3 tangent = float3(0.0f,0.0f,0.0f);
    for (int idx = 0; idx < (int)wave_count; idx++) {
        float2 uv = float2(0.0f + wave_data_step * idx, 0.0f);
        // распаковка данных
        float4 wave_data = sample_texture2d_lod_float(wave_settings_packed, uv);
        float steepness = wave_data.r * 0.00390625;
        int wavelength = (int)wave_data.g;
        float2 direction_normalized = wave_data.ba;
    
        float k = get_k_float(wavelength);
        float c = sqrt(9.8 / k);
        float f = get_f_float(wavelength, c, position, direction_normalized, time);
    
        out_position += get_gerstner_wave_float(steepness, k, f, position, direction_normalized);
        binormal += get_binormal_float(steepness, f, direction_normalized);
        tangent += get_tangent_float(steepness, f, direction_normalized);
    }
    out_position.xz *= wave_data_step;
    out_normal = cross(normalize(binormal * wave_data_step), normalize(tangent * wave_data_step));
}

#endif // GERSTNER_CUSTOM_FUNCTIONS_INCLUDED
