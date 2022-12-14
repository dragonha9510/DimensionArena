using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR

namespace GRITTY
{
    public enum SELECTOR_MODE
    { 
        CREATE,
        ERASE,
        SAVE,
        LOAD,
        TOOL_STATE_END
    }

    public class PrefabSelector : EditorWindow
    {

        #region Static Field
        private static PrefabSelector window;
        public static PrefabSelector Window => window;
        public static SELECTOR_MODE state = SELECTOR_MODE.CREATE;
        static Vector2 windowSize = new Vector2(500, 800);
        #endregion

        #region List Prefabs variables;
        List<NodeInformation> ground_Prefabs;
        List<NodeInformation> brick_Prefabs;
        NodeInformation curNodeInfo;

        int prefabCount;
        int idx_Prefab;
        #endregion

        #region HEADER variable

        Rect hederSection;
        Texture2D headerSectionTexture;
        Color headerSectionColor;


        #endregion

        #region Menu Toggle Variable

        Rect menuRect;
        PREFAB_TYPE prefabType = 0;
        public PREFAB_TYPE Type => prefabType;
        float menuYpadding = 30;
        int tab;

        #endregion

        #region Content Variable

        Rect contentViewRect;
        Rect contentRealRect;
        float contentYpadding = 60;

        #endregion

        #region Content Image Variable

        Rect contentRect;

        #endregion

        #region Mode variable

        Rect modeRect;
        GUIStyle createIconStyle;
        GUIStyle eraseIconStyle;
        GUIStyle saveIconStyle;
        GUIStyle loadIconStyle;

        Texture2D createIcon;
        Texture2D createSelIcon;

        Texture2D eraseIcon;
        Texture2D eraseSelIcon;

        Texture2D saveIcon;
        Texture2D saveSelIcon;

        Texture2D loadIcon;
        Texture2D loadSelIcon;


        #endregion


        /// ============================
        ///     UNITY GUI METHODS
        /// ============================

        #region UNITY_EDITOR_METHODS
        private void OnEnable()
        {
            IconTextureAllocate();
            GetAllPrefabsForTool();
            SetGUIRectField();
        }

        private void OnGUI()
        {
            SetGUIRectField();
            //DrawHeader
            DrawHeader();

            //Draw Toggle Menu Bar
            DrawMenuBar();

            //Draw Contents
            DrawContents();

            //Draw Mode
            DrawModeIcon();
        }
        #endregion

        /// ============================
        ///     DRAW TO EDITOR GUI
        /// ============================

        #region DRAW METHOD

