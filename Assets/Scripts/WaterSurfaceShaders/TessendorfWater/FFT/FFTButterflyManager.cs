using UnityEngine;
using WaveShader.Utils; 

public class FFTButterflyManager : IFFTManager
{
    private int _n;
    private RenderTexture _twiddleTexture;
    private int _threadGroupSize;
    
    public RenderTexture SpatialHeight { get; private set; }
    public RenderTexture SpatialHx { get; private set; }
    public RenderTexture SpatialHy { get; private set; }
    public int N
    {
        set => _n = value;
        get => _n;
    }
    public int ThreadGroupSize
    {
        set => _threadGroupSize = value;
        get => _threadGroupSize;
    }
    public RenderTexture TwiddleTexture
    {
        set => _twiddleTexture = value;
        get => _twiddleTexture;
    }
    
    private int _logN;
    
    private int _fftHorizontalKernel;
    private int _fftVerticalKernel;
    // TODO: переделать нейминг как ниже для компьютов во всех файлах!!!!
    private ComputeShader _fftCS;

    private RenderTexture _pingA, _pongA; // H
    private RenderTexture _pingB, _pongB; // Hx
    private RenderTexture _pingC, _pongC; // Hy
    
    private struct ShaderStrings
    {
        public const string ShaderPath = "Shaders/TessendorfWater/FFTButterfly";
        public const string FFTHorizontal = "FFT_Horizontal";
        public const string FFTVertical = "FFT_Vertical";
    }

    private struct ShaderProperties
    {
        // ins
        public static readonly int InputA = Shader.PropertyToID("InputA");
        public static readonly int InputB = Shader.PropertyToID("InputB");
        public static readonly int InputC = Shader.PropertyToID("InputC");
        public static readonly int N = Shader.PropertyToID("N");
        public static readonly int Stage = Shader.PropertyToID("stage");
        public static readonly int Direction = Shader.PropertyToID("direction");
        public static readonly int TwiddleTex = Shader.PropertyToID("TwiddleTex");
        
        // outs
        public static readonly int OutputA = Shader.PropertyToID("OutputA");
        public static readonly int OutputB = Shader.PropertyToID("OutputB");
        public static readonly int OutputC = Shader.PropertyToID("OutputC");
    }

    public FFTButterflyManager(int n, RenderTexture twiddleTexture)
    {
        this._n = n;
        this._twiddleTexture = twiddleTexture;
    }
    
    public void Initialize()
    {
        _logN = (int)Mathf.Log(_n);
        _fftCS = Resources.Load<ComputeShader>(ShaderStrings.ShaderPath);
        _fftHorizontalKernel = _fftCS.FindKernel(ShaderStrings.FFTHorizontal);
        _fftVerticalKernel = _fftCS.FindKernel(ShaderStrings.FFTVertical);
        
        CreateRenderTextures();
    }
    
    private void CreateRenderTextures()
    {
        _pingA = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        _pingB = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        _pingC = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        _pongA = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        _pongB = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        _pongC = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        SpatialHeight = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        SpatialHx = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
        SpatialHy = RTHelper.CreateRT(_n, _n, RenderTextureFormat.RGFloat);
    }
    
    public void UpdateParameters()
    {
        
    }

    public void DoFFT(RenderTexture inputSpatial, RenderTexture outputFreq)
    {
        
    }

    public void DoIFFT(RenderTexture inputFreq, RenderTexture outputSpatial)
    {
        Graphics.Blit(inputFreq, _pingA);
        var threadGroups = Mathf.CeilToInt((float) N / _threadGroupSize);

        // Horizontal stages
        for (var s = 0; s < _logN; s++)
        {
            _fftCS.SetInt(ShaderProperties.N, N);
            _fftCS.SetInt(ShaderProperties.Stage, s);
            _fftCS.SetInt(ShaderProperties.Direction, -1);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.TwiddleTex, _twiddleTexture);
            
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputA, _pingA);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputB, _pingB);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputC, _pingC);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputA, _pongA);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputB, _pongB);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputC, _pongC);
            
            CSHelper.Dispatch(threadGroups, _fftCS, _fftHorizontalKernel);
            RTHelper.SwapRT(ref _pingA, ref _pongA);
            RTHelper.SwapRT(ref _pingB, ref _pongB);
            RTHelper.SwapRT(ref _pingB, ref _pongC);
        }
        
        // Vertical stages
        for (var s = 0; s < _logN; s++)
        {
            _fftCS.SetInt(ShaderProperties.N, N);
            _fftCS.SetInt(ShaderProperties.Stage, s);
            _fftCS.SetInt(ShaderProperties.Direction, -1);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.TwiddleTex, _twiddleTexture);
            
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputA, _pingA);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputB, _pingB);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.InputC, _pingC);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputA, _pongA);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputB, _pongB);
            _fftCS.SetTexture(_fftHorizontalKernel, ShaderProperties.OutputC, _pongC);
            
            CSHelper.Dispatch(threadGroups, _fftCS, _fftVerticalKernel);
            RTHelper.SwapRT(ref _pingA, ref _pongA);
            RTHelper.SwapRT(ref _pingB, ref _pongB);
            RTHelper.SwapRT(ref _pingB, ref _pongC);
        }
        Graphics.Blit(_pingA, outputSpatial);
    }
}
