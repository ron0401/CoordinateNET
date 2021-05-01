# CoordinateNET

## Abstract
This library converts between GEO coordinates (Geographic coordinates), ECEF coordinates and ENU coordinates.
You can also convert local coordinates by rotating ENU coordinates.

For now, we can only handle 2D coordinates.
I am developing this library for use in robots that drive autonomously outdoors.

Most outdoor robots only need to consider moving in a plane, so they do not consider altitude.

However, in the future, I would like to be able to handle 3D coordinates as well.

## Target

.NET 5.0

## Usage

### Get point by GEO
```cs
var geo = new GEO2d(34.5000, 135.0000);
```

In geo-coordinates, the earth is approximated as an ellipsoid.
WGS84 and GSR80 can be selected as the ellipsoid.
If not specified, it will be WGS84.

```cs
var geo = new GEO2d(34.5000, 135.0000,GEO2d.TypeOfEllipsoid.GRS80);
```

### Convert to ECEF
```cs
var ecef = geo.ConvertToECEF();
```

### Convert to ENU
```cs
var geo_1 = new GEO2d(34.5000, 135.0000,GEO2d.TypeOfEllipsoid.GRS80);
var geo_2 = new GEO2d(34.6000, 135.1000, GEO2d.TypeOfEllipsoid.GRS80);
var enu = geo_2.ConvertToENU(geo_1);
```

GEO is absolute coordinates and ENU is relative coordinates.

Although only one GEO coordinate is determined in the world, ENU is a relative coordinate from a certain point.

Therefore, the ENU coordinates must specify an origin called datum.

### Convert to local point
```cs
var geo_1 = new GEO2d(34.5000, 135.0000);
var geo_2 = new GEO2d(34.6000, 135.1000);
var local_1 = new LocalRotationCoordinate2d(geo_2, geo_1, 30 / 180 * Math.PI);
```

### Calculate distance between point to point
```cs
var geo_1 = new GEO2d(34.5000, 135.0000);
var geo_2 = new GEO2d(34.6000, 135.1000);
var enu_1 = geo_2.ConvertToENU(geo_1);
var geo_3 = new GEO2d(34.7000, 135.1500);
var enu_2 = geo_3.ConvertToENU(geo_1);
double dis = enu_2.GetDistance2d(enu_1);
```
To measure distance in relative coordinates, they must have the same origin.

Distance measurement in relative coordinates with different origins will be implemented in the future.

## Attention!!
This library is under development and has not been fully tested.

We are looking for a collaborator.

