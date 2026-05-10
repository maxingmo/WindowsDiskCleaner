using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowsDiskCleaner.App
{
    public sealed class MainWindow : Window
    {
        public MainWindow()
        {
            Title = "File Scanner P3 - Risk Levels";
            Width = 1240;
            Height = 720;
            MinWidth = 980;
            MinHeight = 520;
            DataContext = new MainWindowViewModel();
            Content = BuildContent();
        }

        private static UIElement BuildContent()
        {
            var root = new Grid
            {
                Margin = new Thickness(16)
            };

            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = new TextBlock
            {
                Text = "\u78c1\u76d8\u6587\u4ef6\u626b\u63cf",
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 12)
            };
            Grid.SetRow(title, 0);
            root.Children.Add(title);

            var toolbar = new Grid
            {
                Margin = new Thickness(0, 0, 0, 12)
            };
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(96) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(96) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(96) });
            Grid.SetRow(toolbar, 1);
            root.Children.Add(toolbar);

            var pathBox = new TextBox
            {
                Height = 32,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            pathBox.SetBinding(TextBox.TextProperty, new Binding("RootPath")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(pathBox, 0);
            toolbar.Children.Add(pathBox);

            AddToolbarButton(toolbar, "\u9009\u62e9\u76ee\u5f55", "BrowseCommand", 1);
            AddToolbarButton(toolbar, "\u5f00\u59cb\u626b\u63cf", "ScanCommand", 2);
            AddToolbarButton(toolbar, "\u53d6\u6d88", "CancelCommand", 3);

            var filters = BuildFilterBar();
            Grid.SetRow(filters, 2);
            root.Children.Add(filters);

            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                IsReadOnly = true,
                EnableRowVirtualization = true,
                EnableColumnVirtualization = true,
                GridLinesVisibility = DataGridGridLinesVisibility.Horizontal
            };
            grid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Files"));
            grid.Columns.Add(new DataGridTextColumn { Header = "\u6587\u4ef6\u540d", Binding = new Binding("Name"), Width = 190 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u6587\u4ef6\u7ea7\u522b", Binding = new Binding("RiskLevelDisplay"), Width = 120 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u5927\u5c0f", Binding = new Binding("SizeDisplay"), SortMemberPath = "SizeBytes", Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u6269\u5c55\u540d", Binding = new Binding("Extension"), Width = 90 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u521b\u5efa\u65f6\u95f4", Binding = new Binding("CreatedAt"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u4fee\u6539\u65f6\u95f4", Binding = new Binding("ModifiedAt"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u5b8c\u6574\u8def\u5f84", Binding = new Binding("FullPath"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            Grid.SetRow(grid, 3);
            root.Children.Add(grid);

            var statusBorder = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(215, 220, 226)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 12, 0, 0)
            };
            var status = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap
            };
            status.SetBinding(TextBlock.TextProperty, new Binding("StatusText"));
            statusBorder.Child = status;
            Grid.SetRow(statusBorder, 4);
            root.Children.Add(statusBorder);

            return root;
        }

        private static UIElement BuildFilterBar()
        {
            var panel = new Grid
            {
                Margin = new Thickness(0, 0, 0, 12)
            };

            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(96) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            AddLabeledTextBox(panel, "\u5173\u952e\u8bcd", "KeywordFilter", 0);
            AddLabeledTextBox(panel, "\u6269\u5c55\u540d", "ExtensionFilter", 1);
            AddLabeledTextBox(panel, "\u6700\u5c0f MB", "MinimumSizeMbFilter", 2);

            var date = new DatePicker
            {
                Margin = new Thickness(8, 18, 0, 0)
            };
            date.SetBinding(DatePicker.SelectedDateProperty, new Binding("ModifiedBeforeFilter")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(date, 3);
            panel.Children.Add(date);
            AddLabel(panel, "\u65e9\u4e8e\u4fee\u6539\u65e5", 3);

            var quick = new ComboBox
            {
                Margin = new Thickness(8, 18, 0, 0),
                DisplayMemberPath = "Label"
            };
            quick.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("QuickFilters"));
            quick.SetBinding(Selector.SelectedItemProperty, new Binding("SelectedQuickFilter")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(quick, 4);
            panel.Children.Add(quick);
            AddLabel(panel, "\u5feb\u6377\u7b5b\u9009", 4);

            var clear = new Button
            {
                Content = "\u6e05\u9664\u7b5b\u9009",
                Margin = new Thickness(8, 18, 0, 0)
            };
            clear.SetBinding(Button.CommandProperty, new Binding("ClearFiltersCommand"));
            Grid.SetColumn(clear, 5);
            panel.Children.Add(clear);

            return panel;
        }

        private static void AddLabeledTextBox(Grid panel, string label, string bindingPath, int column)
        {
            AddLabel(panel, label, column);

            var textBox = new TextBox
            {
                Height = 28,
                Margin = column == 0 ? new Thickness(0, 18, 0, 0) : new Thickness(8, 18, 0, 0),
                VerticalContentAlignment = VerticalAlignment.Center
            };
            textBox.SetBinding(TextBox.TextProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Delay = 400
            });
            Grid.SetColumn(textBox, column);
            panel.Children.Add(textBox);
        }

        private static void AddLabel(Grid panel, string text, int column)
        {
            var label = new TextBlock
            {
                Text = text,
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(88, 96, 105)),
                Margin = column == 0 ? new Thickness(0, 0, 0, 0) : new Thickness(8, 0, 0, 0)
            };
            Grid.SetColumn(label, column);
            panel.Children.Add(label);
        }

        private static void AddToolbarButton(Grid toolbar, string text, string commandPath, int column)
        {
            var button = new Button
            {
                Content = text,
                Margin = new Thickness(8, 0, 0, 0)
            };
            button.SetBinding(Button.CommandProperty, new Binding(commandPath));
            Grid.SetColumn(button, column);
            toolbar.Children.Add(button);
        }
    }
}

