# Move Opera Poped Video Player

This badly writen app will move the popup video player from opera (and now Firefox) to the other desktop screen if you have 2 screens.

## Build

cd .\src\MoveWindows\
dotnet publish --configuration Release --runtime win10-x64

## TODO

- [ ] Read `className` and `windowTitle` from config
- [ ] Get target position from screen position with a config file
- [x] Hide window from taskbar
- [ ] Generate a sample config file from current screen configuration
