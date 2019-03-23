using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.Xaml.Controls.EventView
{
	/// <summary>
	/// Interaction logic for TextEventViewControl.xaml
	/// </summary>
	public partial class TextEventViewControl : UserControl, IModelPresenter<TextEventListViewModel>
	{
		private ScrollViewer _dataGridScrollViewer;
		private ScrollBar _dataGridScrollBar;
		private bool _userScrolling;
		private bool _autoScroll = true;

		private ScrollViewer DataGridScrollViewer
		{
			get
			{
				if (_dataGridScrollViewer == null)
				{
					Border border = (Border)VisualTreeHelper.GetChild(EventList, 0);
					_dataGridScrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
				}
				return _dataGridScrollViewer;
			}
		}

		private ScrollBar DataGridScrollBar
		{
			get
			{
				if (_dataGridScrollBar == null)
				{
					DataGridScrollViewer.ApplyTemplate();
					_dataGridScrollBar = DataGridScrollViewer.Template.FindName("PART_VerticalScrollBar", DataGridScrollViewer) as ScrollBar;
				}
				return _dataGridScrollBar;
			}
		}

		public TextEventViewControl()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			DataGridScrollViewer.ScrollChanged += OnEventListScrollChanged;
			DataGridScrollBar.Scroll += OnEventListScrollBarMoved;
		}

		private void OnEventListScrollBarMoved(object sender, ScrollEventArgs e)
		{
			if (e.ScrollEventType != ScrollEventType.EndScroll)
			{
				_userScrolling = true;
			}
			else
			{
				_userScrolling = false;
				_autoScroll = DataGridScrollBar.Maximum - e.NewValue <= 10;
			}
		}

		private void OnEventListScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (!_userScrolling && _autoScroll)
			{
				DataGridScrollViewer.ScrollToEnd();
			}
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
