using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AvalonMessageBox
{
    internal partial class MessageBoxWindow : Window
    {
        internal MessageBoxImage icon = MessageBoxImage.Information;
        internal Optional<object> returnValue;

        private readonly Optional<object> closeButtonReturnValue;

        public MessageBoxWindow(Optional<object> closeButtonReturnValue)
        {
            this.closeButtonReturnValue = closeButtonReturnValue;
            InitializeComponent();

            Loaded += OnLoaded;
            Closing += OnClosing;
            Closed += OnClosed;
        }

        public void AddButton(string Text, object returnValue, bool isDefault)
        {
            var button = new Button
            {
                Content = Text,
                Padding = new Thickness(5, 2, 5, 2),
                MinWidth = 80,
                VerticalAlignment = VerticalAlignment.Center,
                IsDefault = isDefault
            };

            button.Click += delegate
            {
                this.returnValue = new Optional<object>(returnValue);
                Close();
            };

            if (buttonPanel.Children.Count > 0)
                button.Margin = new Thickness(8, 0, 0, 0);

            buttonPanel.Children.Add(button);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var lineCountProperty = typeof(TextBlock).GetProperty("LineCount", BindingFlags.Instance | BindingFlags.NonPublic);
            var lineCount = (int)lineCountProperty.GetValue(message);
            if (lineCount == 1)
                message.Margin = new Thickness(0, 8, 0, 0);

            IconHelper.RemoveIcon(this);

            var imageResource = icon switch
            {
                MessageBoxImage.Error => FindResource("ErrorIcon"),
                MessageBoxImage.Question => FindResource("QuestionIcon"),
                MessageBoxImage.Warning => FindResource("WarningIcon"),
                _ => FindResource("InformationIcon")
            };

            iconContent.Content = imageResource;

            if (!closeButtonReturnValue.HasValue)
                CloseButton.Disable(this);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!returnValue.HasValue && !closeButtonReturnValue.HasValue)
                e.Cancel = true;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (!returnValue.HasValue)
                returnValue = closeButtonReturnValue;
        }
    }
}