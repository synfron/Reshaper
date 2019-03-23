namespace ReshaperCore.Utils
{
	public class Subject
	{
		private Observer _observer;

		protected object SubjectKey
		{
			private set;
			get;
		} = new object();

		protected bool SubjectWorking
		{
			get;
			set;
		} = false;

		public void Notify()
		{
			if (_observer != null)
			{
				_observer.OnUpdate();
			}
		}

		public void SetObserver(Observer observer)
		{
			this._observer = observer;
		}

		public void RemoveObserver()
		{
			this._observer = null;
		}
	}
}
