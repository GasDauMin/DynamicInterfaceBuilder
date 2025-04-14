namespace DynamicInterfaceBuilder.Core.UI.Enums
{
    public enum ThemeType
    {
        Default,
        SoftDark,
        RedBlackTheme,
        DeepDark,
        GreyTheme,
        DarkGreyTheme,
        LightTheme,
    }

    public static class ThemeTypeExtension
    {
        public static string GetName(this ThemeType type)
        {
            return type.ToString();
        }
    }
}