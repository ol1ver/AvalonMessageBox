using System;
using System.Collections.Generic;
using System.Windows;

namespace AvalonMessageBox
{
    /// <summary>
    /// Builds and shows a message box.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageBoxBuilder<TReturn>
    {
        private readonly List<ButtonConfig> buttons = new List<ButtonConfig>();

        private string title;
        private string message;
        private Window owner;
        private bool centerScreen = true;
        private double zoom = 1;
        private MessageBoxImage icon;
        private Optional<object> closeButtonReturnValue;

        private delegate TReturn ShowDelegate();
        
        /// <summary>
        /// Sets the window title.
        /// </summary>
        /// <param name="title">The window title.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Title(string title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        /// Sets the message of the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Message(string message)
        {
            this.message = message;
            return this;
        }

        /// <summary>
        /// Adds a button to the message box.
        /// </summary>
        /// <param name="text">The button text.</param>
        /// <param name="returnValue">A value that <see cref="Show"/> will return when the user has clicked it.</param>
        /// <param name="isDefault">A boolean value that decides whether the button is the default button.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Button(string text, TReturn returnValue, bool isDefault = false)
        {
            buttons.Add(new ButtonConfig(text, returnValue, isDefault));
            return this;
        }

        /// <summary>
        /// Adds a button to the message box.
        /// </summary>
        /// <param name="button">The button to show. Windows will localize the text.</param>
        /// <param name="returnValue">A value that <see cref="Show"/> will return when the user has clicked it.</param>
        /// <param name="isDefault">A boolean value that decides whether the button is the default button.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Button(Buttons button, TReturn returnValue, bool isDefault = false)
        {
            buttons.Add(new ButtonConfig(button, returnValue, isDefault));
            return this;
        }

        /// <summary>
        /// Removes all previously added buttons.
        /// </summary>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> ClearButtons()
        {
            buttons.Clear();
            return this;
        }

        /// <summary>
        /// Sets the return value when the message box was closed with the X-button.
        /// </summary>
        /// <param name="returnValue">A value that <see cref="Show"/> will return when the user has clicked it.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> CloseButton(TReturn returnValue)
        {
            closeButtonReturnValue = new Optional<object>(returnValue);
            return this;
        }

        /// <summary>
        /// Sets the owner of the message box.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Owner(Window owner)
        {
            this.owner = owner;
            return this;
        }

        /// <summary>
        /// Set the startup location of the message box.
        /// </summary>
        /// <param name="startupLocation">The location where the message box should be shown.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> StartupLocation(StartupLocation startupLocation)
        {
            centerScreen = startupLocation == AvalonMessageBox.StartupLocation.CenterScreen;
            return this;
        }

        /// <summary>
        /// The zoom of the message box content.
        /// </summary>
        /// <param name="zoom">The zoom value as a factor (default: 1).</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Zoom(double zoom)
        {
            this.zoom = Math.Max(0.1, zoom);
            return this;
        }

        /// <summary>
        /// The icon that is display on the left side of the message text.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <returns>The MessageBoxBuilder, so you can chain multiple method calls.</returns>
        public MessageBoxBuilder<TReturn> Icon(MessageBoxImage icon)
        {
            this.icon = icon;
            return this;
        }

        /// <summary>
        /// Shows the message box with the provided parameters.
        /// </summary>
        /// <returns>The provided result value of the selected button.</returns>
        public TReturn Show()
        {
            if (!Application.Current.Dispatcher.CheckAccess())
                return (TReturn)Application.Current.Dispatcher.Invoke(new ShowDelegate(Show));

            var window = new MessageBoxWindow(closeButtonReturnValue) { Owner = owner };

            foreach (var button in buttons)
                if (button.Text is string s)
                    window.AddButton(s, button.ReturnValue, button.IsDefault);
                else if (button.Text is Buttons b)
                    AddButton(window, b, button.ReturnValue, button.IsDefault);

            window.Title = title ?? string.Empty;
            window.message.Text = message ?? string.Empty;
            window.WindowStartupLocation = centerScreen ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
            window.scaleTransform.ScaleX = zoom;
            window.scaleTransform.ScaleY = zoom;
            window.icon = icon;

            window.ShowDialog();

            return (TReturn)window.returnValue.Value;
        }

        private void AddButton(MessageBoxWindow window, Buttons buttons, object returnValue, bool isDefault)
        {
            if (MessageBoxBuilder.localizationDict == null)
            {
                var localizedStrings = new LocalizedStrings();
                MessageBoxBuilder.localizationDict = localizedStrings.Localization;
            }

            window.AddButton(MessageBoxBuilder.localizationDict[buttons], returnValue, isDefault);
        }
    }
}