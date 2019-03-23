using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Vars;
using EventInfo = ReshaperCore.Rules.EventInfo;

namespace ReshaperScript.Core.Functions
{
	public class Event
	{
		private EventInfo eventInfo;

		public Event(EventInfo eventInfo)
		{
			this.eventInfo = eventInfo;
		}

		public string AddMessage(string direction, string messageText, bool insertAtBeginning)
		{
			ThenAddMessage then = new ThenAddMessage()
			{
				Direction = (DataDirection)GetStringAsEnum<DataDirection>(direction),
				MessageText = VariableString.GetAsVariableString(messageText, false),
				InsertAtBeginning = insertAtBeginning
			};
			return then.Perform(eventInfo).ToString();
		}

		public string Broadcast()
		{
			ThenBroadcast then = new ThenBroadcast();
			return then.Perform(eventInfo).ToString();
		}

		public string Connect(string destinationHost, int? destinationPort)
		{
			ThenConnect then = new ThenConnect()
			{
				DestinationHost = (destinationHost == null) ? null : VariableString.GetAsVariableString(destinationHost.ToString(), false),
				DestinationPort = (destinationPort == null) ? null : VariableString.GetAsVariableString(destinationPort.ToString(), false)
			};
			return then.Perform(eventInfo).ToString();
		}

		public string DelimitHttp()
		{
			ThenDelimitHttp then = new ThenDelimitHttp();
			return then.Perform(eventInfo).ToString();
		}

		public string DelimitText(EventInfo eventInfo)
		{
			ThenDelimitText then = new ThenDelimitText();
			return then.Perform(eventInfo).ToString();
		}

		public string HttpConnect(bool overrideCurrentConnection)
		{
			ThenHttpConnect then = new ThenHttpConnect()
			{
				OverrideCurrentConnection = overrideCurrentConnection
			};
			return then.Perform(eventInfo).ToString();
		}

		public string SendData()
		{
			ThenSendData then = new ThenSendData();
			return then.Perform(eventInfo).ToString();
		}

		public string GetMessageValue(EventInfo eventInfo, string messageValue, string messageValueType, string identifier)
		{
			MessageValueHandler handler = new MessageValueHandler();
			MessageValue rawMessageValue = (MessageValue)GetStringAsEnum<MessageValue>(messageValue);
			MessageValueType rawMessageValueType = (MessageValueType)GetStringAsEnum<MessageValueType>(messageValueType);
			VariableString rawIdentifier = (identifier == null) ? null : VariableString.GetAsVariableString(identifier.ToString(), false);
			return handler.GetValue(eventInfo, rawMessageValue, rawMessageValueType, rawIdentifier);
		}

		public void SetMessageValue(EventInfo eventInfo, string value, string messageValue, string messageValueType, string identifier)
		{
			MessageValueHandler handler = new MessageValueHandler();
			MessageValue rawMessageValue = (MessageValue)GetStringAsEnum<MessageValue>(messageValue);
			MessageValueType rawMessageValueType = (MessageValueType)GetStringAsEnum<MessageValueType>(messageValueType);
			VariableString rawIdentifier = (identifier == null) ? null : VariableString.GetAsVariableString(identifier, false);
			handler.SetValue(eventInfo, rawMessageValue, rawMessageValueType, rawIdentifier, value);
		}

		private static object GetStringAsEnum<T>(object value)
		{
			object enumValue = null;
			if (value != null)
			{
				Type targetType = typeof(T);
				FieldInfo fieldInfo = targetType.GetFields().FirstOrDefault(field => field.Name == value?.ToString() || field.GetCustomAttribute<DescriptionAttribute>()?.Description == value.ToString());
				if (fieldInfo != null)
				{
					if (!targetType.IsEnum)
					{
						targetType = targetType.GetGenericArguments().ElementAtOrDefault(0) ?? targetType;
					}
					enumValue = Enum.ToObject(targetType, (int)fieldInfo.GetValue(null));
				}
			}
			return enumValue;
		}
	}
}
