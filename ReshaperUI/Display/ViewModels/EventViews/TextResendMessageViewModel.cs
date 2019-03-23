using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReshaperUI.Commands;
using ReshaperUI.Converters;
using ReshaperCore.Messages;
using ReshaperCore.Rules;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class TextResendMessageViewModel
	{
		private RelayCommand _saveCommand;
		private EventInfo _eventInfo;

		public virtual event CloseRequestedHandler CloseRequested;
		public delegate void CloseRequestedHandler();

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(() =>
					{
						_eventInfo.ProxyConnection.AddData((DataDirection)(new EnumToStringConverter().ConvertBack(Direction, typeof(DataDirection))), _eventInfo.Message.TextEncoding.GetBytes(Text));
						if (CloseRequested != null)
						{
							CloseRequested();
						}
					});
				}
				return _saveCommand;
			}
		}

		public string Text
		{
			get;
			set;
		}

		public string Direction
		{
			get;
			set;
		}

		public IEnumerable<string> DataDirections
		{
			get
			{
				return (Enum.GetValues(typeof(DataDirection)) as DataDirection[]).Where(dataDirection => _eventInfo.ProxyConnection.HasConnection(_eventInfo.Direction)).Select(dataDirection => (string)(new EnumToStringConverter().Convert(dataDirection, typeof(DataDirection))));
			}
		}

		public TextResendMessageViewModel(EventInfo eventInfo)
		{
			this._eventInfo = eventInfo;
			DataDirection direction = (eventInfo.ProxyConnection.HasConnection(eventInfo.Direction) && DataDirection.Origin == eventInfo.Direction) ? eventInfo.Direction : DataDirection.Target;
			Direction = (string)(new EnumToStringConverter().Convert(direction, typeof(DataDirection)));
		}
	}
}
