using UnityEngine;
using UnityEngine.Serialization;

namespace WaveShader.Gerstner {
    [CreateAssetMenu(fileName = "Surface texture distortion settings", menuName = "Gerstner waves/Surface texture distortion settings")]
    public class SurfaceTextureDistortionSettings : ScriptableObject
    {
        private static class ShaderProperties
        {
            public static readonly int DerivativeHeightMap = Shader.PropertyToID("_Derivative_Height_Map");
            public static readonly int HeightScaleConstant = Shader.PropertyToID("_Height_scale_constant");
            public static readonly int HeightScaleModulated = Shader.PropertyToID("_Height_scale_modulated");
        }

        [SerializeField]
        private Texture2D derivativeHeightMap;
        [SerializeField]
        private float heightScaleConstant;
        [SerializeField] 
        private float heightScaleModulated;

        public void SetParametersToShader(Renderer renderer)
        {
            renderer.material.SetTexture(ShaderProperties.DerivativeHeightMap, derivativeHeightMap);
            renderer.material.SetFloat(ShaderProperties.HeightScaleConstant, heightScaleConstant);
            renderer.material.SetFloat(ShaderProperties.HeightScaleModulated, heightScaleModulated);
        }
    }
}