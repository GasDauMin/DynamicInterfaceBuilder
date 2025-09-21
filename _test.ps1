# Set script path
$scriptPath = $PSScriptRoot

# Load the correct assembly
Add-Type -Path "$scriptPath\bin\Debug\net9.0-windows\DynamicInterfaceBuilder.dll"

# Create an instance of the App and test validation
$app = New-Object DynamicInterfaceBuilder.App("Validation Test")

# Set properties
$app.Title = "Validation Test Application"
$app.Width = 800
$app.Height = 600

# Add parameters with various validation types
$app.Parameters["RequiredTextBox"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
    Label = "Required Text"
    Description = "This field is required"
    Validation = @{
        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::Required
        Value = $true
        Message = "This field is required"
    }
}

$app.Parameters["EmailTextBox"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
    Label = "Email Address"
    Description = "Enter a valid email address"
    Validation = @{
        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::Regex
        Value = "^[\w\.-]+@[\w\.-]+\.\w+$"
        Message = "Please enter a valid email address"
    }
}

$app.Parameters["NumbersOnlyTextBox"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
    Label = "Numbers Only"
    Description = "Enter only numbers"
    Validation = @{
        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::OnlyNumbers
        Value = $true
        Message = "Only numbers are allowed"
    }
}

$app.Parameters["RangeTextBox"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
    Label = "Age (18-65)"
    Description = "Enter age between 18 and 65"
    Validation = @{
        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::Range
        Value = "18,65"
        Message = "Age must be between 18 and 65"
    }
}

$app.Parameters["MinLengthTextBox"] = @{
    Type = [DynamicInterfaceBuilder.Core.Enums.FormElementType]::TextBox
    Label = "Password"
    Description = "Enter password (minimum 8 characters)"
    Validation = @{
        Type = [DynamicInterfaceBuilder.Core.Enums.ValidationType]::MinLength
        Value = 8
        Message = "Password must be at least 8 characters long"
    }
}

# Test the form
Write-Host "Validation Test Application Created Successfully!"
Write-Host "Parameters configured:"
foreach ($param in $app.Parameters.Keys) {
    Write-Host "- $param"
}

# Run the form to test validation
$app.Run()

Write-Host "Validation test completed."