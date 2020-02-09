using System;
using System.Reflection;
using UnityEngine;


	public static class ReflectionUtil
	{
		public static void SetPrivateField(this object obj, string fieldName, object value)
		{
			var prop = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			prop.SetValue(obj, value);
		}

		public static T GetPrivateField<T>(this object obj, string fieldName)
		{
			var prop = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			var value = prop.GetValue(obj);
			return (T)value;
		}

	public static T GetPrivateStaticField<T>(Type type, string fieldName)
	{
		var info = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
		var value = info.GetValue(null);
		return (T)value;
	}

	public static void SetPrivateStaticField(Type type, string fieldName, object value)
	{
		var info = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
		info.SetValue(null, value);
	}

	public static void SetPrivateProperty(this object obj, string propertyName, object value)
		{
			var prop = obj.GetType()
				.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			prop.SetValue(obj, value, null);
		}

	public static void InvokePrivateMethod(this object obj, string methodName, object[] methodParams)
	{
		MethodInfo dynMethod = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
		dynMethod.Invoke(obj, methodParams);
	}

	public static void InvokePrivateStaticMethod(Type type, string methodName, object[] methodParams)
	{
		MethodInfo dynMethod = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
		dynMethod.Invoke(null, methodParams);
	}
}