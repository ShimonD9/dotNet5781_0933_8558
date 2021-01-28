
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class DeepCopyUtilities
    {
        /// <summary>
        /// Copies from Source S to Target T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in typeof(T).GetProperties()) // Copy is made for each property
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }

        /// <summary>
        /// Extanstion method that copies from a given object to a new one
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="from"></param>
        /// <param name="type"></param>
        /// <returns>The new target object</returns>
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
          
            return to;
        }
    }
}