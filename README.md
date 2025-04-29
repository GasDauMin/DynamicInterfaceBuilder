# Dynamic Interface Builder

A flexible WPF library for dynamically creating and managing user interfaces at runtime. This library allows you to build forms, dialogs, and interactive UI components programmatically with minimal code.

## Overview

DynamicInterfaceBuilder simplifies the creation of dynamic user interfaces in WPF applications by providing a set of high-level abstractions. It supports a configuration-driven approach to UI development, allowing for easy customization and theming without requiring XAML markup.

## Features

- **Dynamic Form Generation**: Create forms and dialogs at runtime with a fluent API
- **Configurable UI Elements**: Various form elements with customizable appearance and behavior
  - Text Boxes
  - Check Boxes
  - Combo Boxes
  - Radio Buttons
  - File/Folder Pickers
  - List Boxes
  - Numeric Inputs
  - Groups
- **Advanced Validation**: Add validation rules to form elements with custom error messages
- **Theme Support**: Multiple built-in themes with easy customization options
- **Automatic Layout**: Intelligent layout management with proper spacing and alignment
- **Configuration Management**: Save and load UI configurations using JSON
- **Style Inheritance**: Elements can inherit styles from their parent elements
- **Message System**: Built-in message display with configurable appearance

## Getting Started

### Prerequisites

- .NET 9.0 or higher
- Windows operating system

### Installation

Include the DynamicInterfaceBuilder library in your project:

```xml
<PackageReference Include="DynamicInterfaceBuilder" Version="1.0.0" />
```

### Basic Usage

```csharp
// Create a new instance of the builder
var app = new DynamicInterfaceBuilder.App();

// Configure form properties
app.Title = "My Dynamic Form";
app.Width = 800;
app.Height = 600;
app.Theme = ThemeType.Dark;

// Add form elements
app.Parameters["InputFile"] = new OrderedDictionary
{
    { "Type", FormElementType.TextBox },
    { "Label", "Input File" },
    { "Description", "The input file to process" },
    { "Validation", new[] {
        new OrderedDictionary {
            { "Type", FormElementValidationType.Required },
            { "Value", true },
            { "Message", "Input file is required." }
        }
    }}
};

// Display the form
app.Run();
```

## Configuration

The library supports configuration through code or JSON files:

```csharp
// Save the current configuration
app.SaveConfiguration();

// Load a previously saved configuration
app.LoadConfiguration();
```

### Advanced Properties

Configure advanced behavior using the `AdvancedProperties` class:

```csharp
app.AdvancedProperties.AdjustLabels = true;
app.AdvancedProperties.ReverseButtons = false;
app.AdvancedProperties.AllowResize = true;
app.AdvancedProperties.MaxMessageLines = 10;
```

### Style Properties

Customize the appearance using style properties:

```csharp
// Set global styles
app.StyleProperties.FontFamily = new FontFamily("Segoe UI");
app.StyleProperties.FontSize = 12;
app.StyleProperties.BackgroundColor = Colors.LightGray;
```

## Theming

The library includes several built-in themes:
- Default
- Light
- Dark
- DarkGrey
- DeepDark
- Grey
- RedBlack
- SoftDark

Apply a theme:

```csharp
app.Theme = ThemeType.Dark;
```

## Form Elements

### Adding Elements

```csharp
// Add a text box
app.Parameters["UserName"] = new OrderedDictionary
{
    { "Type", FormElementType.TextBox },
    { "Label", "User Name" },
    { "Description", "Enter your username" },
    { "DefaultValue", "user" }
};

// Add a dropdown
app.Parameters["Country"] = new OrderedDictionary
{
    { "Type", FormElementType.ComboBox },
    { "Label", "Country" },
    { "Items", new[] { "USA", "Canada", "Mexico", "UK" } },
    { "DefaultIndex", 0 }
};
```

### Grouping Elements

You can organize elements into groups:

```csharp
app.Parameters["UserGroup"] = new OrderedDictionary
{
    { "Type", FormElementType.Group },
    { "Label", "User Information" },
    { "Elements", new[] {
        new OrderedDictionary
        {
            { "Type", FormElementType.TextBox },
            { "Label", "First Name" },
            { "DefaultValue", "" }
        },
        new OrderedDictionary
        {
            { "Type", FormElementType.TextBox },
            { "Label", "Last Name" },
            { "DefaultValue", "" }
        }
    }}
};
```

## Validation

Add validation rules to form elements:

```csharp
app.Parameters["Email"] = new OrderedDictionary
{
    { "Type", FormElementType.TextBox },
    { "Label", "Email Address" },
    { "Validation", new[] {
        new OrderedDictionary {
            { "Type", FormElementValidationType.Required },
            { "Value", true },
            { "Message", "Email is required." }
        },
        new OrderedDictionary {
            { "Type", FormElementValidationType.Regex },
            { "Value", @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" },
            { "Message", "Invalid email format." }
        }
    }}
};
```

## Startup Settings

Configure startup behavior:

```csharp
// Auto-load previous configurations
app.StartupProperties.AutoLoadConfig = true;
app.StartupProperties.AutoSaveConfig = true;
app.StartupProperties.ConfigPath = "path/to/config.json";
```

## Building the Project

```powershell
# Build the project
.\_build.ps1

# Run tests
.\_test.ps1
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.