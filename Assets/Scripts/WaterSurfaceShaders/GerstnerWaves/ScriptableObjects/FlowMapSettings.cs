using UnityEngine;

namespace WaveShader.Gerstner
{
    [CreateAssetMenu(fileName = "Flow map settings", menuName = "Gerstner waves/Flow map settings")]
    public class FlowMapSettings : ScriptableObject
    {
        private static class ShaderProperties
        {
            public static readonly int FlowMap = Shader.PropertyToID("_Flow_map");
            public static readonly int FlowStrength = Shader.PropertyToID("_Flow_strength");
            public static readonly int FlowOffset = Shader.PropertyToID("_Flow_offset");
            public static readonly int AnimationSpeed = Shader.PropertyToID("_Animation_speed");
            public static readonly int UVTiling = Shader.PropertyToID("_UV_tiling");
            public static readonly int UJumpPerPhase = Shader.PropertyToID("_U_jump_per_phase");
            public static readonly int VJumpPerPhase = Shader.PropertyToID("_V_jump_per_phase");
        }

        [SerializeField]
        public Texture2D flowMap;
        [SerializeField]
        [Range(0.01f, 2)]
        public float flowStrength = 0.25f;
        [SerializeField]
        [Range(-0.5f, 0.5f)]
        public float flowOffset = 0.0f;
        [SerializeField]
        [Range(0.01f, 8)]
        public float animationSpeed = 1.0f;
        [SerializeField]
        public float uvTiling = 1.0f;
        [SerializeField]
        [Range(-0.75f,0.75f)]
        public float uJumpPerPhase = 0.25f;
        [SerializeField]
        [Range(-0.75f,0.75f)]
        public float vJumpPerPhase = 0.25f;

        public void SetParametersToShader(Renderer renderer)
        {
            renderer.material.SetTexture(ShaderProperties.FlowMap, flowMap);
            renderer.material.SetFloat(ShaderProperties.FlowStrength, flowStrength);
            renderer.material.SetFloat(ShaderProperties.FlowOffset, flowOffset);
            renderer.material.SetFloat(ShaderProperties.AnimationSpeed, animationSpeed);
            renderer.material.SetFloat(ShaderProperties.UVTiling, uvTiling);
            renderer.material.SetFloat(ShaderProperties.UJumpPerPhase, uJumpPerPhase);
            renderer.material.SetFloat(ShaderProperties.VJumpPerPhase, vJumpPerPhase);
        }
    }
}