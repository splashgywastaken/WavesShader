using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class BaseWarpFbmManager : MonoBehaviour
{
    [SerializeField] private ComputeShader fbmComputeShader;
    [SerializeField] private int textureSize = 2048;
    private int _generateFbmTextureHandle;
    private RenderTexture _fbmTexture;

    private struct ShaderProperties
    {
        public static readonly int FbmTime = Shader.PropertyToID("Time");
        public static readonly int FbmResolution = Shader.PropertyToID("Resolution");
        public static readonly int FbmTexture = Shader.PropertyToID("FbmTexture");
        public static readonly int FbmColorGlobalTexture = Shader.PropertyToID("_FbmColorTexture");
    }

    private struct ShaderKernels
    {
        public const string GenerateFbmKernel = "GenerateFbmTexture";
    }
    
    void Start()
    {
        _generateFbmTextureHandle = fbmComputeShader.FindKernel(ShaderKernels.GenerateFbmKernel);
        
        _fbmTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGBFloat)
        {
            enableRandomWrite = true
        };
        _fbmTexture.Create();
        fbmComputeShader.SetTexture(_generateFbmTextureHandle, ShaderProperties.FbmTexture, _fbmTexture);
    }
    
    private void Update()
    { 
        fbmComputeShader.SetFloat(ShaderProperties.FbmTime, Time.time);
        fbmComputeShader.SetVector(ShaderProperties.FbmResolution, new Vector2(_fbmTexture.width, _fbmTexture.height));

        var threadGroupsValue = Mathf.CeilToInt(_fbmTexture.width / 8);
        fbmComputeShader.Dispatch(_generateFbmTextureHandle, threadGroupsValue, threadGroupsValue, 1);
        
        Shader.SetGlobalTexture(ShaderProperties.FbmColorGlobalTexture, _fbmTexture);
    }
}
