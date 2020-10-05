$svg = "icon"
$svgFullName = "icon.svg"

$Sizes = @(16, 24, 32, 48, 64, 128, 256)

$ArgumentList = [System.Collections.ArrayList]@("convert", "$($svg).ico")
foreach ($Size in $Sizes) {
    Write-Host "Creating variant $Size"
        $SizedFile = "$($svg)-$($Size).png"
    $InkscapeArguments = @("--export-filename=$($SizedFile)", "--export-width=$($Size)", "--export-height=$($Size)", $svgFullName )
    Start-Process 'C:\Program Files\Inkscape\bin\inkscape.exe' -ArgumentList $InkscapeArguments -Wait -NoNewWindow
    $ArgumentList.Insert(1, $SizedFile)
}
Write-Host "Creating final file"
Start-Process -FilePath magick.exe -ArgumentList $ArgumentList -Wait -NoNewWindow
