#ifndef FBM_CUSTOM_FUNCTIONS
#define FBM_CUSTOM_FUNCTIONS

TEXTURE2D(_FbmColorTexture);
SAMPLER(sampler_FbmColorTexture);

void get_fbm_float(float2 uv, out float4 color)
{
    color = SAMPLE_TEXTURE2D(_FbmColorTexture, sampler_FbmColorTexture, uv);
}

#endif // FBM_CUSTOM_FUNCTIONS
