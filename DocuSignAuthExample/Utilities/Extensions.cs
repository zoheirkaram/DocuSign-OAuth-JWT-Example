using System.Collections;

namespace DocuSignAuthExample.Utilities
{
    public static class Extensions
    {
        public static void DisplayProperties<T>(this T obj, string prefix = "")
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                Console.WriteLine($"{(string.IsNullOrEmpty(prefix) ? "" : prefix)} {property.Name} = {property.GetValue(obj, null)}");

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    foreach (var innerProperty in property.GetValue(obj, null) as IEnumerable)
                    {
                        innerProperty.DisplayProperties("\t-");
                    }
                }
            }
        }
    }
}
