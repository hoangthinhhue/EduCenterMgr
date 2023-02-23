using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitMgr.Domain.Configs;
public class AppConfigurationSettings
{
    public const string SectionName = nameof(AppConfigurationSettings);
    public string Secret { get; set; } = String.Empty;
    public bool BehindSSLProxy { get; set; }
    public string ProxyIP { get; set; } = String.Empty;
    public string ApplicationUrl { get; set; } = String.Empty;
    public bool CorsAllowAnyOrigin{get;set; }
    public string[]? CorsAllowOrigins { get; set; }

}
