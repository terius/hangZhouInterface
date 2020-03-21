using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Common
{
    public class JsonHelper
    {
        public static string SerializeObj(object obj, bool IgnoreNull = false, bool isCamelCase = false)
        {
            if (obj == null)
            {
                return "";
            }
            var jSetting = new JsonSerializerSettings();
            jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //   jSetting.ContractResolver = new SpecialContractResolver();
            if (IgnoreNull)
            {
                jSetting.NullValueHandling = NullValueHandling.Ignore;
            }
            if (isCamelCase)
            {
                jSetting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            return JsonConvert.SerializeObject(obj, jSetting);
        }

        public static string SerializeCamelCaseObj(object obj)
        {
            return SerializeObj(obj, false, true);
        }

        public static T DeserializeObj<T>(string str)
        {
            JsonSerializerSettings jSetting = new JsonSerializerSettings();
            jSetting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<T>(str, jSetting);
        }

        public static string GetStringValue(string str, string key)
        {
            JObject jObject = JObject.Parse(str);
            return (string)jObject[key];
        }

        public static int GetIntValue(string str, string key)
        {
            JObject jObject = JObject.Parse(str);
            return Convert.ToInt32(jObject[key]);
        }






    }

    //public class NullableValueProvider : IValueProvider
    //{
    //    private readonly object _defaultValue;
    //    private readonly IValueProvider _underlyingValueProvider;


    //    public NullableValueProvider(MemberInfo memberInfo, Type underlyingType)
    //    {
    //        _underlyingValueProvider = new DynamicValueProvider(memberInfo);
    //        _defaultValue = Activator.CreateInstance(underlyingType);
    //    }

    //    public void SetValue(object target, object value)
    //    {
    //        _underlyingValueProvider.SetValue(target, value);
    //    }

    //    public object GetValue(object target)
    //    {
    //        return _underlyingValueProvider.GetValue(target) ?? _defaultValue;
    //    }
    //}

    //public class SpecialContractResolver : DefaultContractResolver
    //{
    //    protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
    //    {

    //        if (member.MemberType == MemberTypes.Property)
    //        {

    //            var pi = (PropertyInfo)member;
    //            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
    //            {
    //                return new NullableValueProvider(member, pi.PropertyType.GetGenericArguments().First());
    //            }

    //        }
    //        else if (member.MemberType == MemberTypes.Field)
    //        {
    //            var fi = (FieldInfo)member;
    //            if (fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
    //                return new NullableValueProvider(member, fi.FieldType.GetGenericArguments().First());
    //        }

    //        return base.CreateMemberValueProvider(member);
    //    }
    //}

    ///// <summary>
    ///// json转换时null转空字符串（已弃用，第二个方法更好）
    ///// </summary>
    //public class NullToEmptyStringResolver : CamelCasePropertyNamesContractResolver
    //{

    //    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    //    {
    //        return type.GetProperties()
    //                .Select(p =>
    //                {
    //                    var jp = base.CreateProperty(p, memberSerialization);
    //                    jp.ValueProvider = new NullToEmptyStringValueProvider(p);
    //                    return jp;
    //                }).ToList();
    //    }

    //    class NullToEmptyStringValueProvider : IValueProvider
    //    {
    //        PropertyInfo _MemberInfo;
    //        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
    //        {
    //            _MemberInfo = memberInfo;
    //        }

    //        public object GetValue(object target)
    //        {
    //            object result = _MemberInfo.GetValue(target);
    //            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
    //            return result;

    //        }

    //        public void SetValue(object target, object value)
    //        {
    //            _MemberInfo.SetValue(target, value);
    //        }
    //    }
    //}


    /// <summary>
    /// json转换时null转为空字符串（这个方法好）
    /// </summary>
    public sealed class SubstituteNullWithEmptyStringContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property.PropertyType == typeof(string))
            {
                // Wrap value provider supplied by Json.NET.
                property.ValueProvider = new NullToEmptyStringValueProvider(property.ValueProvider, property.NullValueHandling);
            }
            return property;
        }

        sealed class NullToEmptyStringValueProvider : IValueProvider
        {
            private readonly IValueProvider Provider;
            private readonly NullValueHandling? NullHandling;

            public NullToEmptyStringValueProvider(IValueProvider provider, NullValueHandling? nullValueHandling)
            {
                Provider = provider ?? throw new ArgumentNullException("provider");
                NullHandling = nullValueHandling;
            }

            public object GetValue(object target)
            {
                if (NullHandling.HasValue
                    && NullHandling.Value == NullValueHandling.Ignore
                    && Provider.GetValue(target) == null)
                {
                    return null;
                }
                return Provider.GetValue(target) ?? "";
            }

            public void SetValue(object target, object value)
            {
                Provider.SetValue(target, value);
            }
        }
    }
}
