using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace GRITTY
{
    public struct PATH
    {
        public static string GROUND_PREFAB_PATH = "Tool/Ground/Prefabs/";
        public static string BRICK_PREFAB_PATH = "Tool/Brick/Prefabs/";

        public static string GROUND_BOARD_PATH = "Tool/Ground/Board/";
        public static string BRICK_BOARD_PATH = "Tool/Brick/Board/";

        public static string THUMBNAIL_SAVE_GROUND_PATH = Application.dataPath + "/Resources/Tool/Thumbnail/Ground/";
        public static string THUMBNAIL_SAVE_BRICK_PATH = Application.dataPath + "/Resources/Tool/Thumbnail/Brick/";
        public static string THUMBNAIL_GROUND_PATH = "Tool/Thumbnail/Ground/";
        public static string THUMBNAIL_BRICK_PATH = "Tool/Thumbnail/Brick/";

        public static string MAP_SAVE_PATH = Application.dataPath + "/Resources/Tool/map/";
    }

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
        public Texture2D basicGroundTexture;

        public string objectName;
        public bool isbrown;
        public GameObject prefab;
        public GameObject hireachyObject;
        public GUIStyle gridStyle;
        public GUIStyle boardStyle;
        public PREFAB_TYPE type;

        public Texture2D gridNormalTexture;
        public Texture2D gridSelctedTexture;


        public NodeInformation(bool brown)
        {
            boardStyle = new GUIStyle();

            if (brown)
            {
                basicGroundTexture = TextureManager.brown;
            }
            else
            {
                basicGroundTexture = TextureManager.darkbrown;
            }

            boardStyle.normal.background = basicGroundTexture;
        }

        public NodeInformation(NodeInformation _node, bool brown)
        {
            gridStyle = new GUIStyle();
            boardStyle = new GUIStyle();

            //Prefab이존재하지 않는 버그를 고치자..
            if (_node.prefab)
                prefab = _node.prefab;

            type = _node.type;
            objectName = _node.objectName;
            isbrown = brown;

            if(type == PREFAB_TYPE.GROUND)
            {
                gridNormalTexture = Resources.Load<Texture2D>(PATH.THUMBNAIL_GROUND_PATH + objectName);
            }
            else
            {
                gridNormalTexture = Resources.Load<Texture2D>(PATH.THUMBNAIL_BRICK_PATH + objectName);
            }

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
            gridStyle = new GUIStyle();
            boardStyle = new GUIStyle();
            this.prefab = prefab;
            this.type = type;
            isbrown = brown;
            objectName = name;

            if (type == PREFAB_TYPE.GROUND)
            {
                gridNormalTexture = Resources.Load<Texture2D>(PATH.THUMBNAIL_GROUND_PATH + objectName);
            }
            else
            {
                gridNormalTexture = Resources.Load<Texture2D>(PATH.THUMBNAIL_BRICK_PATH + objectName);
            }
 

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
        void SetupStyles()
        {
            // =======================
            //  SETUP PALLETE TEXTURE
            // =======================

            Texture2D board;     
            board = gridNormalTexture;
           
            boardStyle.normal.background = board;

            // =======================
            //  SETUP PREFAB TEXTURE
            // =======================
            gridStyle.normal.background = gridNormalTexture;
            gridStyle.fixedWidth = SIZE;
            gridStyle.fixedHeight = SIZE;
            gridStyle.fixedWidth = SIZE;
            gridStyle.fixedHeight = SIZE;

            // ==========================
            //  CREATE HIGHLIGHT TEXTURE
            // ==========================
            gridSelctedTexture = TextureManager.MakeSelectedTexture2D(gridNormalTexture);
        }


        public void CreatePngAndSetUp()
        {
            if(type == PREFAB_TYPE.GROUND)
            {
                if (!Directory.Exists(PATH.THUMBNAIL_SAVE_GROUND_PATH))
                {
                    Directory.CreateDirectory(PATH.THUMBNAIL_SAVE_GROUND_PATH);
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
                    File.WriteAllBytes(PATH.THUMBNAIL_SAVE_GROUND_PATH + objectName + ".png", bytes);
                }
            }
            else
            {
                if (!Directory.Exists(PATH.THUMBNAIL_SAVE_BRICK_PATH))
                {
                    Directory.CreateDirectory(PATH.THUMBNAIL_SAVE_BRICK_PATH);
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
                    File.WriteAllBytes(PATH.THUMBNAIL_SAVE_BRICK_PATH + objectName + ".png", bytes);
                }
            }
            

            SetupStyles();
        }

    }

    public static class TextureManager
    {
        static public Texture2D brown;
        static public Texture2D darkbrown;

        static public Texture2D MakeSelectedTexture2D(Texture2D source)
        {
            if (source == null)
            {
                return null;
            }

            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);

            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();

            for (int i = 0; i < readableText.width; ++i)
            {
                for (int j = 0; j < readableText.height; ++j)
                {
                    readableText.SetPixel(i, j, readableText.GetPixel(i, j) * Color.cyan);
                }
            }
            readableText.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            return readableText;
        }


        public static void MakeBrownTexture()
        {
            brown = new Texture2D(128, 128);
            Color32 brownColor = new Color32(249, 166, 117, 255);
            for (int i = 0; i < brown.width; ++i)
            {
                for (int j = 0; j < brown.height; ++j)
                {
                    brown.SetPixel(i, j, brownColor);
                }
            }
            brown.Apply();
        }

        public static void MakeDarkBrownTexture()
        {
            darkbrown = new Texture2D(128, 128);
            Color32 darkbrownColor = new Color32(236, 157, 111, 255);

            for (int i = 0; i < darkbrown.width; ++i)
            {
                for (int j = 0; j < darkbrown.height; ++j)
                {
                    darkbrown.SetPixel(i, j, darkbrownColor);
                }
            }
            darkbrown.Apply();
        }
    }

}

#endif