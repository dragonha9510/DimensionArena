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

        public string objectName;
        public bool isbrown;
        public GameObject prefab;
        public PREFAB_TYPE type;

        public GUIStyle prefabStyle;
        public Texture2D currentTexture;
        public Texture2D gridNormalTexture;
        public Texture2D gridSelctedTexture;
        public Texture2D basicGroundTexture;


        public NodeInformation(bool brown)
        {
            prefabStyle = new GUIStyle();
            if (brown)
            {
                basicGroundTexture = TextureManager.brown;
            }
            else
            {
                basicGroundTexture = TextureManager.darkbrown;
            }

            currentTexture = basicGroundTexture;
        }

        public NodeInformation(NodeInformation _node, bool brown)
        {
            prefabStyle = new GUIStyle();

            //prefab이 존재하지 않았던 버그 -> nodeinformation을 담는 parsingtonode class 생성으로 고침          
            if (_node.prefab)
                prefab = _node.prefab;

            type = _node.type;
            objectName = _node.objectName;
            isbrown = brown;

            if(type == PREFAB_TYPE.GROUND)
                gridNormalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_GROUND_PATH + objectName);
            else
                gridNormalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_BRICK_PATH + objectName);

            basicGroundTexture = new Texture2D(128, 128);

            if (brown)
            {
                basicGroundTexture = TextureManager.brown;
            }
            else
            {
                basicGroundTexture = TextureManager.darkbrown;
            }

            if (gridNormalTexture != null)
            {
                SetupStyles();
            }
            else
            {
                CreatePngAndSetUp();
            }
        }


        public NodeInformation(string name, GameObject prefab, PREFAB_TYPE type, bool brown)
        {
            prefabStyle = new GUIStyle();

            this.prefab = prefab;
            this.type = type;
            isbrown = brown;
            objectName = name;

            if (type == PREFAB_TYPE.GROUND)
                gridNormalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_GROUND_PATH + objectName);
            else
                gridNormalTexture = Resources.Load<Texture2D>
                    (TOOL_PATH.THUMBNAIL_BRICK_PATH + objectName);
 

            if (brown)
                basicGroundTexture = TextureManager.brown;
            else
                basicGroundTexture = TextureManager.darkbrown;
       

            if (gridNormalTexture)
                SetupStyles();
            else
                CreatePngAndSetUp();
        }
        void SetupStyles()
        {
            // =======================
            //  SETUP PALLETE TEXTURE
            // =======================
            currentTexture = gridNormalTexture;

            // =======================
            //  SETUP PREFAB TEXTURE
            // =======================
            prefabStyle.normal.background = gridNormalTexture;
            prefabStyle.fixedWidth = SIZE;
            prefabStyle.fixedHeight = SIZE;
            prefabStyle.fixedWidth = SIZE;
            prefabStyle.fixedHeight = SIZE;

            // ==========================
            //  CREATE HIGHLIGHT TEXTURE
            // ==========================
            gridSelctedTexture = TextureManager.
                MakeSelectedTexture2D(gridNormalTexture);

        }


        public void CreatePngAndSetUp()
        {
            if(type == PREFAB_TYPE.GROUND)
            {
                if (!Directory.Exists(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH))
                {
                    Directory.CreateDirectory(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH);
                }

                while (!gridNormalTexture)
                {
                    gridNormalTexture = AssetPreview.GetAssetPreview(prefab);
                    Thread.Sleep(80);
                }

                if (gridNormalTexture)
                {
                    gridNormalTexture.mipMapBias = -1.5f;
                    gridNormalTexture.Apply();
                    byte[] bytes = gridNormalTexture.EncodeToPNG();
                    File.WriteAllBytes(TOOL_PATH.THUMBNAIL_SAVE_GROUND_PATH + objectName + ".png", bytes);
                }
            }
            else
            {
                if (!Directory.Exists(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH))
                {
                    Directory.CreateDirectory(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH);
                }

                while (!gridNormalTexture)
                {
                    gridNormalTexture = AssetPreview.GetAssetPreview(prefab);
                    Thread.Sleep(80);
                }

                if (gridNormalTexture)
                {
                    gridNormalTexture.mipMapBias = -1.5f;
                    gridNormalTexture.Apply();
                    byte[] bytes = gridNormalTexture.EncodeToPNG();
                    File.WriteAllBytes(TOOL_PATH.THUMBNAIL_SAVE_BRICK_PATH + objectName + ".png", bytes);
                }
            }
           
            SetupStyles();
        }

    }
}

#endif