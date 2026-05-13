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
            Title = "File Scanner P10A - UI Polish";
            Width = 1380;
            Height = 820;
            MinWidth = 1120;
            MinHeight = 680;
            Background = CreateBrush(246, 248, 251);
            DataContext = new MainWindowViewModel();
            Content = BuildContent();
        }

        private static UIElement BuildContent()
        {
            var root = new Grid();
            root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(196) });
            root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var navigation = BuildNavigation();
            Grid.SetColumn(navigation, 0);
            root.Children.Add(navigation);

            var main = new Grid
            {
                Margin = new Thickness(16)
            };
            main.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            main.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            main.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            main.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            main.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetColumn(main, 1);
            root.Children.Add(main);

            var overview = BuildOverviewSection();
            Grid.SetRow(overview, 0);
            main.Children.Add(overview);

            var filters = BuildFilterSection();
            Grid.SetRow(filters, 1);
            main.Children.Add(filters);

            var categories = BuildCategorySection();
            Grid.SetRow(categories, 2);
            main.Children.Add(categories);

            var results = BuildResultsSection();
            Grid.SetRow(results, 3);
            main.Children.Add(results);

            var details = BuildDetailsSection();
            Grid.SetRow(details, 4);
            main.Children.Add(details);

            return root;
        }

        private static UIElement BuildNavigation()
        {
            var border = new Border
            {
                Background = CreateBrush(0, 190, 124),
                Padding = new Thickness(12, 18, 12, 18)
            };

            var panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = "\u78c1\u76d8\u6e05\u7406",
                Foreground = Brushes.White,
                FontSize = 20,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(4, 0, 0, 26)
            });

            panel.Children.Add(CreateNavItem("\u78c1\u76d8\u626b\u63cf", true));
            panel.Children.Add(CreateNavItem("\u91cd\u590d\u6587\u4ef6", false));
            panel.Children.Add(CreateNavItem("\u5927\u6587\u4ef6\u5206\u6790", false));
            panel.Children.Add(CreateNavItem("\u7f13\u5b58/\u4e34\u65f6", false));
            panel.Children.Add(CreateNavItem("\u9690\u79c1\u75d5\u8ff9", false));
            panel.Children.Add(CreateNavItem("\u8bbe\u7f6e", false));

            border.Child = panel;
            return border;
        }

        private static UIElement CreateNavItem(string text, bool isActive)
        {
            var border = new Border
            {
                Background = isActive ? CreateBrush(210, 250, 235) : Brushes.Transparent,
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 14, 12, 14),
                Margin = new Thickness(0, 0, 0, 10)
            };

            border.Child = new TextBlock
            {
                Text = text,
                FontSize = 16,
                FontWeight = isActive ? FontWeights.SemiBold : FontWeights.Normal,
                Foreground = isActive ? CreateBrush(0, 150, 95) : Brushes.White
            };

            return border;
        }

        private static UIElement BuildOverviewSection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var left = new StackPanel();
            left.Children.Add(new TextBlock
            {
                Text = "\u626b\u63cf\u6982\u89c8",
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = CreateBrush(75, 85, 99)
            });

            var headline = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 8, 0, 8)
            };

            var reclaimable = new TextBlock
            {
                FontSize = 34,
                FontWeight = FontWeights.SemiBold,
                Foreground = CreateBrush(31, 41, 55)
            };
            reclaimable.SetBinding(TextBlock.TextProperty, new Binding("ReclaimableSummary"));
            headline.Children.Add(reclaimable);

            var selected = new TextBlock
            {
                FontSize = 24,
                Foreground = CreateBrush(107, 114, 128),
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(18, 0, 0, 4)
            };
            selected.SetBinding(TextBlock.TextProperty, new Binding("SelectedSizeSummary"));
            headline.Children.Add(selected);
            left.Children.Add(headline);

            var overviewLine = new TextBlock
            {
                Foreground = CreateBrush(107, 114, 128),
                FontSize = 13
            };
            overviewLine.SetBinding(TextBlock.TextProperty, new Binding("ScanOverviewSummary"));
            left.Children.Add(overviewLine);

            var pathRow = new Grid
            {
                Margin = new Thickness(0, 14, 0, 0)
            };
            pathRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            pathRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            pathRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            pathRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var pathLabel = CreateSmallLabel("\u626b\u63cf\u76ee\u5f55");
            pathLabel.VerticalAlignment = VerticalAlignment.Center;
            pathLabel.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(pathLabel, 0);
            pathRow.Children.Add(pathLabel);

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
            pathRow.Children.Add(pathBox);

            AddToolbarButton(pathRow, "\u9009\u62e9\u76ee\u5f55", "BrowseCommand", 2, ButtonTone.Neutral);
            AddToolbarButton(pathRow, "\u53d6\u6d88", "CancelCommand", 3, ButtonTone.Neutral);

            left.Children.Add(pathRow);
            Grid.SetColumn(left, 0);
            grid.Children.Add(left);

            var actions = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(22, 0, 0, 0)
            };
            actions.Children.Add(CreateCommandButton("\u5f00\u59cb\u626b\u63cf", "ScanCommand", ButtonTone.Primary, 156, 48));
            actions.Children.Add(CreateCommandButton("\u5220\u9664\u6587\u4ef6", "DeleteSelectedCommand", ButtonTone.Danger, 156, 38));
            Grid.SetColumn(actions, 1);
            grid.Children.Add(actions);

            section.Child = grid;
            return section;
        }

        private static UIElement BuildFilterSection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var filters = new Grid();

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

            var clear = CreateCommandButton("\u6e05\u9664\u7b5b\u9009", "ClearFiltersCommand", ButtonTone.Neutral, 96, 28);
            clear.Margin = new Thickness(8, 18, 0, 0);
            Grid.SetColumn(clear, 6);
            filters.Children.Add(clear);

            section.Child = filters;
            return section;
        }

        private static UIElement BuildCategorySection()
        {
            var section = CreateSectionBorder(new Thickness(0, 0, 0, 12));
            var panel = new StackPanel();
            panel.Children.Add(CreateSectionTitle("\u6e05\u7406\u5206\u7c7b"));

            var items = new ItemsControl
            {
                Margin = new Thickness(0, 10, 0, 0)
            };
            items.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("CleanupCategories"));

            var itemsPanel = new ItemsPanelTemplate();
            var wrap = new FrameworkElementFactory(typeof(WrapPanel));
            wrap.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
            itemsPanel.VisualTree = wrap;
            items.ItemsPanel = itemsPanel;
            items.ItemTemplate = BuildCategoryCardTemplate();

            panel.Children.Add(items);
            section.Child = panel;
            return section;
        }

        private static DataTemplate BuildCategoryCardTemplate()
        {
            var template = new DataTemplate(typeof(CleanupCategoryCardViewModel));

            var button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Control.PaddingProperty, new Thickness(0));
            button.SetValue(Control.BorderThicknessProperty, new Thickness(0));
            button.SetValue(Control.BackgroundProperty, Brushes.Transparent);
            button.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
            button.SetValue(Control.VerticalContentAlignmentProperty, VerticalAlignment.Stretch);
            button.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 12, 12));
            button.SetBinding(Button.CommandProperty, new Binding("DataContext.SelectCleanupCategoryCommand")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ItemsControl), 1)
            });
            button.SetBinding(Button.CommandParameterProperty, new Binding("Kind"));

            var border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.WidthProperty, 190D);
            border.SetValue(Border.MinHeightProperty, 118D);
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(8));
            border.SetValue(Border.PaddingProperty, new Thickness(12));
            border.SetValue(Border.BorderThicknessProperty, new Thickness(1));
            border.SetBinding(Border.BorderBrushProperty, new Binding("BorderHex")
            {
                Converter = HexBrushConverter
            });
            border.SetBinding(Border.BackgroundProperty, new Binding("BackgroundHex")
            {
                Converter = HexBrushConverter
            });

            var stack = new FrameworkElementFactory(typeof(StackPanel));

            var top = new FrameworkElementFactory(typeof(StackPanel));
            top.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            top.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 0, 8));

            var icon = new FrameworkElementFactory(typeof(Border));
            icon.SetValue(Border.WidthProperty, 34D);
            icon.SetValue(Border.HeightProperty, 34D);
            icon.SetValue(Border.CornerRadiusProperty, new CornerRadius(17));
            icon.SetBinding(Border.BackgroundProperty, new Binding("AccentHex")
            {
                Converter = HexBrushConverter
            });

            var iconText = new FrameworkElementFactory(typeof(TextBlock));
            iconText.SetBinding(TextBlock.TextProperty, new Binding("IconText"));
            iconText.SetValue(TextBlock.ForegroundProperty, Brushes.White);
            iconText.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            iconText.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            iconText.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            icon.AppendChild(iconText);
            top.AppendChild(icon);

            var title = new FrameworkElementFactory(typeof(TextBlock));
            title.SetBinding(TextBlock.TextProperty, new Binding("Title"));
            title.SetValue(TextBlock.FontSizeProperty, 15D);
            title.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            title.SetValue(TextBlock.ForegroundProperty, CreateBrush(31, 41, 55));
            title.SetValue(FrameworkElement.MarginProperty, new Thickness(8, 6, 0, 0));
            top.AppendChild(title);

            stack.AppendChild(top);

            var size = new FrameworkElementFactory(typeof(TextBlock));
            size.SetBinding(TextBlock.TextProperty, new Binding("SizeDisplay"));
            size.SetValue(TextBlock.FontSizeProperty, 22D);
            size.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            size.SetBinding(TextBlock.ForegroundProperty, new Binding("AccentHex")
            {
                Converter = HexBrushConverter
            });
            stack.AppendChild(size);

            var count = new FrameworkElementFactory(typeof(TextBlock));
            count.SetBinding(TextBlock.TextProperty, new Binding("CountDisplay"));
            count.SetValue(TextBlock.ForegroundProperty, CreateBrush(75, 85, 99));
            count.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 2, 0, 0));
            stack.AppendChild(count);

            var desc = new FrameworkElementFactory(typeof(TextBlock));
            desc.SetBinding(TextBlock.TextProperty, new Binding("Description"));
            desc.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            desc.SetValue(TextBlock.FontSizeProperty, 11D);
            desc.SetValue(TextBlock.ForegroundProperty, CreateBrush(107, 114, 128));
            desc.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 8, 0, 0));
            stack.AppendChild(desc);

            var action = new FrameworkElementFactory(typeof(TextBlock));
            action.SetBinding(TextBlock.TextProperty, new Binding("ActionText"));
            action.SetBinding(TextBlock.ForegroundProperty, new Binding("AccentHex")
            {
                Converter = HexBrushConverter
            });
            action.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
            action.SetValue(TextBlock.FontSizeProperty, 11D);
            action.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 8, 0, 0));
            stack.AppendChild(action);

            border.AppendChild(stack);
            button.AppendChild(border);
            template.VisualTree = button;
            return template;
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

            resultsHost.Children.Add(BuildFileGrid());
            resultsHost.Children.Add(BuildFolderTree());

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
            viewModes.Children.Add(CreateSmallLabel("\u8be6\u7ec6\u7ed3\u679c"));
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

            batchTools.Children.Add(CreateCommandButton("\u5168\u9009\u5f53\u524d\u7ed3\u679c", "SelectAllVisibleCommand", ButtonTone.Neutral, 118, 32));
            batchTools.Children.Add(CreateCommandButton("\u6e05\u7a7a\u9009\u62e9", "ClearSelectionCommand", ButtonTone.Neutral, 96, 32));
            batchTools.Children.Add(CreateCommandButton("\u6279\u91cf\u5220\u9664", "DeleteCheckedCommand", ButtonTone.Danger, 96, 32));

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
            var button = CreateCommandButton(text, commandPath, tone, 96, 32);
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

        private static Button CreateCommandButton(string text, string commandPath, ButtonTone tone, double minWidth, double height)
        {
            var button = new Button
            {
                Content = text,
                Height = height,
                MinWidth = minWidth,
                Margin = new Thickness(0, 0, 8, 0),
                Padding = new Thickness(10, 3, 10, 3),
                BorderThickness = new Thickness(1)
            };

            if (tone == ButtonTone.Primary)
            {
                button.Background = CreateBrush(0, 190, 124);
                button.BorderBrush = CreateBrush(0, 170, 108);
                button.Foreground = Brushes.White;
                button.FontSize = 18;
                button.FontWeight = FontWeights.SemiBold;
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
                FontSize = 14,
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
