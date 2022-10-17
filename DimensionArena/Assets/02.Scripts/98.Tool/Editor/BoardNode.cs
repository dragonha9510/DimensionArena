using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

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
            nodeInfo.currentTexture = _nodeInfo.currentTexture;

            //If Block Exist Delete and Overwrite
            if (block)
            {
                MonoBehaviour.DestroyImmediate(block);
            }

            int originRow = GridMapEditor.mapSize.x / 2 == 0 ? (GridMapEditor.mapSize.x / 2) - 1 : (GridMapEditor.mapSize.x / 2);
            int originColmn = GridMapEditor.mapSize.y / 2 == 0 ? (GridMapEditor.mapSize.y / 2) - 1 : (GridMapEditor.mapSize.y / 2);


            if(_nodeInfo.type == PREFAB_TYPE.GROUND)
            {
                block = MonoBehaviour.Instantiate(_nodeInfo.prefab);
                block.transform.position = new Vector3((row - originRow) + 0.5f, -0.5f, - (colmn - originColmn) - 0.5f);
                block.transform.parent = GameObject.Find("Floors").transform;
                block.name = _nodeInfo.objectName;
            }
            else
            {
                block = MonoBehaviour.Instantiate(_nodeInfo.prefab);
                block.transform.position = new Vector3((row - originRow) + 0.5f, 0.5f, - (colmn - originColmn) - 0.5f);
                block.transform.parent = GameObject.Find("Obstacles").transform;
                block.name = _nodeInfo.objectName;
            }

            //Parsing Data Add to Gameobject
            ParsingToNode parsingNode = block.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row,colmn);
            parsingNode.nodeInfo = new NodeInformation(_nodeInfo, nodeInfo.basicTexture);
        }

        public void SetBasicGround(Texture2D basicTexture)
        {
            nodeInfo.basicTexture = basicTexture;
            nodeInfo.currentTexture = basicTexture;
        }

        public void EraseBasicGround()
        {
            nodeInfo.basicTexture = TextureManager.alpha;
            nodeInfo.currentTexture = nodeInfo.basicTexture;
        }

        public bool EraseBrick()
        {
            if (block)
            {
                MonoBehaviour.DestroyImmediate(block);
                nodeInfo.currentTexture = nodeInfo.basicTexture;
                return true;
            }

            return false;
        }
        public void Draw()
        {
            GUI.DrawTexture(rect, nodeInfo.currentTexture);
        }
        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }
    }
}
#endif