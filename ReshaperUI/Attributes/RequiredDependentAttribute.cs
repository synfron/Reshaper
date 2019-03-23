using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReshaperUI.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class RequiredDependentAttribute : RequiredAttribute, IDependentAttribute
	{
		private string _jsonDependentPropertyValuePairs;

		public Dictionary<string, object> DependentPropertyValuePairs
		{
			private set;
			get;
		}

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

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			return (this.IsActive(validationContext.ObjectInstance)) ? base.IsValid(value, validationContext) : ValidationResult.Success;
		}
	}
}
