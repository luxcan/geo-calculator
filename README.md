# Geo Calculator

I created this calculator to meet my frequent need to perform the following tasks in my work:
1. Perform coordinate conversions, such as from geocentric to geodetic.
2. Plan the next coordinate from the initial target coordinate based on the distance/azimuth/elevation from the target coordinate.

I hope you find this calculator as useful in your work as it has been in mine.

It is a small Windows desktop tool, written in C# with Windows Forms, that prepares WGS84 coordinates for simulation scenarios such as MAK VR-Forces and HLA (RPR FOM). The geodesy is done by [GeographicLib](https://geographiclib.sourceforge.io/) (GeographicLib.NET).

## Conventions

Read these first; they decide what the numbers mean.

| Item | Meaning |
|---|---|
| Datum | WGS84 ellipsoid everywhere (a = 6378137 m, 1/f = 298.257223563). |
| X, Y, Z | Geocentric (ECEF) coordinates in metres. |
| Altitude | Height above the WGS84 ellipsoid (HAE) in metres, **not** height above mean sea level. The two differ by the local geoid height, which is tens of metres in many places. |
| Azimuth | Degrees clockwise from true (geodetic) north. Not grid north, not magnetic north. |
| Distance Apart, no elevation angle | Ground distance in km along the surface of the WGS84 ellipsoid (geodesic). It is measured on the ellipsoid itself, not at the target's altitude, and the new point keeps the target's altitude. So the straight line between the two points is not exactly the distance typed: height makes it longer (10 km at 10,000 m altitude is about 10.016 km) and the earth's curve makes it shorter over long distances (100 km at 0 m altitude is about 1 m shorter). When the straight-line distance must be exact, use an elevation angle. |
| Distance Apart, with an elevation angle | Straight-line (slant) range in km from the target, along the azimuth and the elevation angle. The elevation angle is measured from the target's local horizontal plane, so earth curvature is accounted for: a 0° elevation is a horizontal line of sight, which rises above the curved earth with distance (about 7.9 m after 10 km). |
| Numbers | Typed with `.` as the decimal point. In a box that holds one number, a comma is never read as a thousands separator or a decimal point; it is reported as an error. In the azimuth **List** the comma separates values, so `1,000` is two azimuths (1 and 0). Check the preview count. |
| Input limits | Lat -90 to 90, Lng -180 to 180, Altitude -12,000 to 100,000,000 m, X/Y/Z -110,000,000 to 110,000,000 m, Add Altitude -100,000,000 to 100,000,000 m, Distance Apart 0 to 40,075 km, Elevation Angle -90 to 90, azimuths -360 to 360 (at most 360 azimuths). On the Converter tab the converted point must also have an altitude from -12,000 to 100,000,000 m, so X, Y, Z and Add Altitude are limited further by the result. |
| Output precision | Latitude and longitude with 8 decimals (about 1 mm), metres with 4 decimals, DMS seconds with 5 decimals. Output always uses `.` as the decimal point. |

Every input is checked before anything is calculated. Problems are listed together in one **Invalid Input** message. A blank number box is an error, except in these optional boxes: a blank Elevation Angle means the distance is measured along the surface (it is not 0°); a blank Add Altitude, DMS minute or DMS second counts as 0; and Running No is needed only when the Replace Output text contains `${runningNo}`. No other blank or mistyped value is ever treated as 0.

## Converter tab

Converts one point between three forms:

- **Geocentric (ECEF)**: X, Y, Z in metres.
- **Lat/Lng**: latitude, longitude and altitude.
- **DMS**: degrees, minutes and seconds for latitude and longitude, plus altitude. Degrees are required whole numbers from 0 to 90 (latitude) or 0 to 180 (longitude), with no minus sign. Minutes and seconds are from 0 to below 60 (a blank minute or second counts as 0), and the whole angle may not pass 90° or 180°. The sign comes only from the **N/S** and **E/W** drop-downs.

Fill in one group and press its **Convert** button. The **Outputs** group shows the point in all three forms, with a line saying which group it was converted from and when.

**Add Altitude (m)** is optional (blank = 0). It is added to the altitude of every conversion, for example an antenna or sensor height.

