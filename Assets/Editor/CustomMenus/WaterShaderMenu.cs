using UnityEditor;
using UnityEngine;

public class WaterShaderMenu : MonoBehaviour
{
    // Water surface:
    // • Light:
    //     ○ Stylized
    //         § Ocean
    //         § Lake
    //         § Pool
    //     ○ Interactive
    //         § Lake 
    //         § Pool
    // • Complex:
    //     ○ PBR
    //         § Ocean
    //         § Lake
    //         § Pool
    //     ○ Interactive
    //         § Lake
    
    // Light
    // - Stylized
    // -- Ocean
    [MenuItem("GameObject/Water surface/Light/Stylized/Ocean")]
    static void AddLightStylizedOcean() {   
        Debug.Log("Added light stylized ocean");
    }
    
    // -- Lake
    [MenuItem("GameObject/Water surface/Light/Stylized/Lake")]
    static void AddLightStylizedLake() {
        Debug.Log("Added light stylized lake");
    }
    
    // -- Pool
    [MenuItem("GameObject/Water surface/Light/Stylized/Pool")]
    static void AddLightStylizedPool() {
        Debug.Log("Added light stylized pool");
    }
    
    // - Interactive
    // -- Lake
    [MenuItem("GameObject/Water surface/Light/Interactive/Lake")]
    static void AddLightInteractiveLake() {
        Debug.Log("Added light interactive lake");
    }
    
    // -- Pool
    [MenuItem("GameObject/Water surface/Light/Interactive/Pool")]
    static void AddLightInteractivePool() {
        Debug.Log("Added light interactive pool");
    }
    
    // Complex
    // - PBR
    // -- Ocean
    [MenuItem("GameObject/Water surface/Complex/PBR/Ocean")]
    static void AddComplexPbrOcean() {
        Debug.Log("Complex pbr ocean");
    }
    
    // -- Lake
    [MenuItem("GameObject/Water surface/Complex/PBR/Lake")]
    static void AddComplexPbrLake() {
        Debug.Log("Complex pbr lake");
    }
    
    // -- Pool
    [MenuItem("GameObject/Water surface/Complex/PBR/Pool")]
    static void AddComplexPbrPool() {
        Debug.Log("Complex pbr pool");
    }
    
    // - Interactive
    // -- Lake
    [MenuItem("GameObject/Water surface/Complex/Interactive/Lake")]
    static void AddComplexInteractiveLake() {
        Debug.Log("Complex interactive lake");
    }
    
    // -- Pool
    [MenuItem("GameObject/Water surface/Complex/Interactive/Pool")]
    static void AddComplexInteractivePool() {
        Debug.Log("Complex interactive pool");
    }
}
