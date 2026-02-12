using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaveShader.Gerstner
{
    [CreateAssetMenu(fileName = "Simple see through water settings", menuName = "Gerstner waves/See through settings")]
    public class SimpleSeeThroughWaterSettings : ScriptableObject
    {
        private static class ShaderProperties
        {
            public static readonly int Depth = Shader.PropertyToID("_Depth");
            public static readonly int DepthStrength = Shader.PropertyToID("_Depth_strength");
            public static readonly int RefractStrength = Shader.PropertyToID("_Refract_strength");
            public static readonly int FresnelStrength = Shader.PropertyToID("_Fresnel_strength");
        }
        
        [SerializeField]
        private float depth = 1;
        [SerializeField]
        [Range(0,1)]
        private float depthStrength = 1;
        [SerializeField]
        [Range(-1,1)]
        private float refractStrength = 1;
        [SerializeField]
        [Range(0.0001f,1)]
        private float fresnelStrength = 0.2f;

        public void SetParametersToShader(Renderer renderer)
        {
            renderer.material.SetFloat(ShaderProperties.Depth, depth);
            renderer.material.SetFloat(ShaderProperties.DepthStrength, depthStrength);
            renderer.material.SetFloat(ShaderProperties.RefractStrength, refractStrength);
            renderer.material.SetFloat(ShaderProperties.FresnelStrength, fresnelStrength);
        }
    }
}