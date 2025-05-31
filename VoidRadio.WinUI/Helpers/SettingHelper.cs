using Microsoft.Windows.Storage;

namespace VoidRadio.WinUI.Helpers;

public static class SettingHelper
{
    public static void Write<T>(string key, T value)
    {
        ApplicationData.GetDefault().LocalSettings.Values[key] = value;
    }

    public static T? Read<T>(string key)
    {
        var value = ApplicationData.GetDefault().LocalSettings.Values[key];

        return value is null ? default : (T)value;
    }
}