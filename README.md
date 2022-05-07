![SwizzleOp](https://user-images.githubusercontent.com/65419234/167270835-2e53e0ad-20c4-469f-82a7-b3ab759419d0.png)

<p align=center><a href="https://github.com/Ikaroon/com.ikaroon.swizzle/blob/master/LICENSE"><img src="https://badgen.net/github/license/Naereen/Strapdown.js"/></a>
<a href="https://GitHub.com/Ikaroon/com.ikaroon.swizzle/releases/"><img src="https://img.shields.io/badge/Release-1.0.0-green.svg"/></a>
<a href="https://ko-fi.com/ikaroon"><img src="https://img.shields.io/badge/Donate-Ko--Fi-red.svg"/></a></p>

# Swizzle Operations
## Introduction
Swizzle operations are well known in shader languages like HLSL. However, they never made its way into Unity’s implementation of vectors.
This code is an attempt to implement this feature into unity in a reasonable way without cluttering code auto completion with hundreds of methods.

## Features
- [x] 2D to 2D, 3D, 4D
- [x] 3D to 2D, 3D, 4D
- [x] 4D to 2D, 3D, 4D
- [x] Custom masks
- [x] Bit based swizzle

## Installation

![PackageInstallation](https://user-images.githubusercontent.com/65419234/167270188-99300531-ec7e-45ea-89d9-612ec1d37eaf.png)
1. Open the package manager in Unity from `Window>Package Manager`
2. Select the `+` dropdown button in the top-left corner of the package manager
3. Select `Add package from git URL...`
4. Enter `https://github.com/Ikaroon/Swizzle.git` as url and confirm

## How it works
The idea behind this solution is the usage of enums. Each enum value defines a swizzle operation. However, instead of using a huge switch block to evaluate which order of vector components we need this attempt uses the underlying integer values to define the swizzle operation on a bit level.

For this we need 8 bits (1 byte) in which we group 2 bits together to define indices from 0 to 3 for each target component.
In the image you can see how the bits relate to the input and output vectors.

<img src="https://user-images.githubusercontent.com/65419234/161394579-9d6f6cb7-7fe0-4ab2-a635-acf7f3d08ab9.png" width="300" height="300">

Here is an example list of swizzle operations and the relating bit masks:

 Swizzle Operation | Bits 
-------------------|------------
XYZW               | 11 10 01 00
WWXX               | 00 00 11 11
XXXX               | 00 00 00 00
YWY                | 00 01 11 01

This data can then converted into indices on a vector like so:
```C#
public static (int x, int y, int z, int w) GetVector4Indices(byte mask)
{
	var x = (mask & 3);
	var y = ((mask & 12) >> 2);
	var z = ((mask & 48) >> 4);
	var w = ((mask & 192) >> 6);
	return (x, y, z, w);
}
```

## How to use
Given this HLSL code:
```HLSL
float3 vector = new float3(1, 2, 3);
float3 a = vector.yzx;
float3 b = vector.xxy;
float4 c = vector.zyxx;
```
You can copy this behaviour to C# like this:
```C#
var vector = new Vector3(1, 2, 3);
var a = vector.To3D(D3_D3.YZX);
var b = vector.To3D(D3_D3.XXY);
var c = vector.To4D(D3_D4.ZYXX);
```
You can also generate a swizzle mask like so:
```C#
var swizzleMask = new Mask3D(Mask.X, Mask.Z, Mask.Y);
var vector = new Vector3(1, 2, 3);
var result = vector.To3D(swizzleMask.ToMask());
```

## Performance and Use-Case
Even though this extension makes the code shorter it takes approximately double as long as a manual swizzle like this one:
```C#
var vector = new Vector3(1, 2, 3);
var a = new Vector3(vector.y, vector.z, vector.x);
var b = new Vector3(vector.x, vector.x, vector.y);
var c = new Vector4(vector.z, vector.y, vector.x, vector.x);
```
However, this might still be useful if you need user defined swizzle instructions. This might be a shader graph or remapping 3D vectors to a specific 2D plane.


## Notice
Work on this project happens in my freetime and, therefore, I cannot promise if and when certain features are added. I am considering to open this up for contribution but for now you can manipulate the code as you please. This project is MIT licensed and may be used freely. (Check the license file for more information)

## Donation
If this project helped you and you want to show gratitude, you can treat me to a coffee or so :D

[![Donate](https://img.shields.io/badge/Donate-Ko--Fi-red.svg)](https://ko-fi.com/ikaroon)
