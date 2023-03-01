using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitMgr.Domain.Constants;
public class AppSettingConstants
{
    public const int DefaultPageSize = 15;
    public static readonly int[] PageSizeOptions = { 10,15, 30, 50, 100 };

    //File
    public const string SaveFileFolder = "Files";
    public const string SaveTemplateFileFolder = "Files/Template";
}
