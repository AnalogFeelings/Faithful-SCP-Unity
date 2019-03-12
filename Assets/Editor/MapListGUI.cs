using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RoomList))]

public class MapListGUI : Editor
{
    RoomList t;
    SerializedObject GetTarget;
    SerializedProperty ThisList;
    RoomType choiceList = RoomType.TwoWay;

    void OnEnable()
    {
        t = (RoomList)target;
        GetTarget = new SerializedObject(t);
        ThisList = GetTarget.FindProperty("RoomList"); // Find the List in our script and create a refrence of it
        
    }

    public override void OnInspectorGUI()
    {
        //Update our list

        GetTarget.Update();

        /*Choose how to display the list<> Example purposes only
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DisplayFieldType = (displayFieldType)EditorGUILayout.EnumPopup("", DisplayFieldType);

        //Resize our list
        EditorGUILayout.Space();
        EditorGUILayout.Space();*/
        SerializedProperty ZoneId = GetTarget.FindProperty("Zone");
        choiceList = (RoomType)EditorGUILayout.EnumPopup("Tipos de Cuarto", choiceList);
        EditorGUILayout.PropertyField(ZoneId);

        switch (choiceList) {
            case RoomType.TwoWay:
                {
                    ThisList = GetTarget.FindProperty("twoWay_List");
                    break;
                }
            case RoomType.CornerWay:
                {
                    ThisList = GetTarget.FindProperty("cornerWay_List");
                    break;
                }
            case RoomType.TWay:
                {
                    ThisList = GetTarget.FindProperty("tWay_List");
                    break;
                }
            case RoomType.EndWay:
                {
                    ThisList = GetTarget.FindProperty("endWay_List");
                    break;
                }
            case RoomType.FourWay:
                {
                    ThisList = GetTarget.FindProperty("fourWay_List");
                    break;
                }
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //Or add a new item to the List<> with a button
        if (GUILayout.Button("Add New"))
        {
            t.AddNew(choiceList);
        }

        EditorGUILayout.Space();

        //Display our list to the inspector window

        for (int i = 0; i < ThisList.arraySize; i++)
        {
            SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
            SerializedProperty Room = MyListRef.FindPropertyRelative("Room");
            SerializedProperty Chance = MyListRef.FindPropertyRelative("Chance");
            SerializedProperty Id = MyListRef.FindPropertyRelative("Id");
            SerializedProperty isSpecial = MyListRef.FindPropertyRelative("isSpecial");
            SerializedProperty hasEvent = MyListRef.FindPropertyRelative("hasEvent");
            SerializedProperty hasSpecial = MyListRef.FindPropertyRelative("hasSpecial");
            SerializedProperty Zone = MyListRef.FindPropertyRelative("Zone");



            // Display the property fields in two ways.


            // Choose to display automatic or custom field types. This is only for example to help display automatic and custom fields.
             //1. Automatic, No customization <-- Choose me I'm automatic and easy to setup
                Texture2D myTexture = AssetPreview.GetAssetPreview(Room.objectReferenceValue);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(myTexture);
                EditorGUILayout.BeginVertical();
                EditorGUIUtility.labelWidth = 60;
                Room.objectReferenceValue = EditorGUILayout.ObjectField("Room", Room.objectReferenceValue, typeof(GameObject), true);
                EditorGUILayout.PropertyField(Chance);
                EditorGUILayout.PropertyField(Id);
                EditorGUILayout.PropertyField(isSpecial);
                EditorGUILayout.PropertyField(Zone);
                EditorGUILayout.PropertyField(hasEvent);
                EditorGUILayout.PropertyField(hasSpecial);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove This Index (" + i.ToString() + ")"))
                {
                    t.Remove(i, choiceList);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

            }

            //Remove this index from the List
            EditorGUILayout.Space();
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();

        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    
        if (GUILayout.Button("Add New"))
                {
                    t.AddNew(choiceList);
                }

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}