**Copy to Clipboard** stores the last converted point inside the app (not the Windows clipboard) and shows it, rounded, in the **Clipboard** box at the bottom of the window. On the Distance From tab, **Paste From Clipboard** fills in Lat, Lng and Altitude (m) with that point at full precision.

## Distance From tab

Works out new points at a distance and azimuth (and optionally an elevation angle) from a target.

1. **Target Coordinate**: Lat, Lng and Altitude (m), or **Paste From Clipboard**.
2. **Distance and Elevation**: Distance Apart (km), and an optional Elevation Angle (degrees, from -90 to 90). Leave the elevation blank to measure the distance along the ellipsoid surface; the new points keep the target's altitude (see [Conventions](#conventions)).
3. **Azimuth**: choose **List** or **Range**.
   - **List**: one or more azimuths separated by commas, for example `45,135,225,315`. They are used in the order typed and printed with the value typed: they are not changed to 0 to below 360, so -45 stays -45 (and `045.50` prints as 45.5). Azimuths that point the same way are all calculated, with a warning.
   - **Range**: keyed as *start* to *end* every *step*, with **Clockwise** or **Anticlockwise**. For example 0 to 90 every 15 gives 0, 15, 30, 45, 60, 75, 90.
4. Press **Calculate N Points**.

### Range rules

- The range runs from the start in the chosen direction. The start is always included.
- The end is included only when it falls exactly on a step. Otherwise the last azimuth stops short of the end, and a warning says so.
- A range can pass through north: 300 to 60 every 30 clockwise gives 300, 330, 0, 30, 60.
- A full circle (for example 0 to 360 every 90) lists the start once: 0, 90, 180, 270.
- Range values are printed from 0 to below 360, so a start of -45 is printed as 315.
- Start and end are from -360 to 360, the step is greater than 0 and at most 360, and a range gives at most 360 azimuths.
- **Copy Range to List** turns the range into a list, so that single values can be removed or added.

### Azimuth Preview

Updates as you type, before you calculate:

