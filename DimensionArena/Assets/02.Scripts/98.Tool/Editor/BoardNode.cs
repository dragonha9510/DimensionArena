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
        public GameObject brick;

        public Node(Vector2 position, float width, float height , NodeInformation _nodeInfo)
        {
            rect = new Rect(position.x, position.y, width, height);
            nodeInfo = _nodeInfo;
        }
        public void CreateBrick(int row, int colmn, NodeInformation _nodeInfo)
        {
            nodeInfo.currentTexture = _nodeInfo.currentTexture;

            //If Block Exist Delete and Overwrite
            if (brick)
            {
                MonoBehaviour.DestroyImmediate(brick);
            }

            int originRow = GridMapEditor.mapSize.x / 2 == 0 ? (GridMapEditor.mapSize.x / 2) - 1 : (GridMapEditor.mapSize.x / 2);
            int originColmn = GridMapEditor.mapSize.y / 2 == 0 ? (GridMapEditor.mapSize.y / 2) - 1 : (GridMapEditor.mapSize.y / 2);


            if(_nodeInfo.type == PREFAB_TYPE.GROUND)
            {
                brick = MonoBehaviour.Instantiate(_nodeInfo.prefab);
                brick.transform.position = new Vector3((row - originRow) + 0.5f, -0.5f, - (colmn - originColmn) - 0.5f);
                brick.transform.parent = GameObject.Find("Floors").transform;
                brick.name = _nodeInfo.objectName;
            }
            else
            {
                brick = MonoBehaviour.Instantiate(_nodeInfo.prefab);
                brick.transform.position = new Vector3((row - originRow) + 0.5f, 0.5f, - (colmn - originColmn) - 0.5f);
                brick.transform.parent = GameObject.Find("Obstacles").transform;
                brick.name = _nodeInfo.objectName;
            }

            //Parsing Data Add to Gameobject
            ParsingToNode parsingNode = brick.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row,colmn);
            parsingNode.nodeInfo = new NodeInformation(_nodeInfo, nodeInfo.basicTexture);
        }

        public void CreateEmptyBrick(int row, int colmn, GameObject obj)
        {
            int originRow = GridMapEditor.mapSize.x / 2 == 0 ? (GridMapEditor.mapSize.x / 2) - 1 : (GridMapEditor.mapSize.x / 2);
            int originColmn = GridMapEditor.mapSize.y / 2 == 0 ? (GridMapEditor.mapSize.y / 2) - 1 : (GridMapEditor.mapSize.y / 2);

            brick = MonoBehaviour.Instantiate(obj);
            brick.transform.position = new Vector3((row - originRow) + 0.5f, 0.5f, -(colmn - originColmn) - 0.5f);
            brick.transform.parent = GameObject.Find("Obstacles").transform;
            brick.name = obj.name;

            //Parsing Data Add to Gameobject
            ParsingToNode parsingNode = brick.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row, colmn);
            parsingNode.nodeInfo = new NodeInformation(TextureManager.alpha);
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
            if (brick)
            {
                MonoBehaviour.DestroyImmediate(brick);
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