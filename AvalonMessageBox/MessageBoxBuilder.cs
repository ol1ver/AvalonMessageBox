using System;
using System.Collections.Generic;
using System.Windows;

namespace AvalonMessageBox
{
    /// <summary>
    /// Builds and shows a message box.
    /// </summary>
    public class MessageBoxBuilder : MessageBoxBuilder<object>
    {
        internal static IDictionary<Buttons, string> localizationDict;

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The window title.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="image">The image to show.</param>
        /// <param name="defaultResult">Based on this value a default button will be selected. Closing the window with the X, will return this value.</param>
        /// <returns>The result of the clicked button.</returns>
        public static MessageBoxResult Show(
            string message,
            string title = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            return Show(null, message, title, buttons, image, defaultResult);
        }

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="owner">The owner-window.</param>
        /// <param name="message">The message.</param>
        /// <param name="title">The window title.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="image">The image to show.</param>
        /// <param name="defaultResult">Based on this value a default button will be selected. Closing the window with the X, will return this value.</param>
        /// <returns>The result of the clicked button.</returns>
        public static MessageBoxResult Show(
            Window owner,
            string message,
            string title = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None)
            => Create(owner, message, title, buttons, image, defaultResult).Show();

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The window title.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="image">The image to show.</param>
        /// <param name="defaultResult">Based on this value a default button will be selected. Closing the window with the X, will return this value.</param>
        /// <returns>The builder for further modification.</returns>
        public static MessageBoxBuilder<MessageBoxResult> Create(
            string message,
            string title = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None)
            => Create(null, message, title, buttons, image, defaultResult);

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="owner">The owner-window.</param>
        /// <param name="message">The message.</param>
        /// <param name="title">The window title.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="image">The image to show.</param>
        /// <param name="defaultResult">Based on this value a default button will be selected. Closing the window with the X, will return this value.</param>
        /// <returns>The builder for further modification.</returns>
        public static MessageBoxBuilder<MessageBoxResult> Create(
            Window owner,
            string message,
            string title = "",
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.None)
        {
            var builder = new MessageBoxBuilder<MessageBoxResult>()
                .Owner(owner)
                .Message(message)
                .Title(title)
                .Icon(image);

            if (IsDefaultResultInvalid(defaultResult, buttons))
                defaultResult = MessageBoxResult.None;
            
            var defaultButton = DetermineDefaultButton(buttons, defaultResult);
            AddButtons(builder, buttons, defaultButton);
            SetUpCloseButton(builder, buttons, defaultResult);

            return builder;
        }

        private static bool IsDefaultResultInvalid(MessageBoxResult defaultResult, MessageBoxButton buttons)
        {
            switch (defaultResult)
            {
                case MessageBoxResult.None:
                    return false;
                case MessageBoxResult.OK:
                    return buttons == MessageBoxButton.YesNo || buttons == MessageBoxButton.YesNoCancel;
                case MessageBoxResult.Cancel:
                    return buttons == MessageBoxButton.OK || buttons == MessageBoxButton.YesNo;
                case MessageBoxResult.Yes:
                case MessageBoxResult.No:
                    return buttons == MessageBoxButton.OK || buttons == MessageBoxButton.OKCancel;
                default:
                    throw new ArgumentOutOfRangeException(nameof(defaultResult), defaultResult, null);
            }
        }

        private static MessageBoxResult DetermineDefaultButton(MessageBoxButton buttons, MessageBoxResult defaultResult)
        {
            if (defaultResult != MessageBoxResult.None)
                return defaultResult;

            switch (buttons)
            {
                case MessageBoxButton.OK:
                case MessageBoxButton.OKCancel:
                    return MessageBoxResult.OK;
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:
                    return MessageBoxResult.Yes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
        }

        private static void AddButtons(MessageBoxBuilder<MessageBoxResult> builder, MessageBoxButton buttons, MessageBoxResult defaultButton)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                case MessageBoxButton.OKCancel:
                    builder.Button(Buttons.Ok, MessageBoxResult.OK, defaultButton == MessageBoxResult.OK);
                    break;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    builder.Button(Buttons.Yes, MessageBoxResult.Yes, defaultButton == MessageBoxResult.Yes);
                    builder.Button(Buttons.No, MessageBoxResult.No, defaultButton == MessageBoxResult.No);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }

            if (buttons == MessageBoxButton.OKCancel || buttons == MessageBoxButton.YesNoCancel)
                builder.Button(Buttons.Cancel, MessageBoxResult.Cancel, defaultButton == MessageBoxResult.Cancel);
        }

        private static void SetUpCloseButton(MessageBoxBuilder<MessageBoxResult> builder, MessageBoxButton buttons, MessageBoxResult defaultResult)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    builder.CloseButton(MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                case MessageBoxButton.YesNoCancel:
                    builder.CloseButton(MessageBoxResult.Cancel);
                    break;
                case MessageBoxButton.YesNo:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(defaultResult), defaultResult, null);
            }
        }
    }
}