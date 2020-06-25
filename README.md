# AutoMovePipWindow

This badly writen app will move the picture-in-picture video player from Firefox and Opera to the other desktop screen if you have 2 screens.

## TODO

- [x] Configuration
  - [x] Read `className` and `windowTitle` from config
  - [x] Get target position from screen position with a config file
  - [ ] ~~Generate a sample config file from current screen configuration~~
  - [x] Define size in configuration
- [x] Hide window from taskbar
- [ ] Handle multiple DPI per screen to avoid multiple move and resize
- [ ] Replace timer with event throttler
- [ ] Better handling of square videos (too big)
