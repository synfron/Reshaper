using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ReshaperUI.Attributes
{
	public class MinLengthDependentAttribute : MinLengthAttribute, IDependentAttribute
	{
		private string _jsonDependentPropertyValuePairs;

		public string DependentProperty
		{
			get;
			set;
		}

		public object DependentValue
		{
			get;
			set;
		}

		public Dictionary<string, object> DependentPropertyValuePairs
		{
			get;
			private set;
		}

		public string JsonDependentPropertyValuePairs
		{
			get
			{
				return _jsonDependentPropertyValuePairs;
			}
			set
			{
				_jsonDependentPropertyValuePairs = value;
				DependentPropertyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(_jsonDependentPropertyValuePairs);
			}
		}

		public MinLengthDependentAttribute(int minValue) : base(minValue)
		{

		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			return this.IsActive(validationContext.ObjectInstance) ? base.IsValid((value is ICollection) ? GetListAsArray(value) : value, validationContext) : ValidationResult.Success;
		}

		private object GetListAsArray(object listObj)
		{
			Type genericType = listObj.GetType().GetGenericArguments()[0];
			MethodInfo ToArrayMethod = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(genericType);
			return ToArrayMethod.Invoke(null, new[] { listObj });
		}
	}
}
