#if UNITY_STANDALONE
using UnityEngine;

public static class ScreenSetup
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetResolution()
    {

        Screen.SetResolution(1050, 1350, false);

    }

}
#endif
