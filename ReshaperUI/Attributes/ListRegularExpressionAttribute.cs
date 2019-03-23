using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ReshaperUI.Attributes
{
	public class ListRegularExpressionAttribute : RegularExpressionAttribute
	{

		public ListRegularExpressionAttribute(string pattern) : base(pattern)
		{

		}

		public override bool IsValid(object listValueObj)
		{
			bool isValid = false;
			ICollection<string> listValue = listValueObj as ICollection<string>;
			if (listValue != null)
			{
				isValid = listValue.All(value => base.IsValid(value));
			}
			return isValid;
		}
	}
}
