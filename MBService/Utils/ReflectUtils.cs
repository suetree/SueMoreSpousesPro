using System;
using System.Reflection;

namespace SueMBService.Utils
{
	public class ReflectUtils
	{
		public static BindingFlags GetBindingFlags()
		{
			return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		}

		public static T ReflectField<T>(string key, object instance)
		{
			T result = default;
			FieldInfo field = instance.GetType().GetField(key, ReflectUtils.GetBindingFlags());
			bool flag = null != field;
			if (flag)
			{
				result = (T)field.GetValue(instance);
			}
			return result;
		}

        public static void  ReflectFieldAndSetValue(string key, object value, object instance)
        {
            FieldInfo field = instance.GetType().GetField(key, ReflectUtils.GetBindingFlags());
            bool flag = null != field;
            if (flag)
            {
                field.SetValue(instance, value);
            }
        }

        public static object ReflectPropertyAndSetValue(string key, object value, object instance)
		{
			object result = null;
			PropertyInfo property = instance.GetType().GetProperty(key, ReflectUtils.GetBindingFlags());
			bool flag = null != property;
			if (flag)
			{
				property.SetValue(instance, value);
			}
			return result;
		}

        public static void  ReflectMethodAndInvoke(string mothodName, object instance, object[] paramObjects)
        {

            MethodInfo method = instance.GetType().GetMethod(mothodName, ReflectUtils.GetBindingFlags());
            bool flag = null != method;
            if (flag)
            {
                method.Invoke(instance, paramObjects);
            }
        }

        public static T ReflectMethodAndInvoke<T>(string mothodName, object instance, object[] paramObjects)
		{
            T result = default;
            MethodInfo method = instance.GetType().GetMethod(mothodName, ReflectUtils.GetBindingFlags());
			bool flag = null != method;
			if (flag)
			{
                object obj = method.Invoke(instance, paramObjects);
                if (null != obj)
                {
                    result = (T)obj;
                }
			}
            return result;

        }
	}
}
