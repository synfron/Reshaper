using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ReshaperCore.Proxies;
using ReshaperCore.Rules.Whens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Whens
{
	[Description("Proxy Type"), Export(typeof(WhenViewModel<WhenProxyType>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class WhenProxyTypeViewModel : WhenViewModel<WhenProxyType>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _proxyType;

		[SourceModelProperty("ProxyType", ConverterType = typeof(EnumToStringConverter), UseConvertBack = true)]
		public string ProxyType
		{
			get
			{
				return Flyweight.Get<string>(_proxyType, When.ProxyType, new EnumToStringConverter());
			}
			set
			{
				this._proxyType = value;
				this.OnPropertyChanged(nameof(ProxyType));
			}
		}

		public IEnumerable<string> ProxyDataTypes
		{
			get
			{
				return (Enum.GetValues(typeof(ProxyDataType)) as ProxyDataType[]).Select(proxyDataType => (string)(new EnumToStringConverter().Convert(proxyDataType, typeof(ProxyDataType))));
			}
		}
	}
}
