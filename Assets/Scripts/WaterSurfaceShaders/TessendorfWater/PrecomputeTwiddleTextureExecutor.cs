using System;
using JetBrains.Annotations;
using UnityEngine;
using WaveShader.Utils;

public class TwiddleTextureExecutor
{
    public int? TwiddleTextureSize { get; set; }
    private RenderTexture _twiddleTexture;
    
    private int _kernel;
    private ComputeShader _twiddleTextureShader;

    private const string ShaderPath = "Shaders/TessendorfWater/PrecomputeTwiddleTexture";
    private const string KernelName = "PrecomputeTwiddleTexture";
    
    private struct ShaderProperties
    {
        public static readonly int N = Shader.PropertyToID("N");
        public static readonly int TwiddleTexture = Shader.PropertyToID("twiddleTexture");
    }

    public void Initialize()
    {
        _twiddleTextureShader = Resources.Load<ComputeShader>(ShaderPath);
        _kernel = _twiddleTextureShader.FindKernel(KernelName);
        
        
    }
    
    [CanBeNull]
    public RenderTexture GetTwiddleTexture()
    {
        if (TwiddleTextureSize != null)
        {
            return GetTwiddleTexture((int)TwiddleTextureSize);
        }
        Debug.LogError("Couldn't create twiddle texture, no size were specified");
        return null;
    }
    
    public RenderTexture GetTwiddleTexture(int textureSize)
    {
        var w = textureSize / 2;
        var h = (int)Math.Log(textureSize);
        _twiddleTexture = RTHelper.CreateRT(w, h, RenderTextureFormat.RGFloat);
        _twiddleTextureShader.SetInt(ShaderProperties.N, textureSize);
        _twiddleTextureShader.SetTexture(_kernel, ShaderProperties.TwiddleTexture, _twiddleTexture);
        
        var threadGroupsX = Mathf.CeilToInt((float) w / 8);
        var threadGroupsY = Mathf.CeilToInt((float) h / 8);
        _twiddleTextureShader.Dispatch(_kernel, threadGroupsX, threadGroupsY, 1);

        return _twiddleTexture;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
