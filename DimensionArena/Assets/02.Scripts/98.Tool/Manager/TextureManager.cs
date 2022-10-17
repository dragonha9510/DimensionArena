using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TextureManager 
{
    public static Texture2D brown;
    public static Texture2D darkbrown;
    public static Texture2D alpha;

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

    public static void MakeAlphaTexture()
    {
        alpha = new Texture2D(128, 128);
        Color32 alphaColor = new Color32(255, 255, 255, 180);

        for (int i = 0; i < darkbrown.width; ++i)
        {
            for (int j = 0; j < darkbrown.height; ++j)
            {
                alpha.SetPixel(i, j, alphaColor);
            }
        }
        alpha.Apply();
    }
}
