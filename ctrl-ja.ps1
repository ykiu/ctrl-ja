$source = Get-Content -Path "./ctrl-ja.cs"
Add-Type -TypeDefinition "$source" -ReferencedAssemblies System.Windows.Forms

[CtrlJa.Program]::Main();