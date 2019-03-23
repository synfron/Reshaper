using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReshaperUI.Attributes
{
	public class VisibleDependentAttribute : Attribute, IDependentAttribute
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
	}
}
