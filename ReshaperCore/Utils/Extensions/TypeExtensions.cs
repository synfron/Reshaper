using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReshaperCore.Utils.Extensions
{
	public static class TypeExtensions
	{
		public static IEnumerable<T> GetAttributes<T>(this IEnumerable<Type> types) where T : Attribute
		{
			return types.Select(type => type.GetCustomAttribute<T>()).Where(attr => attr != null);
		}
	}
}
