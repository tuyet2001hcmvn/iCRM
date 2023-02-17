using ISD.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ISD.Repositories
{
    public static class RepositoryPropertyHelper
    {
        public static string GetPropertyName<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        /// <summary>
        /// Lấy tên hiển thị ở LanguageResource dựa theo lambda expression
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static string GetDisplayName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            string result = string.Empty;
            Type type = typeof(TModel);

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);

            // First look into attributes on a type and it's parents
            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
                        if (attr != null)
                        {
                            result = LanguageResource.ResourceManager.GetString(attr.Name);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy tên hiển thị ở LanguageResource dựa theo field name
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public static string GetDisplayNameByString<TModel>(string PropertyName)
        {
            string result = string.Empty;
            Type type = typeof(TModel);

            // First look into attributes on a type and it's parents
            DisplayAttribute attr;
            if (type.GetProperty(PropertyName) == null)
            {
                return result;
            }
            attr = (DisplayAttribute)type.GetProperty(PropertyName).GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();

            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(PropertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
                        if (attr != null)
                        {
                            result = LanguageResource.ResourceManager.GetString(attr.Name);
                        }
                    }
                }
            }
            else
            {
                result = LanguageResource.ResourceManager.GetString(attr.Name);
            }
            return result;
        }

        /// <summary>
        /// Convert type include Nullable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }
            if (t.Name == "Guid")
            {
                return Guid.Parse(value.ToString());
            }
            return Convert.ChangeType(value, t);
        }
    }
}
