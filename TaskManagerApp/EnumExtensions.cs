using System;

namespace TaskManagerApp
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayNameAttribute), false);
            return attributes.Length > 0 ? ((DisplayNameAttribute)attributes[0]).Name : enumValue.ToString();
        }
    }
}
