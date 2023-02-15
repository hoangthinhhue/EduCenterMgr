using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Constants;
public static class ConstantStringLocalizer
{
    public const string CONSTANTSTRINGRESOURCEID = "CleanArchitecture.Blazor.Application.Resources.Constants.ConstantString";
    private static readonly ResourceManager rm;
    static ConstantStringLocalizer()
    {
        rm = new ResourceManager(CONSTANTSTRINGRESOURCEID, typeof(ConstantStringLocalizer).Assembly);
    }
    public static string Localize(string key)
    {
        return rm.GetString(key, CultureInfo.CurrentCulture) ?? key;
    }
}
