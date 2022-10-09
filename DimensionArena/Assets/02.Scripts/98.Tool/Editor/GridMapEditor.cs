using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace GRITTY
{
    public class GridMapEditor : EditorWindow
    {
        /// ==================================
        ///            STATIC REGION
        /// ==================================

#region STATIC Variable, Methods
        public static GridMapEditor window;
        static GameObject parentGround;
        static GameObject parentBlock;
        //40, 40칸씩 보여줄 예정
        static Vector2 windowSize = new Vector2(800, 800);
        public static Vector2Int mapSize;
       
        [MenuItem("Tool/OpenMap")]
        public static void OpenEditorForMapCreator()
        {
            //Open PrefabSelector 
            PrefabSelector.Open();

            parentGround = GameObject.FindWithTag("ParentGround");

            //Check current Map Exists
            if (parentGround)
            {
                OpenMap();
            }
            else
            {
                CreateMap();
            }

            //Open GridMap
            window = GetWindow<GridMapEditor>();
            window.minSize = windowSize;
            window.titleContent = new GUIContent("GridMap");     
            window.Show();
        }

        [MenuItem("Tool/OpenMap", isValidateFunction:true)]
        static bool validate_OpenMap()
        {           
            return GameObject.FindWithTag("MapTool");
        }


        public static void SetMapSize(int row, int colmn)
        {
            mapSize.x = row;
            mapSize.y = colmn;
        }


        private static void OpenMap()
        {
            //유니티가 꺼졌다 켜졌을 때
            if(!parentBlock)
            {
                parentBlock = GameObject.FindWithTag("ParentObstacle");
                mapSize.x = (int)parentGround.transform.localScale.x * 10;
                mapSize.y = (int)parentGround.transform.localScale.z * 10;
            }

            mapSize.x = Mathf.CeilToInt((parentGround.transform.localScale.x) * 10);
            mapSize.y = Mathf.CeilToInt((parentGround.transform.localScale.z) * 10);

            //Texture Setting
            TextureManager.MakeBrownTexture();
            TextureManager.MakeDarkBrownTexture();
        }

        private static void CreateMap()
        {         
            //Ground Setting
            GameObject grounds = Resources.Load<GameObject>("Tool/Ground/Plane");
            parentGround = Instantiate(grounds);
            parentGround.transform.localScale = new Vector3(mapSize.x / 10, 1, mapSize.y / 10);
            parentGround.transform.parent = GameObject.FindWithTag("MapTool").transform;
            parentGround.tag = "ParentGround";

            //Block Setting
            parentBlock = new GameObject();
            parentBlock.name = "Obstacles";
            parentBlock.tag = "ParentObstacle";
            parentBlock.transform.parent = GameObject.FindWithTag("MapTool").transform;

            //Texture Setting
            TextureManager.MakeBrownTexture();
            TextureManager.MakeDarkBrownTexture();

        }
#endregion

        /// ==================================







        /// ==================================
        ///            Local Variable
        /// ==================================
        
#region Local Variable
        int size = 20;
        Vector2 boxSize = new Vector2(20, 20);

        List<List<Node>> list_Node;
        Vector2 offset;
        Vector2 drag;
#endregion

        /// ==================================


        bool IsPrefabInstance(GameObject go)
        {
            return PrefabUtility.GetCorrespondingObjectFromSource(go) != null 
                && PrefabUtility.GetPrefabInstanceHandle(go) != null;
        }

        private void IsPrefabUnpack()
        {
            GameObject mapTool = GameObject.FindWithTag("MapTool");

            if (IsPrefabInstance(mapTool))
            {
                PrefabUtility.UnpackPrefabInstance(mapTool, PrefabUnpackMode.Completely,
                    InteractionMode.AutomatedAction);
            }
        }





        /// ==================================
        ///           UNITY Methods
        /// ==================================
        
#region UNITY Methods
        private void OnEnable()
        {
            IsPrefabUnpack();
            ListAddtoMemory();
            SetBasicGruound();
            RestoreGridMap();
        }


        private void OnGUI()
        {
            //Draw
            DrawGrid();
            DrawNodes();

            //Process Event
            ProcessBrickNodes(Event.current);
            ProcessGrid(Event.current);


            //Repaint
            if(GUI.changed)
            {
                //새로그리기
                Repaint();
            }
        }
#endregion

        /// ==================================






        /// ==================================
        ///        Node Parsing Methods
        /// ==================================
        
#region Node Add, Parsing Methods
        void RestoreGridMap()
        {          
            if (parentBlock)
            {
                //오브젝트가 존재한다면,,
                if (parentBlock.transform.childCount > 0)
                {
                    for (int i = 0; i < parentBlock.transform.childCount; ++i)
                    {
                        ParsingToNode parsingData = parentBlock.transform.GetChild(i).GetComponent<ParsingToNode>();
                        if (parsingData)
                        {
                            NodeInformation nodeinfo = new NodeInformation(parsingData.nodeInfo, parsingData.brown);

                            list_Node[parsingData.idx.y][parsingData.idx.x].block =
                                parentBlock.transform.GetChild(i).gameObject;

                            list_Node[parsingData.idx.y][parsingData.idx.x].nodeInfo = nodeinfo;
                        }
                    }
                }
            }
        }


        void ListAddtoMemory()
        {
            list_Node = new List<List<Node>>();

            for (int i = 0; i < mapSize.y; ++i)
            {
                list_Node.Add(new List<Node>());
            }
        }

        void SetBasicGruound()
        {
            for (int i = 0; i < mapSize.y; ++i)
            {
                for (int j = 0; j < mapSize.x; ++j)
                {
                    if (i % 2 == 0)
                    {
                        list_Node[i].Add(new Node(new Vector2(j * size, i * size), size, size,
                        new NodeInformation(j % 2 == 0)
                        ));
                    }
                    else
                    {
                        list_Node[i].Add(new Node(new Vector2(j * size, i * size), size, size,
                             new NodeInformation(j % 2 == 1)
                             ));
                    }
                }
            }

        }
#endregion

        /// ==================================



        /// ==================================
        ///            Grid Methods
        /// ==================================
        
#region Grid Method
        void DrawGrid()
        {

            Handles.BeginGUI();
            int widthDivider = Mathf.CeilToInt(position.width / size);
            int heightDivider = Mathf.CeilToInt(position.height / size);

            offset += drag;
            Vector3 newOffSet = new Vector3(offset.x % size, offset.y % size, 0);

            //가로 그리기
            for (int i = 0; i < widthDivider; ++i)
            {
                Handles.DrawLine(new Vector3(i * size, -size, 0) + newOffSet,
                                 new Vector3(i * size, position.height, 0) + newOffSet);
            }

            for (int i = 0; i < heightDivider; ++i)
            {
                Handles.DrawLine(new Vector3(-size, i * size, 0) + newOffSet,
                    new Vector3(position.width, i * size, 0) + newOffSet);
            }

            Handles.EndGUI();

        }



        void ProcessGrid(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDrag:
                    if (e.button == 1)
                    {
                        MouseDrag(e.delta);
                    }
                    break;
            }
        }
#endregion

        /// ==================================





        /// ==================================
        ///           Block Methods
        /// ==================================
       
#region Block(Node) Methods
        void DrawNodes()
        {
            foreach (var n in list_Node)
            {
                foreach (var n2 in n)
                {
                    n2.Draw();
                }
            }
        }

        void ProcessBrickNodes(Event e)
        {
            //find idx
            int row =   (int)((e.mousePosition.x - offset.x) / size);
            int colmn = (int)((e.mousePosition.y - offset.y) / size);

            if ((e.mousePosition.x - offset.x < 0) || (e.mousePosition.x - offset.x > mapSize.x * size)
                || (e.mousePosition.y - offset.y < 0) || (e.mousePosition.y - offset.y > mapSize.y * size))
            {
                return;
                //
            }
            else
            {               
                switch(PrefabSelector.state)
                {
                    case SELECTOR_MODE.CREATE:
                        if (e.type == EventType.MouseDown && e.button == 0)
                        {
                            CreateBrick(row, colmn);
                        }

                        if (e.type == EventType.MouseDrag && e.button == 0)
                        {
                            CreateBrick(row, colmn);
                            e.Use();
                        }
                        break;
                    case SELECTOR_MODE.ERASE:
                        if (e.type == EventType.MouseDown && e.button == 0)
                        {
                            DeleteBrick(row, colmn);
                        }

                        if (e.type == EventType.MouseDrag && e.button == 0)
                        {
                            DeleteBrick(row, colmn);
                            e.Use();
                        }
                        break;
                }          
            }
        }

        void CreateBrick(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Node.Count || colmn < 0 || row >= list_Node[colmn].Count)
                return;

            list_Node[colmn][row].CreateBrick(row, colmn, PrefabSelector.window.GetCurPrefab());
            GUI.changed = true;
        }

        void DeleteBrick(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Node.Count || colmn < 0 || row >= list_Node[colmn].Count)
                return;

            list_Node[colmn][row].EraseBrick();
            GUI.changed = true;
        }
#endregion

        /// ==================================







        /// ==================================
        ///            Event Methods
        /// ==================================

#region EVENT Methods
        void MouseDrag(Vector2 delta)
        {
            drag = delta;
            foreach(var n in list_Node)
            {
                foreach(var n2 in n)
                {
                    n2.Drag(drag);
                }
            }
            GUI.changed = true;
        }
#endregion

        /// ==================================

    }
}
#endif