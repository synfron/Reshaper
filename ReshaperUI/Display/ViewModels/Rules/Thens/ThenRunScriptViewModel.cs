using System.ComponentModel;
using System.ComponentModel.Composition;
using ReshaperScript.Core;
using ReshaperUI.Attributes;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules.Thens
{
	[Description("Run Script"), Export(typeof(ThenViewModel<ThenRunScript>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public class ThenRunScriptViewModel : ThenViewModel<ThenRunScript>, IHttpRuleOperationModel, ITextRuleOperationModel
	{
		private string _scriptName;
		private string _scriptText;
		private bool? _useNamedScript;

		[SourceModelProperty("ScriptName")]
		[RequiredDependent(ErrorMessage = "'Script Name' is required.", DependentProperty = "UseNamedScript", DependentValue = true)]
		public string ScriptName
		{
			get
			{
				return Flyweight.Get<string>(_scriptName, Then.ScriptName);
			}
			set
			{
				_scriptName = value;
				OnPropertyChanged(nameof(ScriptName));
			}
		}

		[SourceModelProperty("ScriptText")]
		[RequiredDependent(ErrorMessage = "'Script Text' is required.", DependentProperty = "UseNamedScript", DependentValue = false)]
		public string ScriptText
		{
			get
			{
				return Flyweight.Get<string>(_scriptText, Then.ScriptText);
			}
			set
			{
				this._scriptText = value;
				this.OnPropertyChanged(nameof(ScriptText));
			}
		}

		[SourceModelProperty("UseNamedScript")]
		public bool UseNamedScript
		{
			get
			{
				return Flyweight.Get<bool>(_useNamedScript, Then.UseNamedScript);
			}
			set
			{
				this._useNamedScript = value;
				this.OnPropertyChanged(nameof(UseNamedScript));
			}
		}
	}
}
