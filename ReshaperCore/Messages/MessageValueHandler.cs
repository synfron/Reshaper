using System;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Messages.Parsers;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperCore.Settings;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Messages
{
	public class MessageValueHandler
	{
		private readonly HttpHeadersParser _httpHeadersParser;
		private readonly HttpMessageParser _httpMessageParser;
		private readonly HttpStatusLineParser _httpStatusLineParser;

		public MessageValueHandler()
		{
			_httpHeadersParser = new HttpHeadersParser();
			_httpStatusLineParser = new HttpStatusLineParser();
			_httpMessageParser = new HttpMessageParser();
		}

        public IGeneralSettings GeneralSettings { get; set; } = new GeneralSettingsProvider().GetInstance();

        public string GetValue(EventInfo eventInfo, MessageValue messageValue, MessageValueType valueType, VariableString identifier = null)
		{
			_httpMessageParser.TextEncoding = eventInfo.ProxyConnection.ProxyInfo.DefaultEncoding;

			string value = null;
			switch (messageValue)
			{
				case MessageValue.DataDirection:
					value = eventInfo.Direction.ToString();
					break;
				case MessageValue.HttpBody:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							value = httpMessage.Body.ToString();
							if (valueType != MessageValueType.Text && identifier != null)
							{
								switch (valueType)
								{
									case MessageValueType.Json:
										value = value?.GetJsonValue(identifier.GetText(eventInfo.Variables));
										break;
									case MessageValueType.Xml:
										value = value?.GetXmlValue(identifier.GetText(eventInfo.Variables));
										break;
								}
							}
						}
					}
					break;
				case MessageValue.HttpHeader:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null && identifier != null)
						{
							value = httpMessage.Headers.GetOrDefault(identifier.GetText(eventInfo.Variables)) ?? string.Empty;
						}
					}
					break;
				case MessageValue.HttpHeaders:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							value = httpMessage.Headers.ToString();
						}
					}
					break;
				case MessageValue.HttpMethod:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							value = (httpMessage.StatusLine as HttpRequestStatusLine)?.Method;
						}
					}
					break;
				case MessageValue.HttpRequestUri:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							value = (httpMessage.StatusLine as HttpRequestStatusLine)?.Uri;
						}
					}
					break;
				case MessageValue.LocalAddress:
					value = eventInfo.ProxyConnection.OriginChannel.LocalEndpoint.Address.ToString();
					break;
				case MessageValue.LocalPort:
					value = eventInfo.ProxyConnection.OriginChannel.LocalEndpoint.Port.ToString();
					break;
				case MessageValue.SourceRemoteAddress:
					if (eventInfo.ProxyConnection.HasOriginConnection)
					{
						value = eventInfo.ProxyConnection.OriginChannel.RemoteEndpoint.Address.ToString();
					}
					break;
				case MessageValue.SourceRemotePort:
					value = eventInfo.ProxyConnection.OriginChannel.RemoteEndpoint.Port.ToString();

					break;
				case MessageValue.DestinationRemoteAddress:
					if (eventInfo.ProxyConnection.HasTargetConnection)
					{
						value = eventInfo.ProxyConnection.TargetChannel.RemoteEndpoint.Address.ToString();
					}
					break;
				case MessageValue.DestinationRemotePort:
					if (eventInfo.ProxyConnection.HasTargetConnection)
					{
						value = eventInfo.ProxyConnection.TargetChannel.RemoteEndpoint.Port.ToString();
					}
					break;
				case MessageValue.Protocol:
					value = eventInfo.Message?.Protocol?.ToString();
					break;
				case MessageValue.HttpVersion:
					{
						HttpStatusLine requestStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine;
						if (requestStatusLine is HttpRequestStatusLine)
						{
							value = (requestStatusLine as HttpRequestStatusLine)?.Version;
						}
						else
						{
							value = (requestStatusLine as HttpResponseStatusLine)?.Version;
						}
					}
					break;
				case MessageValue.HttpStatusLine:
					value = (eventInfo.Message as HttpMessage)?.StatusLine?.ToString();
					break;
				case MessageValue.HttpStatusCode:
					value = ((eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine)?.StatusCode.ToString();
					break;
				case MessageValue.HttpStatusMessage:
					value = ((eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine)?.StatusMessage.ToString();
					break;
				case MessageValue.Message:
					value = eventInfo.Message?.ToString();
					if (valueType != MessageValueType.Text && identifier != null)
					{
						switch (valueType)
						{
							case MessageValueType.Json:
								value = value?.GetJsonValue(identifier.GetText(eventInfo.Variables));
								break;
							case MessageValueType.Xml:
								value = value?.GetXmlValue(identifier.GetText(eventInfo.Variables));
								break;
						}
					}
					break;
			}
			return value ?? string.Empty;
		}

		public void SetValue(EventInfo eventInfo, MessageValue messageValue, MessageValueType valueType, VariableString identifier, string replacementText)
		{
			switch (messageValue)
			{
				case MessageValue.DataDirection:
					{
						DataDirection dataDirection;
						if (Enum.TryParse(replacementText, out dataDirection))
						{
							eventInfo.Direction = dataDirection;
						}
					}
					break;
				case MessageValue.HttpBody:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							if (valueType != MessageValueType.Text && identifier != null)
							{
								switch (valueType)
								{
									case MessageValueType.Json:
										replacementText = httpMessage.Body.Text.SetJsonValue(identifier.GetText(eventInfo.Variables), replacementText);
										break;
									case MessageValueType.Xml:
										replacementText = httpMessage.Body.Text.SetXmlValue(identifier.GetText(eventInfo.Variables), replacementText);
										break;
								}
							}
							httpMessage.Body = new HttpBody() { Text = replacementText };
							if (GeneralSettings.AutoUpdateContentLength)
							{
								if (httpMessage.Headers.Contains("Content-Length"))
								{
									httpMessage.Headers["Content-Length"] = httpMessage.Body.Text.Length.ToString();
								}
							}
						}
					}
					break;
				case MessageValue.HttpHeader:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null && identifier != null)
						{
							httpMessage.Headers[identifier.GetText(eventInfo.Variables)] = replacementText;
						}
					}
					break;
				case MessageValue.HttpHeaders:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							_httpHeadersParser.Parse(eventInfo, replacementText, true);
						}
					}
					break;
				case MessageValue.HttpStatusLine:
					{
						HttpMessage httpMessage = eventInfo.Message as HttpMessage;
						if (httpMessage != null)
						{
							HttpStatusLine newHttpStatusLine = _httpStatusLineParser.Parse(replacementText);
							httpMessage.StatusLine = newHttpStatusLine;
						}
					}
					break;
				case MessageValue.HttpMethod:
					{
						HttpRequestStatusLine httpStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine;
						if (httpStatusLine != null)
						{
							httpStatusLine.Method = replacementText;
						}
					}
					break;
				case MessageValue.HttpRequestUri:
					{
						HttpRequestStatusLine httpStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine;
						if (httpStatusLine != null)
						{
							httpStatusLine.Uri = replacementText;
						}
					}
					break;
				case MessageValue.HttpStatusMessage:
					{
						HttpResponseStatusLine httpStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine;
						if (httpStatusLine != null)
						{
							httpStatusLine.StatusMessage = replacementText;
						}
					}
					break;
				case MessageValue.HttpVersion:
					{
						HttpStatusLine httpStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine;
						if (httpStatusLine != null)
						{
							httpStatusLine.Version = replacementText;
						}
					}
					break;
				case MessageValue.HttpStatusCode:
					{
						HttpResponseStatusLine httpStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine;
						if (httpStatusLine != null)
						{
							int replacementVal;
							if (int.TryParse(replacementText, out replacementVal))
							{
								httpStatusLine.StatusCode = replacementVal;
							}
						}
					}
					break;
				case MessageValue.Message:
					{
						if (eventInfo.Message is HttpMessage)
						{
							HttpMessage newMessage = _httpMessageParser.Parse(replacementText, true).Item2;
							if (newMessage != null)
							{
								eventInfo.Message = newMessage;
							}
						}
						else if (eventInfo.Message != null)
						{
							eventInfo.Message.ResetCheckedEntity(HttpMessage.EntityFlag);
							if (valueType != MessageValueType.Text && identifier != null)
							{
								switch (valueType)
								{
									case MessageValueType.Json:
										replacementText = eventInfo.Message.ToString().SetJsonValue(identifier.GetText(eventInfo.Variables), replacementText);
										break;
									case MessageValueType.Xml:
										replacementText = eventInfo.Message.ToString().SetXmlValue(identifier.GetText(eventInfo.Variables), replacementText);
										break;
								}
							}
							eventInfo.Message.RawText = replacementText + eventInfo.Message.Delimiter;
						}
					}
					break;
			}
		}
	}
}
