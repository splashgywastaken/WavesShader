using NaughtyAttributes;
using UnityEngine;

namespace WaveShader.Gerstner
{
    [RequireComponent(typeof(Material))]
    public class GerstnerWaveShaderController : MonoBehaviour
    {
        [BoxGroup("Settings to use"), SerializeField, Label("Use flow map")]
        public bool useFlowMap = true;
        [BoxGroup("Settings to use"), SerializeField, Label("See through water")]
        public bool isSeeThroughWater = true;
        [BoxGroup("Settings to use"), SerializeField, Label("Use surface texture distortion")]
        public bool useSurfaceTextureDistortion = true;
        
        [BoxGroup("Settings"), SerializeField]
        public GerstnerWaveSettings gerstnerWaveSettings;
        [BoxGroup("Settings"), SerializeField] 
        public WaterSurfaceAndColorSettings waterSurfaceAndColorSettings;
        [BoxGroup("Settings"), SerializeField, ShowIf("useFlowMap")]
        public FlowMapSettings flowMapSettings;
        [BoxGroup("Settings"), SerializeField, ShowIf("isSeeThroughWater")]
        public SimpleSeeThroughWaterSettings simpleSeeThroughWaterSettings;
        [BoxGroup("Settings"), SerializeField, ShowIf("useSurfaceTextureDistortion")]
        public SurfaceTextureDistortionSettings surfaceTextureDistortionSettings;
        
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            // Setting parameters to shader
            gerstnerWaveSettings.SetParametersToShader(_renderer);
            waterSurfaceAndColorSettings.SetParametersToShader(_renderer);
            if (useFlowMap)
            {
                flowMapSettings.SetParametersToShader(_renderer);   
            }
            if (isSeeThroughWater)
            {
                simpleSeeThroughWaterSettings.SetParametersToShader(_renderer);   
            }
            if (useSurfaceTextureDistortion)
            {
                surfaceTextureDistortionSettings.SetParametersToShader(_renderer);                
            }
        }

        [Button("Save gerstner wave texture")]
        private void SaveGerstnerWaveTextureToFile()
        {
            gerstnerWaveSettings.SaveTexture();
            Debug.Log("saved texture to file");
        }
    }
}



