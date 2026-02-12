using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Serialization;
using WaveShader.Utils;

namespace WaveShader.Tessendorf
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Phillips spectrum Params", menuName = "Wave shader/Tessendorf phillips spectrum")]
    public class PhillipsSpectrumParams : ScriptableObject
    {
        // Размер сетки на которой будет происходить генерация
        [SerializeField] private int size = 128;
        // Размер части участка на котором производится симуляция и анимация поверхности воды
        [SerializeField] private float worldSpaceSize = 64.0f;
        // Множитель высоты волн для спектра
        [SerializeField] private float amplitude = 3.0f;
        // Задание вектора направления движения волны по его компонентам
        [SerializeField, Range(-180.0f, 180.0f)] private float windDirectionAngle = 30.0f;
        // Скорость ветра
        [SerializeField, Range(0.5f, 4.0f)] private float windSpeed = 1.0f;
        // Степень для задания более четких волн
        [SerializeField, Range(1, 16)] private int twiddleFactorPower = 9;
        [SerializeField, Range(1, 4)] private int windAlignmentPower = 1;
        [SerializeField, Range(0.001f, 2.0f)] private float spectralExponent = 0.8f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private bool needsUpdate = false;
        private const SpectrumTypes SpectrumType = SpectrumTypes.PhillipsSpectrum;  
        
        // Получение private переменных
        // Вектор направления ветра
        public Vector2 WindDirection => new(
            Mathf.Sin(windDirectionAngle * Mathf.Deg2Rad),
            Mathf.Cos(windDirectionAngle * Mathf.Deg2Rad)
        );
        public int Size
        {
            get => size;
            set
            {
                needsUpdate = true;
                size = value;
            }
        }

        public float WorldSpaceSize => worldSpaceSize;
        public float Amplitude => amplitude;
        public float WindSpeed => windSpeed;
        public int TwiddleFactorPower => twiddleFactorPower;
        public int WindAlignmentPower => windAlignmentPower;
        public float SpectralExponent => spectralExponent;
        public float Gravity => gravity;
        
        public RenderTexture H0Tex { get; private set; }

        public static PhillipsSpectrumParams DefaultSpectrum => CreateInstance<PhillipsSpectrumParams>();

        private struct ShaderProperties
        {
            public static readonly int N = Shader.PropertyToID("N");
            public static readonly int TwiddleFactor = Shader.PropertyToID("twiddleFactor");
            public static readonly int L = Shader.PropertyToID("L");
            public static readonly int G = Shader.PropertyToID("g");
            public static readonly int WindAlignmentPower = Shader.PropertyToID("windAlignmentPower");
            public static readonly int SpectralExponent = Shader.PropertyToID("spectralExponent");
            public static readonly int Amplitude = Shader.PropertyToID("amplitude");
            public static readonly int WindSpeed = Shader.PropertyToID("windSpeed");
            public static readonly int WindDir = Shader.PropertyToID("windDir");
            public static readonly int H0Tex = Shader.PropertyToID("H0");
        }

        public int GetTextureSize()
        {
            return size;
        }

        public void SetTextureSize(int value)
        {
            Size = value;
            needsUpdate = true;
        }
        
        public void GenerateH0Tex(Vector2Int dispatchSize, ComputeShader computeShader, int kernelId)
        {
            computeShader.SetInt(ShaderProperties.N, Size);
            computeShader.SetInt(ShaderProperties.TwiddleFactor, TwiddleFactorPower);
            computeShader.SetFloat(ShaderProperties.L, WorldSpaceSize);
            computeShader.SetFloat(ShaderProperties.G, Gravity);
            computeShader.SetFloat(ShaderProperties.WindAlignmentPower, WindAlignmentPower);
            computeShader.SetFloat(ShaderProperties.SpectralExponent, SpectralExponent);
            computeShader.SetFloat(ShaderProperties.Amplitude, Amplitude);
            computeShader.SetVector(ShaderProperties.WindDir, WindDirection);
            computeShader.SetFloat(ShaderProperties.WindSpeed, WindSpeed);
            computeShader.SetTexture(kernelId, ShaderProperties.H0Tex, H0Tex);
            
            computeShader.Dispatch(kernelId, dispatchSize.x, dispatchSize.y, 1);
        }

        public void UpdateSpectrumParams()
        {
            if (!needsUpdate) return;
            needsUpdate = false;
            
            H0Tex = new RenderTexture(Size, Size, 0, RenderTextureFormat.ARGBFloat)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Point
            };
            H0Tex.Create();
        }

        public void ReleaseTextures()
        {
            H0Tex?.Release();
        }

        public void ChangeSpectrum(ISpectrumParams newParams)
        {
            
        }

        public SpectrumTypes GetSpectrumType()
        {
            return SpectrumType;
        }
    }
}