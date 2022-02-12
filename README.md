# AvalonMessageBox

AvalonMessageBox is a drop-in replacement for the standard WPF MessageBox.

Classic MessageBox:
```cs
MessageBox.Show("Hello World?", "Greeting", MessageBoxButton.YesNo, MessageBoxImage.Question);
```
AvalonMessageBox (just add 'Builder'):
```cs
MessageBoxBuilder.Show("Hello World?", "Greeting", MessageBoxButton.YesNo, MessageBoxImage.Question);
```
The code above will produce this window:

![image](https://user-images.githubusercontent.com/759715/153726249-8973b1db-5a1e-4d1c-bd4a-38575cd2c8b1.png)

A more advanced example with custom buttons and larger UI.
```cs
var option = new MessageBoxBuilder<string>()
    .Message(@"This is a text.")
    .Button("An _option to choose", "optA")
    .Button("_Another option", "optB")
    .CloseButton("optC")
    .Zoom(2)
    .Icon(MessageBoxImage.Exclamation)
    .Show();

MessageBoxBuilder.Show($"Selected option: {option}");
```
This will produce this window:

![image](https://user-images.githubusercontent.com/759715/153726492-b0b40b7e-3793-48a7-bbb6-000793f82e17.png)

Clicking on 'Another option' will show then this:

![image](https://user-images.githubusercontent.com/759715/153726534-61e91e25-63e2-4a39-a37d-efad5dbe7fd5.png)

