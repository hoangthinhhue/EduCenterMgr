// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Mgr.Core.Constants;

public static class LocalizationConstants
{
    public const string ResourcesPath = "Resources";
    public static readonly LanguageCode[] SupportedLanguages = {
             new LanguageCode
            {
                Code = "vi-VN",
                DisplayName= "Tiếng Việt"
            },
            new LanguageCode
            {
                Code = "en-US",
                DisplayName= "English"
            },
         
        };
}

public class LanguageCode
{
    public string DisplayName { get; set; } = "vi-VN";
    public string Code { get; set; } = "Tiếng Việt";
}
