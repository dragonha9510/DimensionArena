using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace GRITTY
{
    public enum PREFAB_TYPE
    {
        GROUND,
        BRICK,
        PREFAB_TYPE_END
    }


    [System.Serializable]
    public class NodeInformation
    {
        public static int SIZE = 128;

<<<<<<< HEAD
=======
        public string objectName;
>>>>>>> GrittyCode
        public GameObject prefab;
        public string objectName;
        public PREFAB_TYPE type;


        public GUIStyle prefabStyle;

        public Texture2D currentTexture;
        public Texture2D normalTexture;
        public Texture2D selectedTexture;
        public Texture2D basicTexture;


        public NodeInformation(Texture2D alpha)
        {
            prefabStyle = new GUIStyle();
            normalTexture = new Texture2D(SIZE,SIZE);
            selectedTexture = new Texture2D(SIZE,SIZE);
            basicTexture = new Texture2D(SIZE, SIZE);
            currentTexture = new Texture2D(SIZE,SIZE);

            basicTexture = alpha;
            currentTexture = alpha;
        }


        public NodeInformation(bool brown)
        {
            prefabStyle = new GUIStyle();
            if (brown)
                basicTexture = TextureManager.brown;
            else
                basicTexture = TextureManager.darkbrown;

            currentTexture = basicTexture;
        }

        public NodeInformation(NodeInformation _node, Texture2D _basicTexture)
        {
            prefabStyle = new GUIStyle();

            //prefab이 존재하지 않았던 버그 -> nodeinformation을 담는 parsingtonode class 생성으로 고침          
            if (_node.prefab)
                prefab = _node.prefab;

            type = _node.type;
            objectName = _node.objectName;

            if (type == PREFAB_TYPE.GROUND)
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_GROUND_PATH + objectName);
            else
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_BRICK_PATH + objectName);

            basicTexture = new Texture2D(128, 128);
            basicTexture = _basicTexture;

            if (normalTexture != null)
                SetupStyles();
            else
                CreatePngAndSetUp();
        }



        public NodeInformation(NodeInformation _node, bool brown)
        {
            prefabStyle = new GUIStyle();

            //prefab이 존재하지 않았던 버그 -> nodeinformation을 담는 parsingtonode class 생성으로 고침          
            if (_node.prefab)
                prefab = _node.prefab;

            type = _node.type;
            objectName = _node.objectName;

            if(type == PREFAB_TYPE.GROUND)
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_GROUND_PATH + objectName);
            else
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_BRICK_PATH + objectName);

            basicTexture = new Texture2D(128, 128);

            if (brown)
                basicTexture = TextureManager.brown;
            else
                basicTexture = TextureManager.darkbrown;

            if (normalTexture != null)
                SetupStyles();
            else
                CreatePngAndSetUp();
        }


        public NodeInformation(string name, GameObject prefab, PREFAB_TYPE type, bool brown)
        {
            prefabStyle = new GUIStyle();

            this.prefab = prefab;
            this.type = type;
            objectName = name;

            if (type == PREFAB_TYPE.GROUND)
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_GROUND_PATH + objectName);
            else
                normalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_BRICK_PATH + objectName);
 

            if (brown)
                basicTexture = TextureManager.brown;
            else
                basicTexture = TextureManager.darkbrown;
       

            if (normalTexture)
                SetupStyles();
            else
                CreatePngAndSetUp();
        }
        void SetupStyles()
        {
            // =======================
            //  SETUP PALLETE TEXTURE
            // =======================
            currentTexture = normalTexture;

            // =======================
            //  SETUP PREFAB TEXTURE
            // =======================
            prefabStyle.normal.background = normalTexture;
            prefabStyle.fixedWidth = SIZE;
            prefabStyle.fixedHeight = SIZE;
            prefabStyle.fixedWidth = SIZE;
            prefabStyle.fixedHeight = SIZE;

            // ==========================
            //  CREATE HIGHLIGHT TEXTURE
            // ==========================
            selectedTexture = TextureManager.
                MakeSelectedTexture2D(normalTexture);

        }


        public void CreatePngAndSetUp()
        {
            if(type == PREFAB_TYPE.GROUND)
            {
                if (!Directory.Exists(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH))
                {
                    Directory.CreateDirectory(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH);
                }

                while (!normalTexture)
                {
                    normalTexture = AssetPreview.GetAssetPreview(prefab);
                    Thread.Sleep(80);
                }

                if (normalTexture)
                {
                    normalTexture.mipMapBias = -1.5f;
                    normalTexture.Apply();
                    byte[] bytes = normalTexture.EncodeToPNG();
                    File.WriteAllBytes(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH + objectName + ".png", bytes);
                }
            }
            else
            {
                if (!Directory.Exists(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH))
                {
                    Directory.CreateDirectory(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH);
                }

                while (!normalTexture)
                {
                    normalTexture = AssetPreview.GetAssetPreview(prefab);
                    Thread.Sleep(80);
                }

                if (normalTexture)
                {
                    normalTexture.mipMapBias = -1.5f;
                    normalTexture.Apply();
                    byte[] bytes = normalTexture.EncodeToPNG();
                    File.WriteAllBytes(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH + objectName + ".png", bytes);
                }
            }
           
            SetupStyles();
        }

    }
}

#endif