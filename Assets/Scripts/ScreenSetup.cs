using UnityEngine;

public static class ScreenSetup
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetResolution()
    {

        if (!SystemInfo.deviceType.Equals(DeviceType.Desktop))
        {

            return;

        }

        Screen.SetResolution(1050, 1350, false);

    }

}
