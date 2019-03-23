using System;
using ReshaperCore.Utils;

namespace ReshaperCore.Messages
{
	public abstract class EntityContainer : ObservableEntity
	{
		private static int _maxFlag = 0;

		protected static long RegisterFlag()
		{
			long entityFlag = (long)Math.Pow(2, _maxFlag);
			_maxFlag++;
			return entityFlag;
		}

		public abstract long GetEntityFlag();
	}
}
