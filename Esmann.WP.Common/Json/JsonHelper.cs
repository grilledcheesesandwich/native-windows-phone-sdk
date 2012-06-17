using System;
using System.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace Esmann.WP.Common.Json
{
    public static class JsonExtensions
    {
        public static JsonValue GetJsonValue<T>(this JsonValue json, Expression<Func<T>> expression)
        {
            if (json == null)
            {
                return null;
            }
            MemberExpression body = (MemberExpression)expression.Body;
            string name = null;
            if (!GetNameFromAttribute(body.Member, ref name))
            {
                name = GetJsonNamingConvention(body.Member.Name);
            }

            var result = json.ContainsKey(name) ? json[name] : null;
            if(result ==  null){
                if (System.Diagnostics.Debugger.IsAttached && !json.ContainsKey(name))
                {
                    throw new ArgumentOutOfRangeException("could not find jsonValue: " + name);
                }
                return null;
            }
            if (result.JsonType == JsonType.Number || result.JsonType == JsonType.Boolean)
            {
                return result.ToString();
            }
            return result;
        }

        private static bool GetNameFromAttribute(MemberInfo member, ref string name)
        {
            var attributes = member.GetCustomAttributes(true);
            if (attributes == null || attributes.Length == 0)
            {
                return false;
            }
            foreach (var attrib in attributes)
            {
                if (attrib is JsonNameAttribute)
                {
                    name = (attrib as JsonNameAttribute).Name;
                    return true;
                }
            }
            return false;
        }

        private static string GetJsonNamingConvention(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var first = name[0].ToString().ToLower();
                var last = name.Substring(1, name.Length - 1);
                return first + last;
            }
            return name;
        }
    }
}