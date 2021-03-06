## Next

**Features**

- [#28] Register ViewModel and View types directly in WindowLocator.

## 2.0.0-alpha4 (2015-02-14)

**Features**

- Minor changes, more tests and some refactoring.

**Bugfixes**

- [#25] NullReferenceException when showing window from another project. (Thanks to [togocoder][togocoder])
- [#27] Views inherited from Window or UserControl not registered in WindowLocator.

## 2.0.0-alpha3 (2015-02-03)

**Features**

- [#19] WindowLocatorBase.GetBaseName tests.
- [#21] Autoload UserControls in Window.
- [#20] Automatically bind a UserControl to the parent Window.

**Bugfixes**

- [#18] ReSharper Annotations attributes are erased from metadata by default.

## 2.0.0-alpha2 (2015-01-10)

**Features**

- [#17] Switch to the official ReSharper Annotations.
- [#16] Include referenced assemblies.

## 2.0.0-alpha1 (2015-01-04)

**Features**

- Initial alpha release.
- Ioc container: Call stored instances of classes from anywhere.
- WindowLocator: Loosely-coupled way to `show` and `close` windows from the ViewModel.
- RelayCommand: Execute actions/methods in the ViewModel from the View eg. from a `Button`.

[togocoder]: https://github.com/togocoder
