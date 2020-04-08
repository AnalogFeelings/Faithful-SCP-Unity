#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(NGSS_Local))]
[CanEditMultipleObjects]
public class NGSS_LocalEditor : Editor
{
    //Global
    SerializedProperty NGSS_MANAGE_GLOBAL_SETTINGS;
#if !UNITY_5
    SerializedProperty NGSS_PCSS_ENABLED;
#endif
    SerializedProperty NGSS_SAMPLING_TEST;
    SerializedProperty NGSS_SAMPLING_FILTER;
    SerializedProperty NGSS_NOISE_SCALE;
    SerializedProperty NGSS_SHADOWS_OPACITY;
    //Local    
    SerializedProperty NGSS_SHADOWS_SOFTNESS;
    SerializedProperty NGSS_SHADOWS_DITHERING;
    SerializedProperty NGSS_SHADOWS_RESOLUTION;
    SerializedProperty NGSS_DISABLE_ON_PLAY;
    SerializedProperty NGSS_NO_UPDATE_ON_PLAY;

    void OnEnable()
    {
        //We normally call FindProperty here but Unity is acting funny -_-
    }

    public override void OnInspectorGUI()
    {
        ///////////GET PROPERTIES///////////

        NGSS_DISABLE_ON_PLAY = serializedObject.FindProperty("NGSS_DISABLE_ON_PLAY");
        NGSS_NO_UPDATE_ON_PLAY = serializedObject.FindProperty("NGSS_NO_UPDATE_ON_PLAY");

        //Global
        NGSS_MANAGE_GLOBAL_SETTINGS = serializedObject.FindProperty("NGSS_MANAGE_GLOBAL_SETTINGS");
#if !UNITY_5
        NGSS_PCSS_ENABLED = serializedObject.FindProperty("NGSS_PCSS_ENABLED");
#endif
        NGSS_SAMPLING_TEST = serializedObject.FindProperty("NGSS_SAMPLING_TEST");
        NGSS_SAMPLING_FILTER = serializedObject.FindProperty("NGSS_SAMPLING_FILTER");
        NGSS_NOISE_SCALE = serializedObject.FindProperty("NGSS_NOISE_SCALE");
        NGSS_SHADOWS_OPACITY = serializedObject.FindProperty("NGSS_SHADOWS_OPACITY");

        //Local        
        NGSS_SHADOWS_SOFTNESS = serializedObject.FindProperty("NGSS_SHADOWS_SOFTNESS");
        NGSS_SHADOWS_DITHERING = serializedObject.FindProperty("NGSS_SHADOWS_DITHERING");
        NGSS_SHADOWS_RESOLUTION = serializedObject.FindProperty("NGSS_SHADOWS_RESOLUTION");
        
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        ///////////DRAW PROPERTIES///////////

        EditorGUILayout.PropertyField(NGSS_DISABLE_ON_PLAY);
        EditorGUILayout.PropertyField(NGSS_NO_UPDATE_ON_PLAY);

        //Global
        EditorGUILayout.PropertyField(NGSS_MANAGE_GLOBAL_SETTINGS);
        if (NGSS_MANAGE_GLOBAL_SETTINGS.boolValue == true)
        {
#if !UNITY_5
            EditorGUILayout.PropertyField(NGSS_PCSS_ENABLED);
#endif
            EditorGUILayout.PropertyField(NGSS_SAMPLING_TEST);
            EditorGUILayout.PropertyField(NGSS_SAMPLING_FILTER);
            EditorGUILayout.PropertyField(NGSS_NOISE_SCALE);
            EditorGUILayout.PropertyField(NGSS_SHADOWS_OPACITY);
            //EditorGUILayout.Space();
        }
        //Local        
        EditorGUILayout.PropertyField(NGSS_SHADOWS_SOFTNESS);        
        EditorGUILayout.PropertyField(NGSS_SHADOWS_DITHERING);
        EditorGUILayout.PropertyField(NGSS_SHADOWS_RESOLUTION);
        
        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI if modifying values via serializedObject.
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
