namespace UnitMgr.Admin.Models.Localization;

public record LanguageCode(string Code, string DisplayName, bool IsRTL = false);



public static class LocalizationConstants
{
    public static readonly LanguageCode[] SupportedLanguages =
    {
        new("en-US", "English"),
        new("vi-VN", "VietNam")
    };
}

