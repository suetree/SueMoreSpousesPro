using System;
using System.Reflection;

namespace SueMoreSpouses.Utils
{
	internal class ReflectUtils
	{
		public static BindingFlags GetBindingFlags()
		{
			return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		}

		public static object ReflectField(string key, object instance)
		{
			object result = null;
			FieldInfo field = instance.GetType().GetField(key, ReflectUtils.GetBindingFlags());
			bool flag = null != field;
			if (flag)
			{
				result = field.GetValue(instance);
			}
			return result;
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

		public static void ReflectMethodAndInvoke(string mothodName, object instance, object[] paramObjects)
		{
			MethodInfo method = instance.GetType().GetMethod(mothodName, ReflectUtils.GetBindingFlags());
			bool flag = null != method;
			if (flag)
			{
				method.Invoke(instance, paramObjects);
			}
		}
	}
}
