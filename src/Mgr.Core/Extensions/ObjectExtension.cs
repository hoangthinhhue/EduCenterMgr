using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Newtonsoft.Json;

namespace Mgr.Core.Extensions;
public static class ObjectExtension
{
    public static T CloneExcept<T, S>(this T target, S source, string[] propertyNames)
    {
        if (source == null)
        {
            return target;
        }
        Type sourceType = typeof(S);
        Type targetType = typeof(T);
        BindingFlags flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        PropertyInfo[] properties = sourceType.GetProperties();
        foreach (PropertyInfo sPI in properties)
        {
            if (!propertyNames.Contains(sPI.Name))
            {
                PropertyInfo tPI = targetType.GetProperty(sPI.Name, flags);
                if (tPI != null && tPI.CanWrite && tPI.PropertyType.IsAssignableFrom(sPI.PropertyType))
                {
                    tPI.SetValue(target, sPI.GetValue(source, null), null);
                }
            }
        }
        return target;
    }
    public static T CloneEntity<T>(this T source)
    {
        try
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
}