        private void IconTextureAllocate()
        {
            //Type Select
            prefabType = PREFAB_TYPE.GROUND;

            //Icon Style Create
            //CREATE
            createIcon = Resources.Load<Texture2D>("Tool/Icon/Create");
            createSelIcon = TextureManager.MakeSelectedTexture2D(createIcon);

            createIconStyle = new GUIStyle();
            createIconStyle.fixedWidth = 80;
            createIconStyle.fixedHeight = 80;
            createIconStyle.normal.background = createIcon;



            //ERASE
            eraseIcon = Resources.Load<Texture2D>("Tool/Icon/Eraser");
            eraseSelIcon = TextureManager.MakeSelectedTexture2D(eraseIcon);

            eraseIconStyle = new GUIStyle();
            eraseIconStyle.fixedWidth = 80;
            eraseIconStyle.fixedHeight = 80;
            eraseIconStyle.normal.background = eraseIcon;


            //SAVE
            saveIcon = Resources.Load<Texture2D>("Tool/Icon/Save");
            saveSelIcon = TextureManager.MakeSelectedTexture2D(saveIcon);

            saveIconStyle = new GUIStyle();
            saveIconStyle.fixedWidth = 80;
            saveIconStyle.fixedHeight = 80;
            saveIconStyle.normal.background = saveIcon;


            //LOAD
            loadIcon = Resources.Load<Texture2D>("Tool/Icon/Load");
            loadSelIcon = TextureManager.MakeSelectedTexture2D(loadIcon);
            loadIconStyle = new GUIStyle();
            loadIconStyle.fixedWidth = 80;
            loadIconStyle.fixedHeight = 80;
            loadIconStyle.normal.background = loadIcon;

        }    
        private void SetGUIRectField()
        {
            prefabCount = prefabCount % 3 == 0 ? prefabCount / 3 : prefabCount / 3 + 1;

            // ================
            //  MENU_RECT_SET
            // ================

            menuRect = new Rect(0, menuYpadding, position.width, menuYpadding * 2);



            // ====================
            //   CONTENT_RECT_SET
            // ====================

            contentViewRect = new Rect(0, contentYpadding,
                position.width, position.height);

            contentRealRect = new Rect(0, 0,
                position.width, prefabCount * NodeInformation.SIZE + contentYpadding);


            // ================
            //  MODE_RECT_SET
            // ================

            modeRect = new Rect(10, menuYpadding * 2, 100, position.height);
        }
        private void DrawHeader()
        {
            //PADDING
            Rect TitleRect = new Rect(0, 0, position.width, 30);
            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.fontSize = 16;
            GUI.Label(TitleRect, "PREFAB SELECTOR", style);
        }
        private void DrawModeIcon()
        {
            GUILayout.BeginArea(modeRect);
            GUILayout.BeginVertical();

            if(GUILayout.Toggle(state == SELECTOR_MODE.CREATE, new GUIContent(),createIconStyle))
            {
                state = SELECTOR_MODE.CREATE;
                createIconStyle.normal.background = createSelIcon;
            }
            else
            {
                createIconStyle.normal.background = createIcon;
            }


            if (GUILayout.Toggle(state == SELECTOR_MODE.ERASE, new GUIContent(), eraseIconStyle))
            {
                state = SELECTOR_MODE.ERASE;
                eraseIconStyle.normal.background = eraseSelIcon;
            }
            else
            {
                eraseIconStyle.normal.background = eraseIcon;
            }


            if (GUILayout.Button(new GUIContent(), saveIconStyle))
            {
                state = SELECTOR_MODE.SAVE;
                saveIconStyle.normal.background = saveSelIcon;
                SaveMap();
            }
            else
            {
                saveIconStyle.normal.background = saveIcon;
            }


            if (GUILayout.Toggle(state == SELECTOR_MODE.LOAD, new GUIContent(), loadIconStyle))
            {
                state = SELECTOR_MODE.LOAD;
                loadIconStyle.normal.background = loadSelIcon;
            }
            else
            {
                loadIconStyle.normal.background = loadIcon;
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawMenuBar()
        {
            GUILayout.BeginArea(menuRect);
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(prefabType == PREFAB_TYPE.GROUND, new GUIContent("GROUND"), 
                EditorStyles.toolbarButton,GUILayout.Width(position.width * 0.5f)))
            {
                prefabType = PREFAB_TYPE.GROUND;
            }

            if (GUILayout.Toggle(prefabType == PREFAB_TYPE.BRICK, new GUIContent("BRICKS"),
             EditorStyles.toolbarButton, GUILayout.Width(position.width * 0.5f)))
            {
                prefabType = PREFAB_TYPE.BRICK;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        private void DrawContents()
        {
            switch (prefabType)
            {
                case PREFAB_TYPE.GROUND:
                    prefabCount = ground_Prefabs.Count;
                    OpenGroundPrefabs();
                    break;
                case PREFAB_TYPE.BRICK:
                    prefabCount = brick_Prefabs.Count;
                    OpenBrickPrefabs();
                    break;
            }
        }
        #endregion


        /// ============================
        ///   GET_EVERTYTHING_PREFAB
        /// ============================

        Vector2 scrollPos = Vector2.zero;

        private void GetAllPrefabsForTool()
        {
            ground_Prefabs = new List<NodeInformation>();
            brick_Prefabs = new List<NodeInformation>();

            GameObject[] grounds = Resources.LoadAll<GameObject>(TOOL_PATH.GROUND_PREFAB_PATH);

            for (int i = 0; i < grounds.Length; ++i)
                ground_Prefabs.Add(
                    new NodeInformation(grounds[i].name, grounds[i], PREFAB_TYPE.GROUND, (i % 2 == 0)));


            GameObject[] bricks = Resources.LoadAll<GameObject>(TOOL_PATH.BRICK_PREFAB_PATH);

            for (int i = 0; i < bricks.Length; ++i)
               brick_Prefabs.Add(
                   new NodeInformation(bricks[i].name, bricks[i], PREFAB_TYPE.BRICK, (i % 2 == 0)));
        }
        private void OpenGroundPrefabs()
        {
            prefabType = PREFAB_TYPE.GROUND;

            scrollPos = GUI.BeginScrollView(contentViewRect, scrollPos, contentRealRect, false,true);

            for (int i = 0; i < ground_Prefabs.Count; ++i)
            {
                if (EditorGUI.Toggle(new Rect((i % 3 * 128) + 100, (i / 3) * 128, 128, 128),
                idx_Prefab == i, ground_Prefabs[i].prefabStyle))
                {
                    idx_Prefab = i;
                    //??????, current PrefabForGrid?? ????????. 
                    ground_Prefabs[i].prefabStyle.normal.background = 
                        ground_Prefabs[i].selectedTexture;

                    curNodeInfo = ground_Prefabs[i];
                }
                else
                {
                    ground_Prefabs[i].prefabStyle.normal.background = 
                        ground_Prefabs[i].normalTexture;
                }
            }

            GUI.EndScrollView();
        }
        private void OpenBrickPrefabs()
        {
            prefabType = PREFAB_TYPE.BRICK;

            scrollPos = GUI.BeginScrollView(contentViewRect, scrollPos, contentRealRect, false, true);

            for (int i = 0; i < brick_Prefabs.Count; ++i)
            {
                if (EditorGUI.Toggle(new Rect((i % 3 * 128) + 100, (i / 3) * 128, 128, 128),
                idx_Prefab == i, brick_Prefabs[i].prefabStyle))
                {
                    idx_Prefab = i;
                    //??????, current PrefabForGrid?? ????????. 
                    brick_Prefabs[i].prefabStyle.normal.background 
                        = brick_Prefabs[i].selectedTexture;

                    curNodeInfo = brick_Prefabs[i];
                }
                else
                {
                    brick_Prefabs[i].prefabStyle.normal.background 
                        = brick_Prefabs[i].normalTexture;
                }
            }

            GUI.EndScrollView();
        }


        /// ============================
        ///   EXTERNALLY_USED_METHOD
        /// ============================

        [MenuItem("Tool/Prefab_Selecotr")]
        public static void Open()
        {
            window = GetWindow<PrefabSelector>();
            window.minSize = window.maxSize = windowSize;
            window.titleContent = new GUIContent("Prefab Selector");

        }
        public NodeInformation GetCurPrefab()
        {
            return curNodeInfo;
        }
        public NodeInformation GetBasicGround()
        {
            return ground_Prefabs[0];
        }

        /// ============================
        ///       TOOL_MODE_METHOD
        /// ============================
         
        private void SaveMap()
        {
            GameObject map = GameObject.FindWithTag("MapTool");

            if(map)
            {
                if(Resources.Load<GameObject>("Tool/Map/" + map.name))
                {
                    if(EditorUtility.DisplayDialog("WARNING!", "???? ???? ??????????! ???????????????", "OK", "Cancle"))
                    {
                        PrefabUtility.SaveAsPrefabAsset(map, TOOL_PATH.MAP_SAVE_PATH + map.name + ".prefab");
                        state = SELECTOR_MODE.CREATE;
 
                    }
                    else
                    {
                        state = SELECTOR_MODE.CREATE;
                    }
                }
                else
                {
                    PrefabUtility.SaveAsPrefabAsset(map, TOOL_PATH.MAP_SAVE_PATH + map.name + ".prefab");
                    state = SELECTOR_MODE.CREATE;
                    EditorUtility.DisplayDialog("???? ????!", "???? ????! ?????????? ???? Resource ?????? ??????.", "OK");
                }
            }

            else
            {
                EditorUtility.DisplayDialog("WARNING!", "Please Create Map!!", "OK");
            }
        }
                         
    }

}

#endif