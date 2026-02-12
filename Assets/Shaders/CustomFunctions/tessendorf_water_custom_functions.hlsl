#ifndef TESSENDORF_WATER_CUSTOM_FUNCTIONS
#define TESSENDORF_WATER_CUSTOM_FUNCTIONS

float4 sample_texture2d_lod_float(UnityTexture2D tex, float2 uv, float lod) {
    return SAMPLE_TEXTURE2D_LOD(tex, tex.samplerstate, uv, lod);
}

float4 sample_texture2d_float(UnityTexture2D tex, float2 uv)
{
    return sample_texture2d_lod_float(tex, uv, 0);
}

void get_phillips_texture_float(
    UnityTexture2D noise_texture,
    float2 uv,
    out float4 out_color
    ) {
    out_color = sample_texture2d_float(noise_texture, uv);
}

#endif // TESSENDORF_WATER_CUSTOM_FUNCTIONS
