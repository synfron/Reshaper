using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReshaperCore.Rules.Whens
{
	public class WhenIsDelimited : When
	{

		public override bool IsMatch(EventInfo eventInfo)
		{
			return eventInfo.Message?.Complete ?? false;
		}
	}
}
