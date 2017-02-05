$assemblyVersion = Get-Content ($args[0] + "Properties\AssemblyInfo.cs") | 
    where { $_ -match '\[assembly: AssemblyVersion\("([\d\.]+)"\)\]' } | 
    foreach { $matches[1] }

$hsdataVersion = git -C ($args[0] + "hsdata") log -1 | 
    where { $_ -match "Update to patch ([\d\.]+)$" } | 
    foreach { $matches[1] }

if ($assemblyVersion -ne $hsdataVersion) {
    $versionRegex = [regex]"^(.+)\.\d+$"
    $aVersion = $versionRegex.Match($assemblyVersion).Groups[1].Value
    $hsdVersion = $versionRegex.Match($hsdataVersion).Groups[1].Value
    if ($aVersion -ne $hsdVersion) {
        echo "verify.ps1: verification error 1: assembly version ($assemblyVersion) does not match hsdata ($hsdataVersion)"
    }
    else {
        echo "verify.ps1: verification warning 1: assembly version ($assemblyVersion) does not match hsdata ($hsdataVersion)"
    }
}
else {
    echo "verify.ps1: version up-to-date: ($assemblyVersion)"
}
