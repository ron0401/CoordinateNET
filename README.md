# CoordinateNET

## Abstract
This library converts between GEO coordinates (Geographic coordinates), ECEF coordinates and ENU coordinates.
You can also convert local coordinates by rotating ENU coordinates.

For now, we can only handle 2D coordinates.
I am developing this library for use in robots that drive autonomously outdoors.

Most outdoor robots only need to consider moving in a plane, so they do not consider altitude.

However, in the future, I would like to be able to handle 3D coordinates as well.
