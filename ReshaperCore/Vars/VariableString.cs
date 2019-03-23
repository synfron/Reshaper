using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReshaperCore.Providers;

namespace ReshaperCore.Vars
{
	public class VariableString
	{
		private string _text;
		private Tuple<VariableSource, string>[] _variables;

        public ISelf Self { get; set; } = new SelfProvider().GetInstance();

        public VariableString(string text, params Tuple<VariableSource, string>[] variables)
		{
			_text = text;
			_variables = variables;
		}

		public virtual string GetFormattedString()
		{
			return string.Format(_text, _variables.Select(variable => $"{{{variable.Item1.ToString().ToLower()}:{variable.Item2}}}").ToArray());
		}

		public static VariableString GetAsVariableString(string str, bool requiresParsing = true)
		{
			str = str.Replace(@"\", @"\\").Replace("{{", @"\{").Replace("}}", @"\}");
			if (requiresParsing)
			{
				bool inEscape = false;
				bool inVar = false;
				int subStringStartIndex = 0;
				int varIndex = 0;
				List<Tuple<VariableSource, string>> variables = new List<Tuple<VariableSource, string>>();
				StringBuilder textBuilder = new StringBuilder();
				for (int charIndex = 0; charIndex < str.Length; charIndex++)
				{
					if (str[charIndex] == '\\')
					{
						if (inEscape)
						{
							int subStringSize = charIndex - subStringStartIndex - 1;
							if (subStringSize > 0)
							{
								textBuilder.Append(str.Substring(subStringStartIndex, subStringSize));
							}
							subStringStartIndex = charIndex + 1;
							inEscape = false;
						}
						else
						{
							inEscape = true;
						}
					}
					else if (inEscape)
					{
						int subStringSize = charIndex - subStringStartIndex - 1;
						if (subStringSize > 0)
						{
							textBuilder.Append(str.Substring(subStringStartIndex, subStringSize).Replace("{", "{{").Replace("}", "}}"));
						}
						subStringStartIndex = charIndex;
						inEscape = false;
					}
					else
					{
						if (str[charIndex] == '{')
						{
							int subStringSize = charIndex - subStringStartIndex - 1;
							if (subStringSize > 0)
							{
								textBuilder.Append(str.Substring(subStringStartIndex, subStringSize));
							}
							subStringStartIndex = charIndex + 1;
							inVar = true;
						}
						else if (str[charIndex] == '}')
						{
							if (!inVar)
							{
								throw new FormatException("'}' found at invalid location.");
							}
							else
							{
								int subStringSize = charIndex - subStringStartIndex;
								string varDataStr = str.Substring(subStringStartIndex, subStringSize);
								if (!string.IsNullOrEmpty(varDataStr))
								{
									string[] varData = varDataStr.Split(new[] { ':' }, 2);
									string varName;
									VariableSource source = VariableSource.Global;
									if (varData.Length > 1)
									{
										if (!Enum.TryParse(varData[0], true, out source))
										{
											throw new FormatException("Unknown variable source.");
										}
										varName = varData[1];
									}
									else
									{
										varName = varData[0];
									}
									variables.Add(new Tuple<VariableSource, string>(source, varName));
									textBuilder.Append($"{{{varIndex++}}}");
									subStringStartIndex = charIndex + 1;
									inVar = false;
								}
								else
								{
									throw new FormatException("Variable string name cannot be empty.");
								}
							}
						}
					}
				}
				if (inVar)
				{
					throw new FormatException("Invalid variable string format.");
				}
				else
				{
					int subStringSize = str.Length - subStringStartIndex;
					if (subStringSize > 0)
					{
						textBuilder.Append(str.Substring(subStringStartIndex, subStringSize).Replace("{", "{{").Replace("}", "}}"));
					}
				}
				return new VariableString(textBuilder.ToString(), variables.ToArray());
			}
			else
			{
				return new VariableString(str, new Tuple<VariableSource, string>[0]);
			}
		}

		public virtual int? GetInt(Variables connectionVariables)
		{
			string text = GetText(connectionVariables);
			int? nullableValue = null;
			int value;
			if (int.TryParse(text, out value))
			{
				nullableValue = value;
			}
			return nullableValue;
		}

		public virtual string GetText(Variables connectionVariables)
		{
			List<string> variableVals = new List<string>();
			foreach (Tuple<VariableSource, string> variable in _variables)
			{
				string value = null;
				switch (variable.Item1)
				{
					case VariableSource.Global:
						value = Self.Variables.GetOrDefault<string>(variable.Item2)?.Value ?? string.Empty;
						break;
					case VariableSource.Channel:
						value = connectionVariables?.GetOrDefault<string>(variable.Item2)?.Value ?? string.Empty;
						break;
					default:
						value = string.Empty;
						break;
				}
				variableVals.Add(value);
			}
			return string.Format(_text, variableVals.ToArray());
		}
	}
}
