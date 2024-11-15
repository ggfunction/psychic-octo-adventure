
$ref = @(
    'System.Data'
    'System.Drawing'
    'System.Windows.Forms'
    '.\bin\Debug\net48\Newtonsoft.Json.dll'
)

Add-Type -Path .\bin\Debug\net48\Newtonsoft.Json.dll
Add-Type -Path .\*.cs -ReferencedAssemblies $ref

[Clipboard.Program]::Main()
