using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReshaperUI.Attributes
{
	public class ListRegularExpressionDependentAttribute : ListRegularExpressionAttribute, IDependentAttribute
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

		public ListRegularExpressionDependentAttribute(string pattern) : base(pattern)
		{

		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			return this.IsActive(validationContext.ObjectInstance) ? base.IsValid(value, validationContext) : ValidationResult.Success;
		}
	}
}
