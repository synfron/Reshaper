using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ReshaperUI.Commands;

namespace ReshaperUI.Display.Xaml.Controls.Common
{
	/// <summary>
	/// Interaction logic for EditableList.xaml
	/// </summary>
	public partial class EditableList : UserControl
	{

		private ICommand _addCommand;
		private ICommand _deleteCommand;

		public readonly static DependencyProperty AllowEmptyInputProperty = DependencyProperty.Register(
			"AllowEmptyInput", typeof(bool), typeof(EditableList), new PropertyMetadata(false, new PropertyChangedCallback(OnAllowEmptyInputPropertyChanged)));
		public readonly static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource", typeof(object), typeof(EditableList), new PropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));
		public readonly static DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register(
			"ListBackground", typeof(Brush), typeof(EditableList), new PropertyMetadata(null, new PropertyChangedCallback(ListBackgroundPropertyChanged)));

		private static void ListBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as EditableList).ItemList.Background = e.NewValue as Brush;
		}

		private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as EditableList)?.UpdateItemsSource();
		}

		private static void OnAllowEmptyInputPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as EditableList).AllowEmptyInput = (bool?)e.NewValue == true;
		}

		public ObservableCollection<string> ItemsSource
		{
			get
			{
				return base.GetValue(ItemsSourceProperty) as ObservableCollection<string>;
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public Brush ListBackground
		{
			get
			{
				return base.GetValue(TextBoxBackgroundProperty) as Brush;
			}
			set
			{
				SetValue(TextBoxBackgroundProperty, value);
			}
		}


		public ICommand AddCommand
		{
			get
			{
				if (_addCommand == null)
				{
					_addCommand = new RelayCommand(
						() =>
						{
							ItemsSource.Add(Regex.Unescape(EnterBox.Text));
							EnterBox.Text = "";
						},
						() =>
						{
							return ItemsSource != null && (AllowEmptyInput || !string.IsNullOrEmpty(EnterBox.Text));
						});
				}
				return _addCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(
						() =>
						{
							ItemsSource.RemoveAt(ItemList.SelectedIndex);
						},
						() =>
						{
							return ItemList.SelectedIndex >= 0;
						});
				}
				return _deleteCommand;
			}
		}

		public bool AllowEmptyInput
		{
			get
			{
				return (bool)base.GetValue(AllowEmptyInputProperty);
			}
			set
			{
				SetValue(AllowEmptyInputProperty, value);
			}
		}

		public EditableList()
		{
			InitializeComponent();
		}

		public void UpdateItemsSource()
		{
			ItemList.ItemsSource = ItemsSource;
		}
	}
}
