# Set script path
$scriptPath = $PSScriptRoot

try {
    # Load the assembly
    Add-Type -Path "$scriptPath\bin\Debug\net7.0-windows\DynamicInterfaceBuilder.dll"

    # Create an instance of the builder
    $builder = New-Object TheToolkit.DynamicInterfaceBuilder("OMG")

    # Set properties
    $builder.Title = "TETS"
    $builder.Width = 1024
    $builder.Height = 768
    $builder.Theme = "Dark"

    $builder.Margin = 5
    $builder.Padding = 5

    $builder.SaveConfiguration()
    $builder.LoadConfiguration()

    # Add parameters (example)
    $builder.Parameters["InputFile"] = "test.txt"
    $builder.Parameters["OutputPath"] = "C:\output"
    $builder.RunForm()
    
    # Display the current settings
    Write-Host "Interface Settings:"
    Write-Host "Title: $($builder.Title)"
    Write-Host "Size: $($builder.Width) x $($builder.Height)"
    Write-Host "`nParameters:"
    $builder.Parameters.GetEnumerator() | ForEach-Object {
        Write-Host "$($_.Key): $($_.Value)"
    }

    
}
finally {

}