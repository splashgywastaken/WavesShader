using UnityEditor;
using UnityEngine;
using WaveShader.Gerstner;

namespace WaveShader.Gerstner
{

    [CustomEditor(typeof(GerstnerWaveSettings))]
    [CanEditMultipleObjects]
    public class GerstnerWaveSettingsEditor : Editor
    {
        private SerializedProperty _waveSettingsList;

        private void OnEnable()
        {
            _waveSettingsList = serializedObject.FindProperty("waveSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_waveSettingsList);
            if (GUILayout.Button("Add new wave"))
            {
                AddNewWave();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void AddNewWave()
        {
            _waveSettingsList.InsertArrayElementAtIndex(_waveSettingsList.arraySize - 1);
            var property = _waveSettingsList.GetArrayElementAtIndex(_waveSettingsList.arraySize - 1);
            property.FindPropertyRelative(WaveSettingsProperties.Steepness).floatValue = 0.1f;
            property.FindPropertyRelative(WaveSettingsProperties.Wavelength).floatValue = 0.1f;
            property.FindPropertyRelative(WaveSettingsProperties.WaveRotationAngle).floatValue = 0.1f;

            var info = _waveSettingsList.GetArrayElementAtIndex(_waveSettingsList.arraySize - 1);
            var steepness = info.FindPropertyRelative("steepness").floatValue;
            var wavelength = info.FindPropertyRelative("wavelength").floatValue;
            var waveRotationAngle = info.FindPropertyRelative("waveRotationAngle").floatValue;
            Debug.Log($"Wave info: s: {steepness}, wavelength: {wavelength}, waveRotationAngle: {waveRotationAngle}");
        }
    }
    
}