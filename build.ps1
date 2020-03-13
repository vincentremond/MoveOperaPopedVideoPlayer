try {
    Push-Location    

    Set-Location .\src\AutoMovePipWindow\

    if (Get-Process AutoMovePipWindow) {
        Stop-Process -Name AutoMovePipWindow -Force
        $startAfter = $true
    }
    else {
        $startAfter = $false
    }

    dotnet publish --configuration Release --runtime win10-x64

    if ($startAfter) {
        Set-Location .\bin\Release\netcoreapp3.1\win10-x64\publish\
        .\AutoMovePipWindow.exe
    }

}
finally {
    Pop-Location
}