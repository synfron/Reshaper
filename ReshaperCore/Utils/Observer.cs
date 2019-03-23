using System;

namespace ReshaperCore.Utils
{
	public abstract class Observer : IDisposable
	{

		protected object ObserverKey
		{
			get;
			private set;
		} = new object();

		protected bool ObserverWorking
		{
			get;
			set;
		} = false;

		public abstract void OnUpdate();

		public void Dispose()
		{
		}
	}
}
