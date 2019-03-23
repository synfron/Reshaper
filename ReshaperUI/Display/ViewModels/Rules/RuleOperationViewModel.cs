using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Providers;
using ReshaperUI.Converters;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Base;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public abstract class RuleOperationViewModel : SourceModelViewModel
	{
		private IEnumerable<string> _settableMessageValues;

		protected virtual RuleViewModel RuleModel
		{
			private set;
			get;
		}

		protected override string PartialDisplayName
		{
			get
			{
				return GetType().GetCustomAttribute<DescriptionAttribute>()?.Description;
			}
		}

		public IEnumerable<string> MessageValues
		{
			get
			{
				if (_settableMessageValues == null)
				{
					IEnumerable<MessageValue> allowedMessageValues = (Enum.GetValues(typeof(MessageValue)) as MessageValue[]).AsEnumerable();
					if (RuleModel is TextRuleViewModel)
					{
						allowedMessageValues = allowedMessageValues.Except(new[] { MessageValue.HttpBody, MessageValue.HttpHeader, MessageValue.HttpHeaders, MessageValue.HttpMethod, MessageValue.HttpRequestUri, MessageValue.HttpStatusCode, MessageValue.HttpStatusLine, MessageValue.HttpStatusMessage, MessageValue.HttpVersion });
					}
					_settableMessageValues = allowedMessageValues.Select(packeValue => (string)(new EnumToStringConverter().Convert(packeValue, typeof(MessageValue))));
				}
				return _settableMessageValues;
			}
		}

		public IModelPresenter Presenter
		{
			get
			{
				IModelPresenter presenter = Container.GetExports(typeof(IModelPresenter<>).MakeGenericType(this.GetType()), null, null).FirstOrDefault()?.Value as IModelPresenter;
				presenter?.SetModel(this);
				return presenter;
			}
        }

        public CompositionContainer Container { get; set; } = new CompositionContainerProvider().GetInstance();

        public abstract object GetModel();

		public void SetRuleModel(RuleViewModel ruleModel)
		{
			this.RuleModel = ruleModel;
		}
	}
}
