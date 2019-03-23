using System.ComponentModel;

namespace ReshaperCore.Utils
{
	public class ObservableEntity
	{

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void RegisterOnEntityChanges<T>(string propertyName, T newValue, T oldValue) where T : ObservableEntity
		{
			PropertyChangedEventHandler entityChangedEvent = (sender, e) =>
			{
				OnPropertyChanged(propertyName);
			};
			if (newValue != null)
			{
				newValue.PropertyChanged += entityChangedEvent;
			}
			if (oldValue != null)
			{
				oldValue.PropertyChanged -= entityChangedEvent;
			}
		}
	}
}
