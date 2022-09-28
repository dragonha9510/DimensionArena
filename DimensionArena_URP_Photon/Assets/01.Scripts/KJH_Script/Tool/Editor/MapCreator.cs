using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GRITTY
{
    public class MapCreator : ScriptableWizard
    {
        string mapName;
        int rowsize;
        int colmnSize;
        GameObject Gridprefab;
        public static MapCreator Window { get; private set; }

        private void OnEnable()
        {
            Gridprefab  = Resources.Load("Tool/GridSystem") as GameObject;
        }

        [MenuItem("Tool/MapCreator")]
        public static void Open()
        {
            Window = DisplayWizard<MapCreator>("MapCreator");
            Window.maxSize = Window.minSize = new Vector2(300, 250);
        }

        [MenuItem("Tool/MapCreator", isValidateFunction: true)]
        static bool validate_CreateMap()
        {
            return !GameObject.FindWithTag("MapTool");
        }

        protected override bool DrawWizardGUI()
        {

            EditorGUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("World Name"), EditorStyles.boldLabel, GUILayout.Width(80));
            mapName = EditorGUILayout.TextField(mapName, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Row Size"), EditorStyles.boldLabel, GUILayout.Width(80));
            rowsize = EditorGUILayout.IntField(rowsize, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Colmn Size"), EditorStyles.boldLabel, GUILayout.Width(80));
            colmnSize = EditorGUILayout.IntField(colmnSize, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(20);
            return false;
        }

        private void OnWizardCreate()
        {
            rowsize = Mathf.Clamp(rowsize, 10, 100);
            colmnSize = Mathf.Clamp(colmnSize, 10, 100);
            GameObject G = Instantiate(Gridprefab, Vector3.zero, Quaternion.identity);
            G.name = mapName;
            G.tag = "MapTool";

            GridMapEditor.SetMapSize(rowsize, colmnSize);
            GridMapEditor.OpenEditorForMapCreator();
        }
    }
}
