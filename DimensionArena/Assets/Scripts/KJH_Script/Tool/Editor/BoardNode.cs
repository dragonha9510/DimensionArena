using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRITTY
{
    public class Node
    {
        Rect rect;
        public NodeInformation nodeInfo;
        public GameObject block;

        public Node(Vector2 position, float width, float height , NodeInformation _nodeInfo)
        {
            rect = new Rect(position.x, position.y, width, height);
            nodeInfo = _nodeInfo;
        }


        public void CreateBrick(int row, int colmn, NodeInformation _nodeInfo)
        {
            nodeInfo.boardStyle.normal.background = _nodeInfo.boardStyle.normal.background;

            //If Block Exist Delete
            if(block)
            {
                MonoBehaviour.DestroyImmediate(block);
            }

            int originRow = GridMapEditor.mapSize.x / 2 == 0 ? (GridMapEditor.mapSize.x / 2) - 1 : (GridMapEditor.mapSize.x / 2);
            int originColmn = GridMapEditor.mapSize.y / 2 == 0 ? (GridMapEditor.mapSize.y / 2) - 1 : (GridMapEditor.mapSize.y / 2);

            block = MonoBehaviour.Instantiate(_nodeInfo.prefab);
            block.transform.position = new Vector3((row - originRow) + 0.5f, 0.5f, -(colmn - originColmn) - 0.5f);
            block.transform.parent = GameObject.Find("Obstacles").transform;
            block.name = _nodeInfo.objectName;



            //Parsing
            ParsingToNode parsingNode = block.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row,colmn);
            parsingNode.brown = ((row + colmn) % 2 == 0);
            parsingNode.nodeInfo = _nodeInfo;

        }

        public void EraseBrick()
        {
            if (block)
            {
                MonoBehaviour.DestroyImmediate(block);
            }

            nodeInfo.boardStyle.normal.background = nodeInfo.basicGroundTexture;
        }
 
        public void Draw()
        {
            GUI.Box(rect, "", nodeInfo.boardStyle);
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }
    }
}
