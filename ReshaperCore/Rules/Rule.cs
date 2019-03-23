using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Rules.Whens;

namespace ReshaperCore.Rules
{
	public class Rule
	{
		public ObservableCollection<When> Whens { get; set; }

		public ObservableCollection<Then> Thens { get; set; }

		public bool Enabled { get; set; }

		public String Name { get; set; }

		public bool Locked
		{
			get;
			set;
		}

		public RunPosition Placement
		{
			get;
			set;
		} = RunPosition.Undefined;

		public Rule()
		{
			Whens = new ObservableCollection<When>();
			Thens = new ObservableCollection<Then>();
			Enabled = true;
		}

		public override string ToString()
		{
			return Name ?? "";
		}
	}
}
