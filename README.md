# DisplayWaker
wake up display sensing human existence.

## sensehuman Arduino sketch
This thing simply sends High/Low signal via USB serial.
I believe this works fine with most of sensors. (I tested with just an ordinary motion sensor.)

## DisplayWakerAppWinUI
This is WPF app wrapped in appx.
Revieves sitgnal from the Arduino sketch above and wake screen.
