# Set script path
$scriptPath = $PSScriptRoot

# Load the correct assembly
Add-Type -Path "$scriptPath\bin\Debug\net9.0-windows\DynamicInterfaceBuilder.dll"

# Create an instance of the App 
$application = New-Object DynamicInterfaceBuilder.App

# Set application properties
$application.Title = "Dynamic Interface Demo"
# Uncomment these lines if you want to set specific dimensions
# $application.Width = 800
# $application.Height = 600

# Add a test group with various form elements
$application.Parameters["TestGroup"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::Group
    Label = "Test group"
    Description = "Test group description"
    # Uncomment these lines if you want to apply specific styling
    # Style = @{
    #     "ValueControl.FontSize" = 15
    #     "ValueControl.FontWeight" = [System.Windows.FontWeights]::Bold
    #     "ValueControl.FontFamily" = New-Object System.Windows.Media.FontFamily("Consolas")
    # }
    Elements = @(
        @{
            Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::CheckBox
            Label = "Test check box"
            Description = "Test check box description"
            DefaultValue = $true
        },
        @{
            Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::CheckBox
            Label = "Test check box"
            Description = "Test check box description"
            DefaultValue = $true
        },
        @{
            Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
            Label = "Test text box"
            Description = "Test text box description"
            DefaultValue = ""
            Validation = @{
                Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::Required
                Value = $true
                Message = "Test validation description"
                Runtime = $true
            }
            # Uncomment these lines if you want to apply specific styling
            # Style = @{
            #     "ValueControl.Background" = [System.Windows.Media.Colors]::LightGray.ToString()
            #     "ValueControl.Foreground" = [System.Windows.Media.Colors]::Black.ToString()
            # }
        },
        @{
            GroupName = "NestedGroup"
            Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::Group
            Label = "Nested group"
            Description = "Nested group description"
            Elements = @(
                @{
                    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
                    Label = "Nested text box"
                    Value = ""
                    Description = "Nested text box description"
                    Validation = @{
                        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::Required
                        Value = $true
                        Message = "Another test validation description"
                        Runtime = $true
                    }
                }
            )
        },
        @{
            Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::ComboBox
            Label = "Test combobox"
            Description = "Test combobox description"
            DefaultValue = "Item 3"
            Items = @("Item 1", "Item 2", "Item 3")
        }
    )
}

# Display some information in the console
Write-Host "Dynamic Interface Demo Application Created Successfully!"
Write-Host "Parameters configured:"
foreach ($param in $application.Parameters.Keys) {
    Write-Host "- $param"
}

# Run the application
try {
    Write-Host "Starting the application..."
    $result = $application.Run()
    Write-Host "Application returned with result: $result"
}
catch {
    Write-Host "Error running the application: $_" -ForegroundColor Red
}
finally {
    # Cleanup
    Write-Host "Performing cleanup..."
    
    # Remove event handlers if they exist using reflection
    $appType = $application.GetType()
    $events = $appType.GetEvents()
    
    foreach ($event in $events) {
        $fieldName = "Event$($event.Name)"
        $field = $appType.GetField($fieldName, [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance)
        if ($field -ne $null) {
            $field.SetValue($application, $null)
        }
    }
    
    # Clear strong references
    if ($application.FormBuilder -ne $null) {
        # Try to explicitly shutdown if possible
        $shutdownMethod = $application.FormBuilder.GetType().GetMethod("Dispose")
        if ($shutdownMethod -ne $null) {
            $shutdownMethod.Invoke($application.FormBuilder, $null)
        }
    }
    
    # Set to null to allow garbage collection
    $application = $null
    
    # Force garbage collection multiple times
    [System.GC]::Collect()
    [System.GC]::WaitForPendingFinalizers()
    [System.GC]::Collect()
    
    Write-Host "Cleanup completed."
}

Write-Host "Application execution completed."