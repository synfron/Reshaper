using System;
using System.Collections.Generic;
using System.Linq;

namespace ReshaperCore.Utils.Extensions
{
	public static class CollectionExtensions
	{
		public static T GetElementAtOrDefault<T>(this IList<T> list, int index)
		{
			return (list.Count > index) ? list[index] : default(T);
		}

		public static T[] Combine<T>(this T[] array1, T[] array2)
		{
			T[] newArray = new T[array1.Length + array2.Length];
			Buffer.BlockCopy(array1, 0, newArray, 0, array1.Length);
			Buffer.BlockCopy(array2, 0, newArray, array1.Length, array2.Length);
			return newArray;
		}

		public static T[] Combine<T>(this T[] array1, params T[][] otherArrays)
		{
			int combinedSize = otherArrays.Sum(array => array.Length);
			T[] newArray = new T[array1.Length + combinedSize];
			Buffer.BlockCopy(array1, 0, newArray, 0, array1.Length);
			int combinedArrayIndex = array1.Length;
			for (int otherArrayIndex = 0; otherArrayIndex < otherArrays.Length; otherArrayIndex++)
			{
				T[] otherArray = otherArrays[otherArrayIndex];
				Buffer.BlockCopy(otherArray, 0, newArray, combinedArrayIndex, otherArray.Length);
				combinedArrayIndex += otherArray.Length;
			}
			return newArray;
		}

		public static T[] Combine<T>(this IList<T[]> arrays)
		{
			int combinedSize = arrays.Sum(array => array.Length);
			T[] combinedArray = new T[combinedSize];
			int combinedArrayIndex = 0;
			for (int arraysIndex = 0; arraysIndex < arrays.Count; arraysIndex++)
			{
				T[] array = arrays[arraysIndex];
				Buffer.BlockCopy(array, 0, combinedArray, combinedArrayIndex, array.Length);
				combinedArrayIndex += array.Length;
			}
			return combinedArray;
		}
	}
}
