$source = Get-Content -Path (Join-Path -Path $PSScriptRoot -ChildPath "ctrl-ja.cs")
Add-Type -TypeDefinition "$source" -ReferencedAssemblies System.Windows.Forms

[CtrlJa.Program]::Main();