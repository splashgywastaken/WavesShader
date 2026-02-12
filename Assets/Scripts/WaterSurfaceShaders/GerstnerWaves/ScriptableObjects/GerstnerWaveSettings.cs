using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WaveShader.Gerstner
{
    public static class WaveSettingsProperties
    {
        public const string Steepness = "steepness";
        public const string Wavelength = "wavelength";
        public const string WaveRotationAngle = "waveRotationAngle";
    }
    [System.Serializable]
    public struct DisplayWaveSettings
    {
        public float steepness;
        public float wavelength;
        [Range(-90, 90)] 
        public float waveRotationAngle;

    }

    [CreateAssetMenu(fileName = "New Gerstner wave settings", menuName = "Gerstner waves/Wave settings")]
    public class GerstnerWaveSettings : ScriptableObject
    {
        private static class ShaderProperties
        {
            public static readonly int WaveSettingsPacked = Shader.PropertyToID("_WaveSettingsPacked");
            public static readonly int WavesCount = Shader.PropertyToID("_WavesCount");            
        }
        
        // Всегда добавляет набор настроек для двух волн
        [SerializeField] private List<DisplayWaveSettings> waveSettings = new()
        {
            new DisplayWaveSettings
            {
                steepness = .34f,
                wavelength = 16.0f,
                waveRotationAngle = 180.0f
            },
            new DisplayWaveSettings
            {
                steepness = .22f,
                wavelength = 32.0f,
                waveRotationAngle = 180.0f
            }
        };
        
        private Texture2D _waveSettingsPacked;

        private void CreateWaveTexture()
        {
            // Задание ширины текстуры как количества волн
            var textureWidth = waveSettings.Count;
            _waveSettingsPacked = new Texture2D(
                textureWidth,
                1,
                // Формат текстуры - представление величин RGBA через float тип данных
                TextureFormat.RGBAFloat,
                // Выключение создания mip текстур при обработке текстуры пайплайном рендеринга 
                false
            )
            {
                // Режим текстуры - значения без сглаживаний 
                filterMode = FilterMode.Point
            };

            // Задание значений параметров в пиксели текстуры
            for (var i = 0; i < waveSettings.Count; i++)
            {
                // var rotationVector = Quaternion.AngleAxis(waveSettings[i].waveRotationAngle, Vector3.one);
                var rotationVector = new Vector2(
                        Mathf.Cos(waveSettings[i].waveRotationAngle * Mathf.Deg2Rad),
                        Mathf.Sin(waveSettings[i].waveRotationAngle * Mathf.Deg2Rad)
                    );
                
                var pixel = new Color(
                    waveSettings[i].steepness * 256,
                    waveSettings[i].wavelength,
                    rotationVector.x,
                    rotationVector.y
                );
                _waveSettingsPacked.SetPixel(i, 0, pixel);
            }

            // Применение заданных значений
            _waveSettingsPacked.Apply();
        }
        
        public void SetParametersToShader(Renderer renderer)
        {
            // Задание текстуры для записи данных
            CreateWaveTexture();
            
            // Запись данных текстуры в шейдер
            renderer.material.SetFloat(ShaderProperties.WavesCount, waveSettings.Count);
            renderer.material.SetTexture(ShaderProperties.WaveSettingsPacked, _waveSettingsPacked);
        }

        public void SaveTexture()
        {
            if (_waveSettingsPacked)
                WriteTextureToFile(_waveSettingsPacked, $"wave_settings_packed_{_waveSettingsPacked.width}x{_waveSettingsPacked.height}");
        }

        private static void WriteTextureToFile(Texture2D texture2D, string textureName)
        {
            var bytes = texture2D.EncodeToPNG();
            var dirPath = Application.dataPath + "/../SaveImages/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            File.WriteAllBytes(dirPath + textureName + ".png", bytes);
        }

        private static void WriteDebugUV(Vector2 uv, Texture2D texture2D)
        {
            var pixelSample = texture2D.GetPixelBilinear(uv.x, uv.y);
            Debug.Log(
                $"Pixel uv({uv.x},{uv.y}) rgba({pixelSample.r}, {pixelSample.g}, {pixelSample.b}, {pixelSample.a})");
        }
    }
}