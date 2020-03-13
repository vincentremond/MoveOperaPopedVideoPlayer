try {
    Push-Location    

    Set-Location .\src\MoveWindows\

    if (Get-Process MoveWindows) {
        Stop-Process -Name MoveWindows -Force
        $startAfter = $true
    }
    else {
        $startAfter = $false
    }

    dotnet publish --configuration Release --runtime win10-x64

    if ($startAfter) {
        Set-Location .\bin\Release\netcoreapp3.1\win10-x64\publish\
        .\MoveWindows.exe
    }

}
finally {
    Pop-Location
}