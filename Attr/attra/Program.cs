using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;
using System.ComponentModel;

namespace attra
{
    class Program
    {
        static void Main(string[] args)
        {
            var customerInfoName = typeof(CustomerInfo).GetCustomAttributeValue<NameAttribute, string>(x => x.Name);
            var customerAddressName = typeof(CustomerInfo).GetCustomAttributeValue<NameAttribute, string>(x => x.Name, "Address");
            var customerInfoDesc = typeof(CustomerInfo).GetCustomAttributeValue<DescriptionAttribute, string>(x => x.Description);

            Console.WriteLine("CustomerInfo Name:" + customerInfoName);
            Console.WriteLine("customerInfo >Address Name:" + customerAddressName);
            Console.WriteLine("customerInfo Desc:" + customerInfoDesc);
        }
    }
    [Description("Customer Information")]
    public class CustomerInfo
    {
       
        public string Name { get; set; }

        
        public string Address;
    }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class NameAttribute : Attribute
    {
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        public NameAttribute(string name)
        {
            _name = name;
        }
    }

    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// Cache Data
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
        /// <typeparam name="TReturn">TReturn的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static TReturn GetCustomAttributeValue<TAttribute, TReturn>(this Type sourceType, Func<TAttribute, TReturn> attributeValueAction)
            where TAttribute : Attribute
        {
            return _getAttributeValue(sourceType, attributeValueAction, null);
        }

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="TAttribute">Attribute的子类型</typeparam>
        /// <typeparam name="TReturn">TReturn的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <param name="propertyName">field name或property name</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static TReturn GetCustomAttributeValue<TAttribute, TReturn>(this Type sourceType, Func<TAttribute, TReturn> attributeValueAction, string propertyName)
            where TAttribute : Attribute
        {
            return _getAttributeValue(sourceType, attributeValueAction, propertyName);
        }

        #region private methods

        private static TReturn _getAttributeValue<TAttribute, TReturn>(Type sourceType, Func<TAttribute, TReturn> attributeFunc, string propertyName)
            where TAttribute : Attribute
        {
            var cacheKey = BuildKey<TAttribute>(sourceType, propertyName);
            var value = Cache.GetOrAdd(cacheKey, k => GetValue(sourceType, attributeFunc, propertyName));
            if (value is TReturn) return (TReturn)Cache[cacheKey];
            return default(TReturn);
        }

        private static string BuildKey<TAttribute>(Type type, string propertyName) where TAttribute : Attribute
        {
            var attributeName = typeof(TAttribute).FullName;
            if (string.IsNullOrEmpty(propertyName))
            {
                return type.FullName + "." + attributeName;
            }

            return type.FullName + "." + propertyName + "." + attributeName;
        }

        private static TReturn GetValue<TAttribute, TReturn>(this Type type, Func<TAttribute, TReturn> attributeValueAction, string name)
            where TAttribute : Attribute
        {
            TAttribute attribute = default(TAttribute);
            if (string.IsNullOrEmpty(name))
            {
                attribute = type.GetCustomAttribute<TAttribute>(false);
            }
            else
            {
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    attribute = propertyInfo.GetCustomAttribute<TAttribute>(false);
                }
                else
                {
                    var fieldInfo = type.GetField(name);
                    if (fieldInfo != null)
                    {
                        attribute = fieldInfo.GetCustomAttribute<TAttribute>(false);
                    }
                }
            }

            return attribute == null ? default(TReturn) : attributeValueAction(attribute);
        }

        #endregion
    }
}
