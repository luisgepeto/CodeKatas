using System;
using System.ComponentModel;
using System.Linq;

namespace SpellChecker
{
    public static class SpellCheckerUtil
    {
        //TODO These method will throw an exception if the enum does not have a description attribute
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var descriptionAttribute = (DescriptionAttribute)enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0];
            return descriptionAttribute.Description;
        }

        public static T GetEnum<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                var descriptionAttribute = (DescriptionAttribute)field.GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault();
                if (descriptionAttribute?.Description == description)
                    return (T)field.GetValue(null);
            }
            throw new ApplicationException("Enum description not found");
        }

    }
}
