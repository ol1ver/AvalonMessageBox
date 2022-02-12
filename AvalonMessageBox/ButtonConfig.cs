namespace AvalonMessageBox
{
    internal class ButtonConfig
    {
        public ButtonConfig(object text, object returnValue, bool isDefault)
        {
            Text = text;
            ReturnValue = returnValue;
            IsDefault = isDefault;
        }

        public object Text { get; }
        public object ReturnValue { get; }
        public bool IsDefault { get; }
    }
}