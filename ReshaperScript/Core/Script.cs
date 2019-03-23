namespace ReshaperScript.Core
{
	public class Script
	{
		private bool _isStaticScript;
		private string _text;

		public event ScriptChangedEventHandler ScriptChanged;
		public delegate void ScriptChangedEventHandler(Script script);

		public bool IsStaticScript
		{
			get
			{
				return _isStaticScript;
			}
			set
			{
				if (_isStaticScript != value)
				{
					_isStaticScript = value;
					OnScriptChanged();
				}
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					OnScriptChanged();
				}
			}
		}

		public string Name
		{
			get;
			set;
		}

		protected virtual void OnScriptChanged()
		{
			if (ScriptChanged != null)
			{
				ScriptChanged(this);
			}
		}
	}
}
