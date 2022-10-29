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
        static GameObject parentFloor;
  
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
                OpenMap();
            else
                CreateMap();

            //Open GridMap
            window = GetWindow<GridMapEditor>();
            window.minSize = windowSize;
            window.titleContent = new GUIContent("GridMap");
            window.Show();
        }

        [MenuItem("Tool/OpenMap", isValidateFunction: true)]
        static bool validate_OpenMap()
        {
            return GameObject.FindWithTag("MapTool");
        }
        private static void OpenMap()
        {
            //유니티가 꺼졌다 켜졌을 때
            if (!parentBlock)
            {
                parentBlock = GameObject.FindWithTag("ParentObstacle");
                mapSize.x = (int)parentGround.transform.localScale.x * 10;
                mapSize.y = (int)parentGround.transform.localScale.z * 10;
            }

            if (!parentFloor)
            {
                parentFloor = GameObject.FindWithTag("ParentFloor");
                mapSize.x = (int)parentGround.transform.localScale.x * 10;
                mapSize.y = (int)parentGround.transform.localScale.z * 10;
            }

            mapSize.x = Mathf.CeilToInt((parentGround.transform.localScale.x) * 10);
            mapSize.y = Mathf.CeilToInt((parentGround.transform.localScale.z) * 10);

            //Texture Setting
            TextureManager.MakeBrownTexture();
            TextureManager.MakeDarkBrownTexture();
            TextureManager.MakeAlphaTexture();
        }
        private static void CreateMap()
        {
            //Ground Setting
            GameObject grounds = Resources.Load<GameObject>("Tool/Ground/Plane");
            parentGround = Instantiate(grounds);
            parentGround.transform.localScale = new Vector3(mapSize.x * 0.1f, 1, mapSize.y * 0.1f);
            parentGround.transform.parent = GameObject.FindWithTag("MapTool").transform;
            parentGround.tag = "ParentGround";

            //Block Parent Setting
            parentBlock = new GameObject();
            parentBlock.name = "Obstacles";
            parentBlock.tag = "ParentObstacle";
            parentBlock.transform.parent = GameObject.FindWithTag("MapTool").transform;

            //Floor Parent Setting
            parentFloor = new GameObject();
            parentFloor.name = "Floors";
            parentFloor.tag = "ParentFloor";
            parentFloor.transform.parent = GameObject.FindWithTag("MapTool").transform;

            //Texture Setting
            TextureManager.MakeBrownTexture();
            TextureManager.MakeDarkBrownTexture();
            TextureManager.MakeAlphaTexture();

        }
        public static void SetMapSize(int row, int colmn)
        {
            mapSize.x = row;
            mapSize.y = colmn;
        }
        #endregion

        /// ==================================


        /// ==================================
        ///            Local Variable
        /// ==================================

        #region Local Variable
        int size = 20;
        Vector2 boxSize = new Vector2(20, 20);

        static List<List<Node>> list_Ground_Node;
        static List<List<Node>> list_Brick_Node;

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

            DrawNode(PrefabSelector.Window.Type);
            ProcessNode(PrefabSelector.Window.Type, Event.current);

            /*
            if (PrefabSelector.Window.Type == PREFAB_TYPE.GROUND)
            {
                DrawGroundNodes();
                ProcessGroundNodes(Event.current);
            }
            else
            {
                DrawBrickNodes();
                ProcessBrickNodes(Event.current);

            }
            */
            //Process Event
            ProcessGrid(Event.current);


            //Repaint
            if (GUI.changed)
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
            if (parentFloor)
            {
                //오브젝트가 존재한다면,,
                if (parentFloor.transform.childCount > 0)
                {
                    ParsingToNode parsingData;
                    for (int i = 0; i < parentFloor.transform.childCount; ++i)
                    {
                        parsingData =  parentFloor.transform.GetChild(i).GetComponent<ParsingToNode>();
                        if (parsingData)
                        {
                            list_Ground_Node[parsingData.idx.y][parsingData.idx.x].brick =
                                parentFloor.transform.GetChild(i).gameObject;

                            list_Ground_Node[parsingData.idx.y][parsingData.idx.x].nodeInfo =
                                parsingData.nodeInfo;

                            list_Brick_Node[parsingData.idx.y][parsingData.idx.x].SetBasicGround(
                                list_Ground_Node[parsingData.idx.y][parsingData.idx.x].nodeInfo.currentTexture);

                            list_Brick_Node[parsingData.idx.y][parsingData.idx.x].SetCurrentTextureToBasicTexture();
                        }
                    }
                }
            }

            if (parentBlock)
            {
                //오브젝트가 존재한다면,,
                if (parentBlock.transform.childCount > 0)
                {
                    ParsingToNode parsingData;
                    for (int i = 0; i < parentBlock.transform.childCount; ++i)
                    {
                        parsingData = parentBlock.transform.GetChild(i).GetComponent<ParsingToNode>();

                        if (parsingData)
                        {
                            list_Brick_Node[parsingData.idx.y][parsingData.idx.x].brick =
                                parentBlock.transform.GetChild(i).gameObject;

                            list_Brick_Node[parsingData.idx.y][parsingData.idx.x].nodeInfo =
                                parsingData.nodeInfo;
                        }
                    }
                }
            }
        }
        void ListAddtoMemory()
        {          
            list_Brick_Node = new List<List<Node>>();
            list_Ground_Node = new List<List<Node>>();

            for (int i = 0; i < mapSize.y; ++i)
            {
                list_Brick_Node.Add(new List<Node>());
                list_Ground_Node.Add(new List<Node>());
            }
        }
        void SetBasicGruound()
        {
            for (int i = 0; i < mapSize.y; ++i)
            {
                for (int j = 0; j < mapSize.x; ++j)
                {
                    if (i % 2 == 0)
                        list_Ground_Node[i].Add(new Node(new Vector2(j * size, i * size), size, size,
                       new NodeInformation(j % 2 == 0)));
                    else
                        list_Ground_Node[i].Add(new Node(new Vector2(j * size, i * size), size, size,
                             new NodeInformation(j % 2 == 1)));

                    list_Brick_Node[i].Add(new Node(new Vector2(j * size, i * size), size, size,
                   new NodeInformation(TextureManager.alpha)));

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
                        if (PrefabSelector.Window.Type == PREFAB_TYPE.GROUND)
                            MouseDragToGroundMode(e.delta);
                        else
                            MouseDragToBrickMode(e.delta);
                    }
                    break;
            }
        }
        #endregion

        /// ==================================
        ///         Node Draw Methods
        /// ==================================

        void DrawNode(PREFAB_TYPE type)
        {
            if (type == PREFAB_TYPE.GROUND)
            {
                foreach (var n in list_Ground_Node)
                {
                    foreach (var n2 in n)
                    {
                        n2.Draw();
                    }
                }
            }
            else
            {
                foreach (var n in list_Brick_Node)
                {
                    foreach (var n2 in n)
                    {
                        n2.Draw();
                    }
                }
            }
        }

        void ProcessNode(PREFAB_TYPE type, Event e)
        {
            bool isChanaged = false;
            int row = (int)((e.mousePosition.x - offset.x) / size);
            int colmn = (int)((e.mousePosition.y - offset.y) / size);

            if ((e.mousePosition.x - offset.x < 0) || (e.mousePosition.x - offset.x > mapSize.x * size)
                || (e.mousePosition.y - offset.y < 0) || (e.mousePosition.y - offset.y > mapSize.y * size))
            {
                return;
            }
            else
            {
                switch (PrefabSelector.state)
                {
                    case SELECTOR_MODE.CREATE:
                        if (e.type == EventType.MouseDown && e.button == 0)
                        {
                            isChanaged = type == PREFAB_TYPE.GROUND ?
                                CreateGround(row, colmn) : CreateBrick(row, colmn);
                        }
                        if (e.type == EventType.MouseDrag && e.button == 0)
                        {
                            isChanaged = type == PREFAB_TYPE.GROUND ?
                                CreateGround(row, colmn) : CreateBrick(row, colmn);
                            e.Use();
                        }
                        break;
                    case SELECTOR_MODE.ERASE:
                        if (e.type == EventType.MouseDown && e.button == 0)
                        {
                            isChanaged = type == PREFAB_TYPE.GROUND ?
                                DeleteGround(row, colmn) : DeleteBrick(row, colmn);

                        }
                        if (e.type == EventType.MouseDrag && e.button == 0)
                        {
                            isChanaged = type == PREFAB_TYPE.GROUND ?
                                DeleteGround(row, colmn) : DeleteBrick(row, colmn);
                            e.Use();
                        }
                        break;
                }
            }

            GUI.changed = isChanaged;
        }
        /// ==================================








        /// ==================================
        ///           Block Methods
        /// ==================================

        #region Block(Node) Methods
        bool CreateBrick(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Brick_Node.Count || colmn < 0 || row >= list_Brick_Node[colmn].Count)
                return false;

            list_Brick_Node[colmn][row].CreateBrick(row, colmn, PrefabSelector.Window.GetCurPrefab());
            return true;
        }
        bool DeleteBrick(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Brick_Node.Count || colmn < 0 || row >= list_Brick_Node[colmn].Count)
                return false;

            list_Brick_Node[colmn][row].EraseBrick();
            return true;
        }


        #endregion

        /// ==================================


        /// ==================================
        ///           Ground Methods
        /// ==================================

        bool CreateGround(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Ground_Node.Count ||
                colmn < 0 || row >= list_Ground_Node[colmn].Count)
                return false;

            list_Ground_Node[colmn][row].CreateBrick(row, colmn, PrefabSelector.Window.GetCurPrefab());
            list_Brick_Node[colmn][row].SetBasicGround(PrefabSelector.Window.GetCurPrefab().normalTexture);

            if (!list_Brick_Node[colmn][row].brick)
                list_Brick_Node[colmn][row].SetCurrentTextureToBasicTexture();

            return true;
        }
        bool DeleteGround(int row, int colmn)
        {
            if (row < 0 || colmn >= list_Ground_Node.Count ||
                colmn < 0 || row >= list_Ground_Node[colmn].Count)
                return false;

            if (list_Brick_Node[colmn][row].nodeInfo.basicTexture 
                == list_Ground_Node[colmn][row].nodeInfo.currentTexture)
                list_Brick_Node[colmn][row].EraseBasicGround();

            if (!list_Brick_Node[colmn][row].brick)
                list_Brick_Node[colmn][row].SetCurrentTextureToBasicTexture();

            list_Ground_Node[colmn][row].EraseBrick();
            return true;

        }


        /// ==================================




        /// ==================================
        ///            Event Methods
        /// ==================================

        #region EVENT Methods
        void MouseDragToBrickMode(Vector2 delta)
        {
            drag = delta;

            foreach (var n in list_Brick_Node)
            {
                foreach (var n2 in n)
                {
                    n2.Drag(drag);
                }
            }

            foreach (var n in list_Ground_Node)
            {
                foreach (var n2 in n)
                {
                    n2.Drag(drag);
                }
            }
            GUI.changed = true;
        }
        void MouseDragToGroundMode(Vector2 delta)
        {
            drag = delta;

            foreach (var n in list_Ground_Node)
            {
                foreach (var n2 in n)
                {
                    n2.Drag(drag);
                }
            }

            foreach (var n in list_Brick_Node)
            {
                foreach (var n2 in n)
                {
                    n2.Drag(drag);
                }
            }
            GUI.changed = true;
        }
        void CreateEmptyBox()
        {
            GameObject emptyBox = Resources.Load<GameObject>("Tool/Brick/Empty_Box");
            for (int i = 0; i < mapSize.y; ++i)
            {
                for (int j = 0; j < mapSize.x; ++j)
                {
                    //모두 비어있을때
                    if(!list_Ground_Node[i][j].brick)
                        if(!list_Brick_Node[i][j].brick)
                            list_Brick_Node[i][j].CreateEmptyBrick(j , i, emptyBox);
                }
            }
        }
        #endregion
        /// ==================================

        private void OnDisable()
        {
            //Brick 빈 오브젝트 체크
            CreateEmptyBox();
        }

    }
}
#endif