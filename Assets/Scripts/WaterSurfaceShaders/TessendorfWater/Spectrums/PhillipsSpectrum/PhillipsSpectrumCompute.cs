using UnityEngine;
using WaveShader.Tessendorf;

namespace WaveShader.Tessendorf
{
    public class PhillipsSpectrumCompute : ISpectrumCompute
    {
        private PhillipsSpectrumParams _spectrumParams;
        private ComputeShader _spectrumCompute;
        private int _kernelID;
        private Vector2Int _dispatchSize;

        private struct ShaderStrings
        {
            public const string ComputeShaderPath = "Shaders/TessendorfWater/Spectrums/PhillipsSpectrum";
            public const string PhillipsSpectrumKernel = "PhillipsSpectrum";
        }

        public void Initialize()
        {
            // Initialize unchangeable values to use shader and it's kernel
            _spectrumCompute = Resources.Load<ComputeShader>(ShaderStrings.ComputeShaderPath);
            if (!_spectrumCompute)
            {
                Debug.LogError("There is no PhillipsSpectrum compute shader in resources folder." +
                               $"\nAdd shader to {ShaderStrings.ComputeShaderPath} directory.");
            }

            _kernelID = _spectrumCompute.FindKernel(ShaderStrings.PhillipsSpectrumKernel);
            // Update parameters with method
            UpdateParameters();
        }

        public void UpdateSpectrum()
        {
            UpdateParameters();
            DispatchCompute();
        }

        public void UpdateParameters()
        {
            var textureSize = _spectrumParams.GetTextureSize();
            _dispatchSize = new Vector2Int(Mathf.CeilToInt(textureSize / 8.0f), Mathf.CeilToInt(textureSize / 8.0f));
            _spectrumParams.UpdateSpectrumParams();
        }

        public void SetSpectrumParameters(PhillipsSpectrumParams spectrumParams)
        {
            if (spectrumParams.GetSpectrumType() != _spectrumParams.GetSpectrumType())
            {
                Debug.LogError("Wrong spectrum parameters type provided");
                return;
            }

            _spectrumParams = spectrumParams;
        }

        public void DispatchCompute()
        {
            _kernelID = _spectrumCompute.FindKernel(ShaderStrings.PhillipsSpectrumKernel);
            _spectrumParams.GenerateH0Tex(_dispatchSize, _spectrumCompute, _kernelID);
        }
    }
}