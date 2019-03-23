using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperCore.Rules.Thens;
using ReshaperUI.Attributes;
using ReshaperUI.Converters;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Connect"), Export(typeof(ThenViewModel<ThenConnect>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenConnectViewModel : ThenViewModel<ThenConnect>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _destinationHost;
		private string _destinationPort;
		private bool? _overrideCurrentConnection;

		[SourceModelProperty("DestinationHost", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string DestinationHost
		{
			get
			{
				return Flyweight.Get<string>(_destinationHost, Then.DestinationHost, new VariableStringToStringConverter());
			}
			set
			{
				this._destinationHost = value;
				this.OnPropertyChanged(nameof(DestinationHost));
			}
		}

		[SourceModelProperty("DestinationPort", ConverterType = typeof(VariableStringToStringConverter), UseConvertBack = true)]
		public string DestinationPort
		{
			get
			{
				return Flyweight.Get<string>(_destinationPort, Then.DestinationPort, new VariableStringToStringConverter());
			}
			set
			{
				this._destinationPort = value;
				this.OnPropertyChanged(nameof(DestinationPort));
			}
		}

		[SourceModelProperty("OverrideCurrentConnection")]
		public bool OverrideCurrentConnection
		{
			get
			{
				return Flyweight.Get<bool>(_overrideCurrentConnection, Then.OverrideCurrentConnection);
			}
			set
			{
				this._overrideCurrentConnection = value;
				this.OnPropertyChanged(nameof(OverrideCurrentConnection));
			}
		}
	}
}