- A north-up **compass** with a ray for each direction, numbered like the table (#1 is green). Azimuths that point the same way share a ray, and crowded rays are only partly numbered. With Replace Output ticked, `${runningNo}` in the text and a valid Running No, the numbers are the running numbers. A range also shows its sweep and direction. Hover over a ray for its details, and click a ray to select its row.
- A **count** and a description of the sweep, for example `7 azimuths · Range` and `0° to 90° clockwise, every 15° · covers 90°`.
- A **table** with each azimuth's Latitude and Longitude, worked out the same way as Calculate. The two columns show `–` until the target, distance and elevation are valid. Selecting a row highlights its ray and, when the output is up to date and Replace Output is off, scrolls the output to that result.
- A **status line** with any error, warning or note, for example *Passes through north at #3 (0°).*

### Output

The output starts with a summary line of the azimuths that were calculated (more than 24 are shortened to the first three … the last), followed by one block per azimuth. Each block header starts with `#n`, the same number as the azimuth's row in the table, and each block ends with a dashed line:

```
7 azimuths (Range 0° to 90° clockwise, every 15°): 0, 15, 30, 45, 60, 75, 90
---------------------------------------------------------------------------------------------
#1  Azimuth: 0°  Surface Distance: 10 km
X: -1523059.8672
Y: 6191551.6187
Z: 159490.9060
Latitude:  1.44253641
Longitude: 103.81980000
Altitude (m):  0.0000
DMS Latitude:    1° 26' 33.13107" N
DMS Longitude: 103° 49' 11.28000" E
---------------------------------------------------------------------------------------------
#2  Azimuth: 15°  Surface Distance: 10 km
...
```

(Target 1.3521, 103.8198, altitude 0 m, distance 10 km, no elevation angle; the first block is shown. With an elevation angle the header reads `#1  Azimuth: 0°  Elevation: 5°  Slant Range: 10 km`.)

- **Show:** chooses which coordinate types each block lists: **X, Y, Z**, **Lat, Lng, Altitude** and **DMS**. All three are ticked by default.
- If any input changes after Calculate, the status above the output says *Inputs changed. Press Calculate to refresh.*

### Replace Output

Tick **Replace Output With Below Text** to write your own text once per azimuth, instead of the summary line and the blocks above. For example, the text can be a line of a scenario file; it can also span several lines. **Show:** is greyed out while this is ticked. These placeholders (case-sensitive) are replaced each time:

| Placeholder | Value |
|---|---|
| `${x}`, `${y}`, `${z}` | Geocentric X, Y, Z in metres (4 decimals) |
| `${lat}`, `${lon}` | Latitude and longitude in degrees (8 decimals) |
| `${altitude}` | Altitude in metres (4 decimals) |
| `${azimuth}` | Azimuth in degrees, as printed in the header |
| `${runningNo}` | A running number that starts at **Running No** and goes up by 1 per azimuth. Running No is needed only when the text contains `${runningNo}`, and must be a whole number. |

For example, the text `Entity_${runningNo} ${lat} ${lon} ${altitude}` with Running No 1 gives `Entity_1 1.44253641 103.81980000 0.0000` for the first azimuth above.

## Requirements

- Windows (64-bit).
- To run the standalone build: nothing else. Make it with the publish command below. The `GeoCalculator.zip` in the repository holds an older build (August 2024). It lacks most of what is described here (azimuth Range, preview, Show, input checking), and the parts it does have, such as the elevation angle, work differently, so do not use it.
- To build from source: the .NET SDK 8 or later. The solution includes the unit tests, which target .NET 8 and need the .NET 8 Desktop Runtime to run. The app itself targets .NET 6 (`net6.0-windows`); newer SDKs build it with a NETSDK1138 "out of support" warning, which is expected. A normal build is written to `bin/Release/net6.0-windows/GeoCalculator.exe` and needs the .NET 6 Desktop Runtime to run.

## Build, test and publish

```bash
dotnet build GeoCalculator.sln -c Release
```

```bash
dotnet test GeoCalculator.Tests/GeoCalculator.Tests.csproj -c Release
```

To make a standalone single-file `GeoCalculator.exe` that runs without .NET installed (about 66 MB, written to `bin/Publish/win-x64`):

```bash
dotnet publish GeoCalculator.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None -o bin/Publish/win-x64
```

## Project layout

| Path | Contents |
|---|---|
| `Program.cs` | Entry point |
| `CalculatorForm.cs`, `CalculatorForm.Designer.cs` | The window, its two tabs and their handlers |
| `CalculatorForm.resx` | The window's icon |
| `Codes/Model/Coordinates.cs` | One point in geocentric and geodetic form |
| `Codes/Model/AzimuthPlan.cs` | The ordered set of azimuths built from a List or Range |
| `Codes/Util/ConverterUtil.cs` | Geocentric/geodetic conversion (GeographicLib) and DMS to degrees |
| `Codes/Util/DistanceUtil.cs` | Surface (geodesic) and slant-range (azimuth/elevation) calculations |
| `Codes/Util/AzimuthPlanBuilder.cs` | The List and Range rules |
| `Codes/Util/InputUtil.cs` | Parsing and checking of typed numbers |
| `Codes/Controls/AzimuthCompassPanel.cs`, `PreviewStyle.cs` | The compass preview and its colours |
| `GeoCalculator.Tests/` | Unit tests (xUnit) for the azimuth List and Range rules, compass point names and number echoing |
| `GeoCalculator.zip` | An older published exe (August 2024) |

## Accuracy notes

- Surface distances use GeographicLib's geodesic on the WGS84 ellipsoid, accurate to well under a millimetre at simulation distances. The printed output is limited by its 8 decimals (about 1 mm).
- Without an elevation angle the distance is a ground distance on the ellipsoid, so the straight line between the two points differs slightly from it (see [Conventions](#conventions)).
- Altitudes, typed and printed, are heights above the ellipsoid. If your scenario or terrain uses heights above mean sea level, convert the target altitude to HAE before you type it, and convert the printed altitudes back to MSL.
- The tool measures distance on the WGS84 ellipsoid (or as a straight line for slant ranges). A simulation that uses a flat-earth or UTM terrain database may measure the same two points a little differently.
