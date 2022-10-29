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
            nodeInfo.objectName = _nodeInfo.objectName;
            
            //If Block Exist Delete and Overwrite
            if (brick)
                MonoBehaviour.DestroyImmediate(brick);

            float offsetRow = GridMapEditor.mapSize.x * 0.5f - 0.5f;
            float offsetColmn = GridMapEditor.mapSize.y * 0.5f - 0.5f;

            //Ground Obstacle split a branch
            string parentStr = _nodeInfo.type == PREFAB_TYPE.GROUND ? "Floors" : "Obstacles";
            float yPos = _nodeInfo.type == PREFAB_TYPE.GROUND ? -0.5f : 0.5f;
            brick = MonoBehaviour.Instantiate(_nodeInfo.prefab);
            brick.transform.position = new Vector3(row - offsetRow, yPos, offsetColmn - colmn);
            brick.name = _nodeInfo.objectName;
            brick.transform.SetParent(GameObject.Find(parentStr).transform);

        
            //Parsing Data Add to Gameobject
            ParsingToNode parsingNode = brick.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row,colmn);
            parsingNode.name = _nodeInfo.objectName;
            parsingNode.nodeInfo = new NodeInformation(_nodeInfo, nodeInfo.basicTexture);
        }

        public void CreateEmptyBrick(int row, int colmn, GameObject obj)
        {
            float offsetRow = GridMapEditor.mapSize.x * 0.5f - 0.5f;
            float offsetColmn = GridMapEditor.mapSize.y * 0.5f - 0.5f;

            brick = MonoBehaviour.Instantiate(obj);
            brick.transform.position = new Vector3(row - offsetRow, 0.5f, offsetColmn - colmn);
            brick.transform.parent = GameObject.Find("Obstacles").transform;
            brick.name = obj.name;

            //Parsing Data Add to Gameobject
            ParsingToNode parsingNode = brick.AddComponent<ParsingToNode>();
            parsingNode.rect = rect;
            parsingNode.idx = new Vector2Int(row, colmn);
            parsingNode.nodeInfo = new NodeInformation(TextureManager.alpha);
        }


        public void ChangeCurrentTexture(Texture2D texture)
        {
            nodeInfo.currentTexture = texture;
        }

        public void SetBasicGround(Texture2D basicTexture)
        {
            nodeInfo.basicTexture = basicTexture;
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