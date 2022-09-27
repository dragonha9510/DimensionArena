//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//namespace GRITTY
//{
//    public class GridMapCreator : EditorWindow
//    {
//        #region Static Value
//        public static Vector2Int worldsize = new Vector2Int();
//        public static GameObject prefab_Ground;
//        #endregion

//        Vector2 offset;
//        Vector2 drag;


//        Rect MenuBar;
//        int nodeSize = 30;

//        //BrickNode
//        List<List<Node>> nodes;
//        List<List<PartScript>> parts;


//        //GroundNode
//        GUIStyle empty;
//        Vector2 nodePos;
        
//        //StyleManager
//        StyleManager stylemanager;
               

//        //2D Pallet Icon Style
//        private GUIStyle currentStyle;
       
//        //Parent Object
//        GameObject TheMap;

//        //Erasing
//        bool isErasing;


//        #region Create Map

//        public static void SetWorldSize(int row, int colmn)
//        {
//            worldsize = new Vector2Int(row, colmn);
//        }


//        public static void CreateTileMap()
//        {      
//            GridMapCreator window = GetWindow<GridMapCreator>();
//            window.titleContent = new GUIContent("GridMapCreator");
//            window.maxSize = window.minSize = new Vector2(800, 600);
//        }

//        #endregion

//        #region UNITY_EDITOR

//        private void OnEnable()
//        {
//            prefab_Ground  = Resources.Load("Tool/Prefabs/Grounds/Ground") as GameObject;
//            //SetUp
//            SetUpStyles();
//            SetUpNodesAndParts();
//            SetUpMap();
//        }

       
//        private void OnGUI()
//        {
//            DrawGrid();
//            DrawNodes();
//            DrawMenuBar();
//            ProcessNodes(Event.current);
//            ProcessGrid(Event.current);

//            if (GUI.changed)
//            {
//                Repaint();
//            }
//        }

//        #endregion


//        #region SetUp

//        private void SetUpMap()
//        {
//            try 
//            {
//                TheMap = GameObject.FindWithTag("Map");
//                RestoretheMap();
//            } catch (Exception) { }

//            if(TheMap == null)
//            {
//                TheMap = new GameObject("Map");
//                TheMap.tag = "Map"; 
//            }
//        }

//        private void SetUpStyles()
//        {
//            try
//            {
//                stylemanager = GameObject.FindGameObjectWithTag("StyleManager").GetComponent<StyleManager>();
//                for (int i = 0; i < stylemanager.buttonstyles.Length; ++i)
//                {
//                    stylemanager.buttonstyles[i].NodeStyle = new GUIStyle();
//                    stylemanager.buttonstyles[i].NodeStyle.normal.background = stylemanager.buttonstyles[i].Icon;
//                }
//            }
//            catch (Exception) { }

//            empty = stylemanager.buttonstyles[0].NodeStyle;
//            currentStyle = stylemanager.buttonstyles[1].NodeStyle;
//        }

//        private void SetUpNodesAndParts()
//        {      
//            GameObject floor = Instantiate(new GameObject());
//            floor.transform.position = Vector3.zero;
//            floor.name = "Ground";
//            floor.tag = "Ground";
//            floor.transform.parent = GameObject.FindWithTag("MapTool").transform;

//            for (int i = 0; i < worldsize.x; ++i)
//            {
//                for (int j = 0; j < worldsize.y; ++j)
//                {                 
//                    GameObject ground = Instantiate(prefab_Ground);
//                    ground.transform.position = new Vector3(i, 5, j);
//                    ground.transform.parent = floor.transform;
//                }
//            }

//            nodes = new List<List<Node>>();
//            parts = new List<List<PartScript>>();

//            for (int i = 0; i < worldsize.x; ++i)
//            {
//                nodes.Add(new List<Node>());
//                parts.Add(new List<PartScript>());
//                for (int j = 0; j < worldsize.y; ++j)
//                {
//                    nodePos.Set(i * nodeSize, j * nodeSize);
//                    nodes[i].Add(new Node(nodePos, nodeSize, nodeSize, empty));
//                    parts[i].Add(null);
//                }
//            }
//        }

//        #endregion

//        private void RestoretheMap()
//        {
//            if(TheMap.transform.childCount > 0)
//            {
//                for(int i = 0; i < TheMap.transform.childCount; ++i)
//                {
//                    int ii = TheMap.transform.GetChild(i).GetComponent<PartScript>().Row;
//                    int jj = TheMap.transform.GetChild(i).GetComponent<PartScript>().Column;
//                    GUIStyle theStyle = TheMap.transform.GetChild(i).GetComponent<PartScript>().style;
//                    //nodes[ii][jj].Setstyle(theStyle);

//                    parts[ii][jj] = TheMap.transform.GetChild(i).GetComponent<PartScript>();
//                    parts[ii][jj].name = TheMap.transform.GetChild(i).name;
//                    parts[ii][jj].PartName = TheMap.transform.GetChild(i).GetComponent<PartScript>().PartName;
//                    parts[ii][jj].Row = ii;
//                    parts[ii][jj].Column = jj;
//                }
//            }
//        }

