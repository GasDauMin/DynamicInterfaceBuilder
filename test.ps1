# Set script path
$scriptPath = $PSScriptRoot

try {
    # Load the assembly
    Add-Type -Path "$scriptPath\bin\Debug\net7.0-windows\DynamicInterfaceBuilder.dll"

    # Create an instance of the builder
    $builder = New-Object TheToolkit.DynamicInterfaceBuilder("OMG")

    # Set properties
    $builder.Title = "Custom Interface"
    $builder.Width = 1024
    $builder.Height = 768

    $builder.SaveConfiguration()
    $builder.LoadConfiguration()

    # Add parameters (example)
    $builder.Parameters["InputFile"] = "test.txt"
    $builder.Parameters["OutputPath"] = "C:\output"

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
    # # Cleanup
    # Write-Host "`nCleaning up..."
    
    # # Release builder instance
    # if ($builder) {
    #     $builder = $null
    # }
    
    # # Force garbage collection
    # [System.GC]::Collect()
    # [System.GC]::WaitForPendingFinalizers()
    
    # # Find and release the assembly
    # $assemblies = [AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.FullName -like "*DynamicInterfaceBuilder*" }
    # foreach ($assembly in $assemblies) {
    #     try {
    #         # Get the assembly name
    #         $assemblyName = $assembly.GetName()
            
    #         # Remove the assembly from the current domain
    #         $domain = [AppDomain]::CurrentDomain
    #         $domain.GetAssemblies() | Where-Object { $_.FullName -eq $assembly.FullName } | ForEach-Object {
    #             $domain.Load($assemblyName)
    #         }
            
    #        # Write-Host "Released assembly: $($assembly.FullName)"
    #     } catch {
    #         Write-Host "Warning: Could not release assembly: $($assembly.FullName)"
    #     }
    # }
    
    # # Final garbage collection
    # [System.GC]::Collect()
    # [System.GC]::WaitForPendingFinalizers()
}