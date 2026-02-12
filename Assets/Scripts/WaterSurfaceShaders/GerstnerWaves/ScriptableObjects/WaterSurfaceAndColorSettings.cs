using UnityEngine;

[CreateAssetMenu(fileName = "Water color and surface settings", menuName = "Gerstner waves/Water color and surface settings")]
public class WaterSurfaceAndColorSettings : ScriptableObject
{
    private static class ShaderProperties
    {
        public static readonly int Smoothness = Shader.PropertyToID("_Smoothness");
        public static readonly int Metallic = Shader.PropertyToID("_Metallic");
        public static readonly int MainColor = Shader.PropertyToID("_Main_color");
        public static readonly int DeepWaterColor = Shader.PropertyToID("_Deep_water_color");
    }
    
    [SerializeField]
    private Color deepWaterColor = Color.blue;
    [SerializeField] 
    private Color mainColor = Color.cyan;
    [SerializeField]
    private float smoothness = 0.2f;
    [SerializeField]
    private float metallic = 0.8f;

    public void SetParametersToShader(Renderer renderer)
    {
        renderer.material.SetColor(ShaderProperties.MainColor, mainColor);
        renderer.material.SetColor(ShaderProperties.DeepWaterColor, deepWaterColor);
        renderer.material.SetFloat(ShaderProperties.Metallic, metallic);
        renderer.material.SetFloat(ShaderProperties.Smoothness, smoothness);
    }
}
