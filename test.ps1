# Set scirpt path
$scriptPath = $PSScriptRoot

# Load the DLL
Add-Type -Path "$scriptPath\DynamicInterfaceBuilder.dll"

# Create an instance of the builder
$builder = New-Object TheToolkit.DynamicInterfaceBuilder("My Interface", 800, 600)

# Set properties
$builder.Title = "Custom Interface"
$builder.Width = 1024
$builder.Height = 768
$builder.DarkMode = $true

# Add parameters (example)
$builder.Parameters["InputFile"] = "test.txt"
$builder.Parameters["OutputPath"] = "C:\output"

# Display the current settings
Write-Host "Interface Settings:"
Write-Host "Title: $($builder.Title)"
Write-Host "Size: $($builder.Width) x $($builder.Height)"
Write-Host "Dark Mode: $($builder.DarkMode)"
Write-Host "`nParameters:"
$builder.Parameters.GetEnumerator() | ForEach-Object {
    Write-Host "$($_.Key): $($_.Value)"
}