//        private void DrawMenuBar()
//        {
//            MenuBar = new Rect(0, 0, position.width, 20);
//            GUILayout.BeginArea(MenuBar, EditorStyles.toolbar);
//            GUILayout.BeginHorizontal();

//            for(int i = 0; i < stylemanager.buttonstyles.Length; ++i)
//            {
//                if (GUILayout.Toggle((currentStyle == stylemanager.buttonstyles[i].NodeStyle),
//                    new GUIContent(stylemanager.buttonstyles[i].ButtonTex), GUILayout.Width(80)))
//                {
//                    currentStyle = stylemanager.buttonstyles[i].NodeStyle;

//                }
//            }
//            GUILayout.EndHorizontal();
//            GUILayout.EndArea();
//        }

//        private void ProcessNodes(Event e)
//        {
//            int Row = (int)((e.mousePosition.x - offset.x) / nodeSize);
//            int Col = (int)((e.mousePosition.y - offset.y) / nodeSize);

//            if ((e.mousePosition.x - offset.x) < 0 || (e.mousePosition.x - offset.x) > nodeSize * worldsize.x
//                || (e.mousePosition.y - offset.y) < 0 || (e.mousePosition.y - offset.y) > nodeSize * worldsize.y)
//            {
//            }
//            else
//            {
//                if (e.type == EventType.MouseDown && e.button == 0)
//                {

//                    //if (nodes[Row][Col].style.normal.background.name == "Empty")
//                    //{
//                    //    isErasing = false;
//                    //}
//                    //else
//                    //{
//                    //    isErasing = true;
//                    //}

//                    PaintNodes(Row, Col);
//                }

//                if (e.type == EventType.MouseDrag && e.button == 0)
//                {
//                    PaintNodes(Row, Col);
//                    e.Use();
//                }
//            }

//        }

//        private void PaintNodes(int Row, int Col)
//        {
//            if(isErasing)
//            {
//                if(parts[Row][Col] != null)
//                {
//                    nodes[Row][Col].Setstyle(empty);
//                    DestroyImmediate(parts[Row][Col].gameObject);
//                    GUI.changed = true;
//                }
//                parts[Row][Col] = null;
//            }
//            else
//            {
//                if(parts[Row][Col] == null)
//                {
//                    nodes[Row][Col].Setstyle(currentStyle);

//                    GameObject G = Instantiate(Resources.Load("MapParts/" + currentStyle.normal.background.name) as GameObject);
//                    G.name = currentStyle.normal.background.name;
//                    G.transform.position = new Vector3(Row, 0 , Col) + Vector3.forward * 0.25f + Vector3.right * 0.25f;
//                    G.transform.parent = TheMap.transform;

//                    //Parts Setting
//                    parts[Row][Col] = G.GetComponent<PartScript>();
//                    parts[Row][Col].name = G.name;
//                    parts[Row][Col].PartName = G.name;
//                    parts[Row][Col].Row = Row;
//                    parts[Row][Col].Column = Col;
//                    parts[Row][Col].style = currentStyle;

//                    GUI.changed = true;
//                }
//            }
//        }
      
//        private void DrawNodes()
//        {
//            for (int i = 0; i < worldsize.x; ++i)
//            {
//                for (int j = 0; j < worldsize.y; ++j)
//                {
//                    nodes[i][j].Draw();
//                }
//            }
//        }

//        private void OnMouseDrag(Vector2 delta)
//        {
//            drag = delta;

//            for (int i = 0; i < worldsize.x; ++i)
//            {
//                for (int j = 0; j < worldsize.y; ++j)
//                {
//                    nodes[i][j].Drag(delta);
//                }
//            }


//            GUI.changed = true;
//        }
//        #region GRID SYSTEM
//        private void ProcessGrid(Event e)
//        {
//            drag = Vector2.zero;
//            switch (e.type)
//            {
//                case EventType.MouseDrag:
//                    if (e.button == 1)
//                    {
//                        OnMouseDrag(e.delta);
//                    }
//                    break;
//            }
//        }


//        private void DrawGrid()
//        {
//            int widthDivider = Mathf.CeilToInt(position.width / 20);
//            int heightDivider = Mathf.CeilToInt(position.height / 20);

//            Handles.BeginGUI();
//            Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
//            offset += drag;

//            Vector3 newOffset = new Vector3(offset.x % 20, offset.y % 20, 0);

//            for (int i = 0; i < widthDivider; ++i)
//            {
//                Handles.DrawLine(new Vector3(20 * i, -20, 0) + newOffset, new Vector3(20 * i, position.height, 0) + newOffset);
//            }

//            for (int i = 0; i < heightDivider; ++i)
//            {
//                Handles.DrawLine(new Vector3(-20, 20 * i, 0) + newOffset, new Vector3(position.width, 20 * i, 0) + newOffset);
//            }
//            Handles.color = Color.white;
//            Handles.EndGUI();
//        }
//        #endregion
//    }
//}