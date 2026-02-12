using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System.IO;
using Unity.VisualScripting;

public class PerlinNoiseGeneratorWindow : EditorWindow
{
    private enum TextureSizes
    {
        X128 = 128,
        X256 = 256,
        X512 = 512,
        X1024 = 1024,
        X2048 = 2048
    }

    #region Perlin noise properties

    private TextureSizes _textureSize = TextureSizes.X256;
    private float _noiseScale = 50f;
    private float _offsetX = 0f;
    private float _offsetY = 0f;
    private float _noiseMultiplier = 1f;
    private string _fileName = "perlin_noise";

    #endregion

    private Texture2D _previewTexture;
    private bool _parametersChanged;
    private EditorCoroutine _updateRoutine;
    private Vector2 _scrollPosition;

    [MenuItem("Wave shaders/Perlin noise generator")]
    public static void ShowWindow()
    {
        GetWindow<PerlinNoiseGeneratorWindow>("Perlin noise Generator");
    }

    private void OnGUI() {
        GUILayout.Label("Noise settings", EditorStyles.boldLabel);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        
        _textureSize = (TextureSizes)EditorGUILayout.EnumPopup("Texture size", _textureSize);
        _noiseScale = EditorGUILayout.Slider("Noise scale", _noiseScale, 0.01f, 100.0f);
        _offsetX = EditorGUILayout.Slider("X offset", _offsetX, -10f, 10f);
        _offsetY = EditorGUILayout.Slider("Y offset", _offsetY, -10f, 10f);
        _noiseMultiplier = EditorGUILayout.Slider("Noise multiplier", _noiseMultiplier, 0.01f, 4.0f);
        _fileName = EditorGUILayout.TextField("File name", _fileName);
        
        EditorGUILayout.EndScrollView();

        if (EditorGUI.EndChangeCheck()) {
            _parametersChanged = true;
            if (_updateRoutine == null) {
                _updateRoutine = EditorCoroutineUtility.StartCoroutine(ThrottleGenerator(), this);
            }
        }
        
        EditorGUILayout.Space(20);

        if (_previewTexture) {
            var rect = GUILayoutUtility.GetRect(256, 256, GUILayout.ExpandWidth(false));
            EditorGUI.DrawPreviewTexture(rect, _previewTexture);
        }
        
        GUILayout.Space(10);

        if (GUILayout.Button("Save texture")) {
            SaveTexture();
        }
    }
    
    private IEnumerator ThrottleGenerator() {
        yield return new EditorWaitForSeconds(0.3f);
        if (_parametersChanged) {
            _parametersChanged = false;
            GeneratePreviewTexture();
        }

        _updateRoutine = null;
    }

    private void GeneratePreviewTexture() {
        var textureSize = (int)_textureSize;
        Debug.Log($"texture size = {textureSize}");
        _previewTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Repeat,
            filterMode = FilterMode.Bilinear
        };

        var pixels = new Color[textureSize * textureSize];
        for (int y = 0; y < textureSize; y++) {
            for (int x = 0; x < textureSize; x++)
            {
                float xCoord = (float) x / textureSize * _noiseScale + _offsetX;
                float yCoord = (float) y / textureSize * _noiseScale + _offsetY;

                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) * _noiseMultiplier;
                pixels[y * textureSize + x] = new Color(noiseValue, noiseValue, noiseValue, 1.0f);
            }
        }
        
        _previewTexture.SetPixels(pixels);
        _previewTexture.Apply();
        
        Repaint();
    }

    private void SaveTexture() {
        if (!_previewTexture) return;

        var path = EditorUtility.SaveFilePanel(
            "Save Perlin Noise Texture as PNG",
            "",
            _fileName + ".png",
            "png"
        );

        if (string.IsNullOrEmpty(path)) return;
        var pngData = _previewTexture.EncodeToPNG();
        if (pngData != null) {
            File.WriteAllBytes(path, pngData);
            Debug.Log("Texture saved to: " + path);
            AssetDatabase.Refresh();
        }
        else {
            Debug.LogError("Failed to encode texture to PNG.");
        }
    }

    void OnDestroy() {
        if (_previewTexture != null) {
            DestroyImmediate(_previewTexture);
        }
    }
}