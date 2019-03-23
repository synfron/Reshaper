using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ReshaperCore.Utils;

namespace ReshaperCore.Vars
{
	public class Variables : INotifyCollectionChanged
	{
		private ConcurrentDictionary<String, IVariable> _variables = new ConcurrentDictionary<String, IVariable>(StringComparer.OrdinalIgnoreCase);

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public string PersistPath
		{
			get;
			set;
		}

		public IEnumerable<string> VariableNames
		{
			get
			{
				return _variables.Keys;
			}
		}

		public void SavePersistables()
		{
			if (!string.IsNullOrEmpty(PersistPath))
			{
				Dictionary<string, string> persistables = _variables.Where(variable => variable.Value.Persistent && variable.Value is IVariable<string>).ToDictionary(key => key.Key, value => (value.Value as IVariable<string>).Value);

				Serializer.SerializeToFile(PersistPath, persistables);
			}
		}

		public void LoadPersistables()
		{
			if (!string.IsNullOrEmpty(PersistPath))
			{
				Dictionary<string, string> persistables = Serializer.DeserializeFromFile<Dictionary<string, string>>(PersistPath);
				if (persistables != null)
				{
					foreach (KeyValuePair<string, string> pair in persistables)
					{
						IVariable<string> variable = Add<string>(pair.Key);
						variable.Value = pair.Value;
						variable.Persistent = true;
					}
				}
			}
		}

		public IVariable<T> Add<T>(String name)
		{
			IVariable<T> var = new Variable<T>();
			if (!_variables.TryAdd(name, var))
			{
				var = null;
			}
			else
			{
				OnCollectionChanged(NotifyCollectionChangedAction.Add, name);
			}
			return var;
		}

		private void OnCollectionChanged(NotifyCollectionChangedAction action, string changedItem = null)
		{
			if (CollectionChanged != null)
			{
				NotifyCollectionChangedEventArgs args = null;
				switch (action)
				{
					case NotifyCollectionChangedAction.Add:
					case NotifyCollectionChangedAction.Remove:
						args = new NotifyCollectionChangedEventArgs(action, changedItem);
						break;
					case NotifyCollectionChangedAction.Reset:
						args = new NotifyCollectionChangedEventArgs(action);
						break;
				}
				CollectionChanged(this, args);
			}
		}

		public IVariable<T> Get<T>(String name)
		{
			IVariable varObj = null;
			_variables.TryGetValue(name, out varObj);
			if (varObj == null)
			{
				throw new ArgumentOutOfRangeException("Variable does not exist.");
			}
			return (IVariable<T>)varObj;
		}

		public IVariable<T> GetOrDefault<T>(String name)
		{
			IVariable varObj = null;
			_variables.TryGetValue(name, out varObj);
			return varObj as IVariable<T>;
		}

		public bool Has<T>(String name)
		{
			return GetOrDefault<T>(name) != null;
		}

		public bool Remove(String name)
		{
			IVariable var = null;
			bool removed = false;
			if (_variables.TryRemove(name, out var))
			{
				OnCollectionChanged(NotifyCollectionChangedAction.Remove, name);
				removed = true;
			}
			return removed;
		}

		public void Clear()
		{
			_variables.Clear();
			OnCollectionChanged(NotifyCollectionChangedAction.Reset);
		}

		private class Variable<T> : IVariable<T>
		{

			private T _value;

			public event PropertyChangedEventHandler PropertyChanged;

			public bool Persistent
			{
				get;
				set;
			}

			public T Value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
					OnPropertyChanged(nameof(Value));
				}
			}

			public Variable()
			{
			}

			protected virtual void OnPropertyChanged(string propertyName)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
	}
}
