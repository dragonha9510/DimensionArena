using UnityEngine;

public static class Android_Vibrator
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    public static AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
    public static int defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
    public static AndroidJavaClass vibrationEffectClass;
    public static int defaultAmplitude;
#endif

    private static int getSDKInt()
    {
        if (IsAndroid())
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }
        else
        {
            return -1;
        }
    }
    private static void OldVibrate(long milliseconds)
    {
        vibrator.Call("vibrate", milliseconds);
    }
    private static void OldVibrate(long[] pattern, int repeat)
    {
        vibrator.Call("vibrate", pattern, repeat);
    }

    public static void CreateOneShot(long milliseconds = 250)
    {

        if (IsAndroid())
        {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26)
            {
                CreateOneShot(milliseconds, defaultAmplitude);
            }
            else
            {
                OldVibrate(milliseconds);
            }
        }
        //If not android do simple solution for now
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void CreateOneShot(long milliseconds, int amplitude)
    {

        if (IsAndroid())
        {
            //If Android 8.0 (API 26+) or never use the new vibrationeffects
            if (getSDKInt() >= 26)
            {
                CreateVibrationEffect("createOneShot", new object[] { milliseconds, amplitude });
            }
            else
            {
                OldVibrate(milliseconds);
            }
        }
        //If not android do simple solution for now
        else
        {
            Handheld.Vibrate();
        }
    }

    private static void CreateVibrationEffect(string function, params object[] args)
    {

        AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(function, args);
        vibrator.Call("vibrate", vibrationEffect);
    }

    //public static void Vibrate(long milliseconds = 250)
    //{
    //    if(IsAndroid())
    //    {
    //        vibrator.Call("vibrate", milliseconds);
    //    }
    //    else
    //    {
    //        // Half Second
    //        Handheld.Vibrate();
    //    }
    //}

    public static void Cancel()
    {
        if (IsAndroid())
            vibrator.Call("cancel");

    }

    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
