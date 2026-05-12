using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace WindowsDiskCleaner.App
{
    public sealed class MainWindow : Window
    {
        private static readonly HexBrushConverter HexBrushConverter = new HexBrushConverter();

        public MainWindow()
        {
            Title = "File Scanner P9A - UI Layout";
            Width = 1320;
            Height = 780;
            MinWidth = 1080;
            MinHeight = 620;
            Background = CreateBrush(246, 248, 251);
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
            root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var topSection = BuildTopSection();
            Grid.SetRow(topSection, 0);
            root.Children.Add(topSection);

            var filters = BuildFilterSection();
            Grid.SetRow(filters, 1);
            root.Children.Add(filters);

            var results = BuildResultsSection();
            Grid.SetRow(results, 2);
            root.Children.Add(results);

            var details = BuildDetailsSection();
            Grid.SetRow(details, 3);
            root.Children.Add(details);

            return root;
        }

        private static UIElement BuildTopSection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var panel = new Grid();
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = new TextBlock
            {
                Text = "\u78c1\u76d8\u6587\u4ef6\u626b\u63cf",
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                Foreground = CreateBrush(17, 24, 39),
                Margin = new Thickness(0, 0, 0, 12)
            };
            Grid.SetRow(title, 0);
            panel.Children.Add(title);

            var toolbar = new Grid();
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var pathLabel = CreateSmallLabel("\u626b\u63cf\u76ee\u5f55");
            pathLabel.VerticalAlignment = VerticalAlignment.Center;
            pathLabel.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(pathLabel, 0);
            toolbar.Children.Add(pathLabel);

            var pathBox = new TextBox
            {
                Height = 32,
                MinWidth = 320,
                Padding = new Thickness(8, 0, 8, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = CreateBrush(203, 213, 225),
                BorderThickness = new Thickness(1)
            };
            pathBox.SetBinding(TextBox.TextProperty, new Binding("RootPath")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(pathBox, 1);
            toolbar.Children.Add(pathBox);

            AddToolbarButton(toolbar, "\u9009\u62e9\u76ee\u5f55", "BrowseCommand", 2, ButtonTone.Neutral);
            AddToolbarButton(toolbar, "\u5f00\u59cb\u626b\u63cf", "ScanCommand", 3, ButtonTone.Primary);
            AddToolbarButton(toolbar, "\u53d6\u6d88", "CancelCommand", 4, ButtonTone.Neutral);
            AddToolbarButton(toolbar, "\u5220\u9664\u6587\u4ef6", "DeleteSelectedCommand", 5, ButtonTone.Danger);

            Grid.SetRow(toolbar, 1);
            panel.Children.Add(toolbar);

            section.Child = panel;
            return section;
        }

        private static UIElement BuildFilterSection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var panel = new Grid();
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = CreateSectionTitle("\u7b5b\u9009\u6761\u4ef6");
            Grid.SetRow(title, 0);
            panel.Children.Add(title);

            var filters = new Grid
            {
                Margin = new Thickness(0, 8, 0, 0)
            };

            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(155) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(175) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(155) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            AddLabeledTextBox(filters, "\u5173\u952e\u8bcd", "KeywordFilter", 0);
            AddLabeledTextBox(filters, "\u6269\u5c55\u540d", "ExtensionFilter", 1);
            AddLabeledTextBox(filters, "\u6700\u5c0f MB", "MinimumSizeMbFilter", 2);
            AddLabeledDatePicker(filters, "\u65e9\u4e8e\u4fee\u6539\u65e5", "ModifiedBeforeFilter", 3);
            AddLabeledComboBox(filters, "\u5feb\u6377\u7b5b\u9009", "QuickFilters", "SelectedQuickFilter", 4);
            AddLabeledComboBox(filters, "\u6587\u4ef6\u7ea7\u522b", "RiskFilters", "SelectedRiskFilter", 5);

            var clear = CreateCommandButton("\u6e05\u9664\u7b5b\u9009", "ClearFiltersCommand", ButtonTone.Neutral, 96);
            clear.Margin = new Thickness(8, 18, 0, 0);
            Grid.SetColumn(clear, 6);
            filters.Children.Add(clear);

            Grid.SetRow(filters, 1);
            panel.Children.Add(filters);

            section.Child = panel;
            return section;
        }

        private static UIElement BuildResultsSection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var panel = new Grid();
            panel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var commandBar = BuildResultCommandBar();
            Grid.SetRow(commandBar, 0);
            panel.Children.Add(commandBar);

            var resultsHost = new Grid
            {
                Margin = new Thickness(0, 10, 0, 0)
            };
            Grid.SetRow(resultsHost, 1);
            panel.Children.Add(resultsHost);

            var grid = BuildFileGrid();
            resultsHost.Children.Add(grid);

            var tree = BuildFolderTree();
            resultsHost.Children.Add(tree);

            section.Child = panel;
            return section;
        }

        private static UIElement BuildResultCommandBar()
        {
            var bar = new Grid();
            bar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            bar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var viewModes = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            viewModes.Children.Add(CreateSmallLabel("\u7ed3\u679c\u89c6\u56fe"));
            viewModes.Children.Add(CreateViewModeRadioButton("\u5217\u8868\u89c6\u56fe", "IsListView"));
            viewModes.Children.Add(CreateViewModeRadioButton("\u6587\u4ef6\u5939\u89c6\u56fe", "IsFolderView"));
            Grid.SetColumn(viewModes, 0);
            bar.Children.Add(viewModes);

            var batchTools = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };
            batchTools.SetBinding(UIElement.VisibilityProperty, new Binding("IsListView")
            {
                Converter = new BooleanToVisibilityConverter()
            });

            batchTools.Children.Add(CreateCommandButton("\u5168\u9009\u5f53\u524d\u7ed3\u679c", "SelectAllVisibleCommand", ButtonTone.Neutral, 118));
            batchTools.Children.Add(CreateCommandButton("\u6e05\u7a7a\u9009\u62e9", "ClearSelectionCommand", ButtonTone.Neutral, 96));
            batchTools.Children.Add(CreateCommandButton("\u6279\u91cf\u5220\u9664", "DeleteCheckedCommand", ButtonTone.Danger, 96));

            var summary = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = CreateBrush(75, 85, 99),
                Margin = new Thickness(8, 0, 0, 0)
            };
            summary.SetBinding(TextBlock.TextProperty, new Binding("CheckedFileSummary"));
            batchTools.Children.Add(summary);

            Grid.SetColumn(batchTools, 1);
            bar.Children.Add(batchTools);

            return bar;
        }

        private static DataGrid BuildFileGrid()
        {
            var grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                IsReadOnly = true,
                EnableRowVirtualization = true,
                EnableColumnVirtualization = true,
                GridLinesVisibility = DataGridGridLinesVisibility.Horizontal,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                RowHeight = 30,
                ColumnHeaderHeight = 32,
                Background = Brushes.White,
                BorderBrush = CreateBrush(203, 213, 225),
                BorderThickness = new Thickness(1),
                HorizontalGridLinesBrush = CreateBrush(226, 232, 240),
                AlternatingRowBackground = CreateBrush(248, 250, 252),
                SelectionMode = DataGridSelectionMode.Single,
                SelectionUnit = DataGridSelectionUnit.FullRow
            };
            grid.SetBinding(UIElement.VisibilityProperty, new Binding("IsListView")
            {
                Converter = new BooleanToVisibilityConverter()
            });
            grid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Files"));
            grid.SetBinding(DataGrid.SelectedItemProperty, new Binding("SelectedFile")
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            grid.Columns.Add(new DataGridCheckBoxColumn
            {
                Header = "\u9009\u62e9",
                Binding = new Binding("IsSelected")
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                },
                Width = 58
            });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u6587\u4ef6\u540d", Binding = new Binding("Name"), Width = 190 });
            grid.Columns.Add(CreateRiskColumn());
            grid.Columns.Add(new DataGridTextColumn { Header = "\u5927\u5c0f", Binding = new Binding("SizeDisplay"), SortMemberPath = "SizeBytes", Width = 100 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u6269\u5c55\u540d", Binding = new Binding("Extension"), Width = 90 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u521b\u5efa\u65f6\u95f4", Binding = new Binding("CreatedAt"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u4fee\u6539\u65f6\u95f4", Binding = new Binding("ModifiedAt"), Width = 150 });
            grid.Columns.Add(new DataGridTextColumn { Header = "\u5b8c\u6574\u8def\u5f84", Binding = new Binding("FullPath"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            return grid;
        }

        private static UIElement BuildDetailsSection()
        {
            var section = CreateSectionBorder(new Thickness(0));
            var panel = new Grid();
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var fileDetails = new StackPanel
            {
                Margin = new Thickness(0, 0, 14, 0)
            };
            fileDetails.Children.Add(CreateSectionTitle("\u9009\u4e2d\u6587\u4ef6"));

            var riskDetail = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.SemiBold,
                Foreground = CreateBrush(17, 24, 39),
                Margin = new Thickness(0, 6, 0, 4)
            };
            riskDetail.SetBinding(TextBlock.TextProperty, new Binding("SelectedFileRiskDetail"));
            fileDetails.Children.Add(riskDetail);

            var pathDetail = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Foreground = CreateBrush(75, 85, 99)
            };
            pathDetail.SetBinding(TextBlock.TextProperty, new Binding("SelectedFilePathDetail"));
            fileDetails.Children.Add(pathDetail);

            Grid.SetColumn(fileDetails, 0);
            panel.Children.Add(fileDetails);

            var statusDetails = new StackPanel
            {
                Margin = new Thickness(14, 0, 0, 0)
            };
            statusDetails.Children.Add(CreateSectionTitle("\u8fd0\u884c\u72b6\u6001"));

            var status = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Foreground = CreateBrush(31, 41, 55),
                Margin = new Thickness(0, 6, 0, 0)
            };
            status.SetBinding(TextBlock.TextProperty, new Binding("StatusText"));
            statusDetails.Children.Add(status);

            Grid.SetColumn(statusDetails, 1);
            panel.Children.Add(statusDetails);

            section.Child = panel;
            return section;
        }

        private static DataGridTemplateColumn CreateRiskColumn()
        {
            var template = new DataTemplate();

            var border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
            border.SetValue(Border.PaddingProperty, new Thickness(6, 2, 6, 2));
            border.SetValue(Border.MarginProperty, new Thickness(0, 2, 6, 2));
            border.SetBinding(Border.BackgroundProperty, new Binding("RiskBackgroundHex")
            {
                Converter = HexBrushConverter
            });
            border.SetBinding(Border.BorderBrushProperty, new Binding("RiskBorderHex")
            {
                Converter = HexBrushConverter
            });
            border.SetValue(Border.BorderThicknessProperty, new Thickness(1));

            var text = new FrameworkElementFactory(typeof(TextBlock));
            text.SetBinding(TextBlock.TextProperty, new Binding("RiskLevelDisplay"));
            text.SetBinding(TextBlock.ForegroundProperty, new Binding("RiskForegroundHex")
            {
                Converter = HexBrushConverter
            });
            text.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            text.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);

            border.AppendChild(text);
            template.VisualTree = border;

            return new DataGridTemplateColumn
            {
                Header = "\u6587\u4ef6\u7ea7\u522b",
                CellTemplate = template,
                SortMemberPath = "RiskLevelDisplay",
                Width = 128
            };
        }

        private static TreeView BuildFolderTree()
        {
            var tree = new TreeView
            {
                BorderBrush = CreateBrush(203, 213, 225),
                BorderThickness = new Thickness(1),
                Background = Brushes.White
            };

            tree.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            tree.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            tree.SetBinding(UIElement.VisibilityProperty, new Binding("IsFolderView")
            {
                Converter = new BooleanToVisibilityConverter()
            });
            tree.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("FolderTree"));
            tree.SelectedItemChanged += OnFolderTreeSelectedItemChanged;
            tree.ItemTemplate = BuildFolderTreeTemplate();

            return tree;
        }

        private static HierarchicalDataTemplate BuildFolderTreeTemplate()
        {
            var template = new HierarchicalDataTemplate(typeof(FolderTreeNodeViewModel));
            template.ItemsSource = new Binding("Children");

            var text = new FrameworkElementFactory(typeof(TextBlock));
            text.SetBinding(TextBlock.TextProperty, new Binding("DisplayText"));
            text.SetBinding(FrameworkElement.ToolTipProperty, new Binding("FullPath"));
            text.SetValue(TextBlock.MarginProperty, new Thickness(2, 3, 2, 3));
            text.SetValue(TextBlock.PaddingProperty, new Thickness(4, 1, 4, 1));
            text.SetBinding(TextBlock.ForegroundProperty, new Binding("RiskForegroundHex")
            {
                Converter = HexBrushConverter
            });
            text.SetBinding(TextBlock.BackgroundProperty, new Binding("RiskBackgroundHex")
            {
                Converter = HexBrushConverter
            });

            template.VisualTree = text;
            return template;
        }

        private static void OnFolderTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> args)
        {
            var tree = sender as TreeView;
            if (tree == null)
            {
                return;
            }

            var viewModel = tree.DataContext as MainWindowViewModel;
            if (viewModel == null)
            {
                return;
            }

            viewModel.SelectedFolderTreeNode = args.NewValue as FolderTreeNodeViewModel;
        }

        private static void AddLabeledTextBox(Grid panel, string label, string bindingPath, int column)
        {
            AddLabel(panel, label, column);

            var textBox = new TextBox
            {
                Height = 28,
                Margin = column == 0 ? new Thickness(0, 18, 0, 0) : new Thickness(8, 18, 0, 0),
                Padding = new Thickness(6, 0, 6, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = CreateBrush(203, 213, 225),
                BorderThickness = new Thickness(1)
            };
            textBox.SetBinding(TextBox.TextProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Delay = 400
            });
            Grid.SetColumn(textBox, column);
            panel.Children.Add(textBox);
        }

        private static void AddLabeledDatePicker(Grid panel, string label, string bindingPath, int column)
        {
            AddLabel(panel, label, column);

            var date = new DatePicker
            {
                Height = 28,
                Margin = new Thickness(8, 18, 0, 0)
            };
            date.SetBinding(DatePicker.SelectedDateProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(date, column);
            panel.Children.Add(date);
        }

        private static void AddLabeledComboBox(Grid panel, string label, string itemsSourcePath, string selectedItemPath, int column)
        {
            AddLabel(panel, label, column);

            var comboBox = new ComboBox
            {
                Height = 28,
                Margin = new Thickness(8, 18, 0, 0),
                DisplayMemberPath = "Label"
            };
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(itemsSourcePath));
            comboBox.SetBinding(Selector.SelectedItemProperty, new Binding(selectedItemPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
            Grid.SetColumn(comboBox, column);
            panel.Children.Add(comboBox);
        }

        private static void AddLabel(Grid panel, string text, int column)
        {
            var label = CreateSmallLabel(text);
            label.Margin = column == 0 ? new Thickness(0, 0, 0, 0) : new Thickness(8, 0, 0, 0);
            Grid.SetColumn(label, column);
            panel.Children.Add(label);
        }

        private static void AddToolbarButton(Grid toolbar, string text, string commandPath, int column, ButtonTone tone)
        {
            var button = CreateCommandButton(text, commandPath, tone, 96);
            button.Margin = new Thickness(8, 0, 0, 0);
            Grid.SetColumn(button, column);
            toolbar.Children.Add(button);
        }

        private static RadioButton CreateViewModeRadioButton(string text, string bindingPath)
        {
            var radioButton = new RadioButton
            {
                Content = text,
                GroupName = "ResultViewMode",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(14, 0, 0, 0),
                Foreground = CreateBrush(31, 41, 55)
            };

            radioButton.SetBinding(ToggleButton.IsCheckedProperty, new Binding(bindingPath)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            return radioButton;
        }

        private static Button CreateCommandButton(string text, string commandPath, ButtonTone tone, double minWidth)
        {
            var button = new Button
            {
                Content = text,
                Height = 32,
                MinWidth = minWidth,
                Margin = new Thickness(0, 0, 8, 0),
                Padding = new Thickness(10, 3, 10, 3),
                BorderThickness = new Thickness(1)
            };

            if (tone == ButtonTone.Primary)
            {
                button.Background = CreateBrush(37, 99, 235);
                button.BorderBrush = CreateBrush(37, 99, 235);
                button.Foreground = Brushes.White;
            }
            else if (tone == ButtonTone.Danger)
            {
                button.Background = CreateBrush(220, 38, 38);
                button.BorderBrush = CreateBrush(220, 38, 38);
                button.Foreground = Brushes.White;
            }
            else
            {
                button.Background = Brushes.White;
                button.BorderBrush = CreateBrush(203, 213, 225);
                button.Foreground = CreateBrush(31, 41, 55);
            }

            button.SetBinding(Button.CommandProperty, new Binding(commandPath));
            return button;
        }

        private static Border CreateSectionBorder(Thickness margin)
        {
            return new Border
            {
                Background = Brushes.White,
                BorderBrush = CreateBrush(226, 232, 240),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(6),
                Padding = new Thickness(12),
                Margin = margin
            };
        }

        private static TextBlock CreateSectionTitle(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Foreground = CreateBrush(31, 41, 55)
            };
        }

        private static TextBlock CreateSmallLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 12,
                Foreground = CreateBrush(75, 85, 99)
            };
        }

        private static SolidColorBrush CreateBrush(byte red, byte green, byte blue)
        {
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }

        private enum ButtonTone
        {
            Neutral,
            Primary,
            Danger
        }
    }
}
