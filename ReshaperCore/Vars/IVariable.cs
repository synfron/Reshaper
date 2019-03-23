using System.ComponentModel;

namespace ReshaperCore.Vars
{
	public interface IVariable<T> : IVariable, INotifyPropertyChanged
	{
		T Value
		{
			get;
			set;
		}
	}

	public interface IVariable
	{
		bool Persistent
		{
			get;
			set;
		}
	}
}
