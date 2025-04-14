using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute/>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        /// <summary>
        /// Get enum display name
        /// </summary>
        /// <param name="enumMember"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static string GetDisplayName<TEnum>(TEnum enumMember)
        {
            var enumType = Nullable.GetUnderlyingType(typeof(TEnum)) ?? typeof(TEnum);
            var fieldName = enumMember.ToString();
            var field = enumType.GetField(fieldName);

            var displayName = field != null ?
                (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) : null;

            return displayName != null ? displayName.Name : fieldName;
        }

        /// <summary>
        /// Parse int value to enum
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ParseEnum<T>(int value)
        {
            var stringValue = value.ToString();
            return (T)Enum.Parse(typeof(T), stringValue, true);
        }
    }
}
