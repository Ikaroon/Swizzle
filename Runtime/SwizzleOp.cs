using UnityEngine;

namespace Ikaroon.Swizzle
{
	/// <summary>
	/// Swizzles any Unity Vector2/3/4 into another Vector2/3/4
	/// </summary>
	/// <remarks>
	/// In its current form this is approximately 2 to 3 times slower than manually changing the order of vector components.
	/// Keep that in mind if you're using this for a performance critical task.
	/// </remarks>
	public static partial class SwizzleOp
	{
		/// <summary>
		/// A 1D mask for swizzle operations
		/// </summary>
		public enum Mask : byte
		{
			X = 0, // 00
			Y = 1, // 01
			Z = 2, // 10
			W = 3, // 11
		}

		/// <summary>
		/// A 2D mask for swizzle operations
		/// </summary>
		public struct Mask2D
		{
			public Mask X { get; }
			public Mask Y { get; }

			public Mask2D(Mask x, Mask y)
			{
				X = x;
				Y = y;
			}

			public byte ToMask()
			{
				return (byte)((byte)X | ((byte)Y << 2));
			}
		}

		/// <summary>
		/// A 3D mask for swizzle operations
		/// </summary>
		public struct Mask3D
		{
			public Mask X { get; }
			public Mask Y { get; }
			public Mask Z { get; }

			public Mask3D(Mask x, Mask y, Mask z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public byte ToMask()
			{
				return (byte)((byte)X | ((byte)Y << 2) | ((byte)Z << 4));
			}
		}

		/// <summary>
		/// A 4D mask for swizzle operations
		/// </summary>
		public struct Mask4D
		{
			public Mask X { get; }
			public Mask Y { get; }
			public Mask Z { get; }
			public Mask W { get; }

			public Mask4D(Mask x, Mask y, Mask z, Mask w)
			{
				X = x;
				Y = y;
				Z = z;
				W = w;
			}

			public byte ToMask()
			{
				return (byte)((byte)X | ((byte)Y << 2) | ((byte)Z << 4) | ((byte)W << 6));
			}
		}

		// ==============================
		// ============= 2D =============
		// ==============================

		/// <summary>
		/// Instruction for a Vector2 to Vector2 swizzle operation
		/// </summary>
		public enum D2_D2 : byte
		{
			XX = 0,
			XY = 4,
			YX = 1,
			YY = 5,
		}

		/// <summary>
		/// Instruction for a Vector2 to Vector3 swizzle operation
		/// </summary>
		public enum D2_D3 : byte
		{
			XXX = 0,
			XXY = 16,
			XYX = 4,
			XYY = 20,
			YXX = 1,
			YXY = 17,
			YYX = 5,
			YYY = 21,
		}

		/// <summary>
		/// Instruction for a Vector2 to Vector4 swizzle operation
		/// </summary>
		public enum D2_D4 : byte
		{
			XXXX = 0,
			XXXY = 64,
			XXYX = 16,
			XXYY = 80,
			XYXX = 4,
			XYXY = 68,
			XYYX = 20,
			XYYY = 84,
			YXXX = 1,
			YXXY = 65,
			YXYX = 17,
			YXYY = 81,
			YYXX = 5,
			YYXY = 69,
			YYYX = 21,
			YYYY = 85,
		}

		// ==============================
		// ============= 3D =============
		// ==============================

		/// <summary>
		/// Instruction for a Vector3 to Vector2 swizzle operation
		/// </summary>
		public enum D3_D2 : byte
		{
			XX = 0,
			XY = 4,
			XZ = 8,
			YX = 1,
			YY = 5,
			YZ = 9,
			ZX = 2,
			ZY = 6,
			ZZ = 10,
		}

		/// <summary>
		/// Instruction for a Vector3 to Vector3 swizzle operation
		/// </summary>
		public enum D3_D3 : byte
		{
			XXX = 0,
			XXY = 16,
			XXZ = 32,
			XYX = 4,
			XYY = 20,
			XYZ = 36,
			XZX = 8,
			XZY = 24,
			XZZ = 40,
			YXX = 1,
			YXY = 17,
			YXZ = 33,
			YYX = 5,
			YYY = 21,
			YYZ = 37,
			YZX = 9,
			YZY = 25,
			YZZ = 41,
			ZXX = 2,
			ZXY = 18,
			ZXZ = 34,
			ZYX = 6,
			ZYY = 22,
			ZYZ = 38,
			ZZX = 10,
			ZZY = 26,
			ZZZ = 42,
		}

		/// <summary>
		/// Instruction for a Vector3 to Vector4 swizzle operation
		/// </summary>
		public enum D3_D4 : byte
		{
			XXXX = 0,
			XXXY = 64,
			XXXZ = 128,
			XXYX = 16,
			XXYY = 80,
			XXYZ = 144,
			XXZX = 32,
			XXZY = 96,
			XXZZ = 160,
			XYXX = 4,
			XYXY = 68,
			XYXZ = 132,
			XYYX = 20,
			XYYY = 84,
			XYYZ = 148,
			XYZX = 36,
			XYZY = 100,
			XYZZ = 164,
			XZXX = 8,
			XZXY = 72,
			XZXZ = 136,
			XZYX = 24,
			XZYY = 88,
			XZYZ = 152,
			XZZX = 40,
			XZZY = 104,
			XZZZ = 168,
			YXXX = 1,
			YXXY = 65,
			YXXZ = 129,
			YXYX = 17,
			YXYY = 81,
			YXYZ = 145,
			YXZX = 33,
			YXZY = 97,
			YXZZ = 161,
			YYXX = 5,
			YYXY = 69,
			YYXZ = 133,
			YYYX = 21,
			YYYY = 85,
			YYYZ = 149,
			YYZX = 37,
			YYZY = 101,
			YYZZ = 165,
			YZXX = 9,
			YZXY = 73,
			YZXZ = 137,
			YZYX = 25,
			YZYY = 89,
			YZYZ = 153,
			YZZX = 41,
			YZZY = 105,
			YZZZ = 169,
			ZXXX = 2,
			ZXXY = 66,
			ZXXZ = 130,
			ZXYX = 18,
			ZXYY = 82,
			ZXYZ = 146,
			ZXZX = 34,
			ZXZY = 98,
			ZXZZ = 162,
			ZYXX = 6,
			ZYXY = 70,
			ZYXZ = 134,
			ZYYX = 22,
			ZYYY = 86,
			ZYYZ = 150,
			ZYZX = 38,
			ZYZY = 102,
			ZYZZ = 166,
			ZZXX = 10,
			ZZXY = 74,
			ZZXZ = 138,
			ZZYX = 26,
			ZZYY = 90,
			ZZYZ = 154,
			ZZZX = 42,
			ZZZY = 106,
			ZZZZ = 170,
		}

		// ==============================
		// ============= 4D =============
		// ==============================

		/// <summary>
		/// Instruction for a Vector4 to Vector2 swizzle operation
		/// </summary>
		public enum D4_D2 : byte
		{
			XX = 0,
			XY = 4,
			XZ = 8,
			XW = 12,
			YX = 1,
			YY = 5,
			YZ = 9,
			YW = 13,
			ZX = 2,
			ZY = 6,
			ZZ = 10,
			ZW = 14,
			WX = 3,
			WY = 7,
			WZ = 11,
			WW = 15,
		}

		/// <summary>
		/// Instruction for a Vector4 to Vector3 swizzle operation
		/// </summary>
		public enum D4_D3 : byte
		{
			XXX = 0,
			XXY = 16,
			XXZ = 32,
			XXW = 48,
			XYX = 4,
			XYY = 20,
			XYZ = 36,
			XYW = 52,
			XZX = 8,
			XZY = 24,
			XZZ = 40,
			XZW = 56,
			XWX = 12,
			XWY = 28,
			XWZ = 44,
			XWW = 60,
			YXX = 1,
			YXY = 17,
			YXZ = 33,
			YXW = 49,
			YYX = 5,
			YYY = 21,
			YYZ = 37,
			YYW = 53,
			YZX = 9,
			YZY = 25,
			YZZ = 41,
			YZW = 57,
			YWX = 13,
			YWY = 29,
			YWZ = 45,
			YWW = 61,
			ZXX = 2,
			ZXY = 18,
			ZXZ = 34,
			ZXW = 50,
			ZYX = 6,
			ZYY = 22,
			ZYZ = 38,
			ZYW = 54,
			ZZX = 10,
			ZZY = 26,
			ZZZ = 42,
			ZZW = 58,
			ZWX = 14,
			ZWY = 30,
			ZWZ = 46,
			ZWW = 62,
			WXX = 3,
			WXY = 19,
			WXZ = 35,
			WXW = 51,
			WYX = 7,
			WYY = 23,
			WYZ = 39,
			WYW = 55,
			WZX = 11,
			WZY = 27,
			WZZ = 43,
			WZW = 59,
			WWX = 15,
			WWY = 31,
			WWZ = 47,
			WWW = 63,
		}

		/// <summary>
		/// Instruction for a Vector4 to Vector4 swizzle operation
		/// </summary>
		public enum D4_D4 : byte
		{
			XXXX = 0,
			XXXY = 64,
			XXXZ = 128,
			XXXW = 192,
			XXYX = 16,
			XXYY = 80,
			XXYZ = 144,
			XXYW = 208,
			XXZX = 32,
			XXZY = 96,
			XXZZ = 160,
			XXZW = 224,
			XXWX = 48,
			XXWY = 112,
			XXWZ = 176,
			XXWW = 240,
			XYXX = 4,
			XYXY = 68,
			XYXZ = 132,
			XYXW = 196,
			XYYX = 20,
			XYYY = 84,
			XYYZ = 148,
			XYYW = 212,
			XYZX = 36,
			XYZY = 100,
			XYZZ = 164,
			XYZW = 228,
			XYWX = 52,
			XYWY = 116,
			XYWZ = 180,
			XYWW = 244,
			XZXX = 8,
			XZXY = 72,
			XZXZ = 136,
			XZXW = 200,
			XZYX = 24,
			XZYY = 88,
			XZYZ = 152,
			XZYW = 216,
			XZZX = 40,
			XZZY = 104,
			XZZZ = 168,
			XZZW = 232,
			XZWX = 56,
			XZWY = 120,
			XZWZ = 184,
			XZWW = 248,
			XWXX = 12,
			XWXY = 76,
			XWXZ = 140,
			XWXW = 204,
			XWYX = 28,
			XWYY = 92,
			XWYZ = 156,
			XWYW = 220,
			XWZX = 44,
			XWZY = 108,
			XWZZ = 172,
			XWZW = 236,
			XWWX = 60,
			XWWY = 124,
			XWWZ = 188,
			XWWW = 252,
			YXXX = 1,
			YXXY = 65,
			YXXZ = 129,
			YXXW = 193,
			YXYX = 17,
			YXYY = 81,
			YXYZ = 145,
			YXYW = 209,
			YXZX = 33,
			YXZY = 97,
			YXZZ = 161,
			YXZW = 225,
			YXWX = 49,
			YXWY = 113,
			YXWZ = 177,
			YXWW = 241,
			YYXX = 5,
			YYXY = 69,
			YYXZ = 133,
			YYXW = 197,
			YYYX = 21,
			YYYY = 85,
			YYYZ = 149,
			YYYW = 213,
			YYZX = 37,
			YYZY = 101,
			YYZZ = 165,
			YYZW = 229,
			YYWX = 53,
			YYWY = 117,
			YYWZ = 181,
			YYWW = 245,
			YZXX = 9,
			YZXY = 73,
			YZXZ = 137,
			YZXW = 201,
			YZYX = 25,
			YZYY = 89,
			YZYZ = 153,
			YZYW = 217,
			YZZX = 41,
			YZZY = 105,
			YZZZ = 169,
			YZZW = 233,
			YZWX = 57,
			YZWY = 121,
			YZWZ = 185,
			YZWW = 249,
			YWXX = 13,
			YWXY = 77,
			YWXZ = 141,
			YWXW = 205,
			YWYX = 29,
			YWYY = 93,
			YWYZ = 157,
			YWYW = 221,
			YWZX = 45,
			YWZY = 109,
			YWZZ = 173,
			YWZW = 237,
			YWWX = 61,
			YWWY = 125,
			YWWZ = 189,
			YWWW = 253,
			ZXXX = 2,
			ZXXY = 66,
			ZXXZ = 130,
			ZXXW = 194,
			ZXYX = 18,
			ZXYY = 82,
			ZXYZ = 146,
			ZXYW = 210,
			ZXZX = 34,
			ZXZY = 98,
			ZXZZ = 162,
			ZXZW = 226,
			ZXWX = 50,
			ZXWY = 114,
			ZXWZ = 178,
			ZXWW = 242,
			ZYXX = 6,
			ZYXY = 70,
			ZYXZ = 134,
			ZYXW = 198,
			ZYYX = 22,
			ZYYY = 86,
			ZYYZ = 150,
			ZYYW = 214,
			ZYZX = 38,
			ZYZY = 102,
			ZYZZ = 166,
			ZYZW = 230,
			ZYWX = 54,
			ZYWY = 118,
			ZYWZ = 182,
			ZYWW = 246,
			ZZXX = 10,
			ZZXY = 74,
			ZZXZ = 138,
			ZZXW = 202,
			ZZYX = 26,
			ZZYY = 90,
			ZZYZ = 154,
			ZZYW = 218,
			ZZZX = 42,
			ZZZY = 106,
			ZZZZ = 170,
			ZZZW = 234,
			ZZWX = 58,
			ZZWY = 122,
			ZZWZ = 186,
			ZZWW = 250,
			ZWXX = 14,
			ZWXY = 78,
			ZWXZ = 142,
			ZWXW = 206,
			ZWYX = 30,
			ZWYY = 94,
			ZWYZ = 158,
			ZWYW = 222,
			ZWZX = 46,
			ZWZY = 110,
			ZWZZ = 174,
			ZWZW = 238,
			ZWWX = 62,
			ZWWY = 126,
			ZWWZ = 190,
			ZWWW = 254,
			WXXX = 3,
			WXXY = 67,
			WXXZ = 131,
			WXXW = 195,
			WXYX = 19,
			WXYY = 83,
			WXYZ = 147,
			WXYW = 211,
			WXZX = 35,
			WXZY = 99,
			WXZZ = 163,
			WXZW = 227,
			WXWX = 51,
			WXWY = 115,
			WXWZ = 179,
			WXWW = 243,
			WYXX = 7,
			WYXY = 71,
			WYXZ = 135,
			WYXW = 199,
			WYYX = 23,
			WYYY = 87,
			WYYZ = 151,
			WYYW = 215,
			WYZX = 39,
			WYZY = 103,
			WYZZ = 167,
			WYZW = 231,
			WYWX = 55,
			WYWY = 119,
			WYWZ = 183,
			WYWW = 247,
			WZXX = 11,
			WZXY = 75,
			WZXZ = 139,
			WZXW = 203,
			WZYX = 27,
			WZYY = 91,
			WZYZ = 155,
			WZYW = 219,
			WZZX = 43,
			WZZY = 107,
			WZZZ = 171,
			WZZW = 235,
			WZWX = 59,
			WZWY = 123,
			WZWZ = 187,
			WZWW = 251,
			WWXX = 15,
			WWXY = 79,
			WWXZ = 143,
			WWXW = 207,
			WWYX = 31,
			WWYY = 95,
			WWYZ = 159,
			WWYW = 223,
			WWZX = 47,
			WWZY = 111,
			WWZZ = 175,
			WWZW = 239,
			WWWX = 63,
			WWWY = 127,
			WWWZ = 191,
			WWWW = 255,
		}

		/// <summary>
		/// Converts the mask to the component index
		/// </summary>
		/// <param name="component">The vector component to use</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The index in the vector</returns>
		public static int GetCND(byte component, byte mask)
		{
			var compInt = (component * 2);
			var compMask = (3 << compInt);

			var result = ((mask & compMask) >> compInt);

			return result;
		}

		/// <summary>
		/// Converts the mask to the component indices in a Vector2
		/// </summary>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static (int x, int y) GetC2D(byte mask)
		{
			var x = (mask & 3);
			var y = ((mask & 12) >> 2);
			return (x, y);
		}

		/// <summary>
		/// Converts the mask to the component indices in a Vector3
		/// </summary>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static (int x, int y, int z) GetC3D(byte mask)
		{
			var x = (mask & 3);
			var y = ((mask & 12) >> 2);
			var z = ((mask & 48) >> 4);
			return (x, y, z);
		}

		/// <summary>
		/// Converts the mask to the component indices in a Vector4
		/// </summary>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static (int x, int y, int z, int w) GetC4D(byte mask)
		{
			var x = (mask & 3);
			var y = ((mask & 12) >> 2);
			var z = ((mask & 48) >> 4);
			var w = ((mask & 192) >> 6);
			return (x, y, z, w);
		}

		/// <summary>
		/// Calculates the byte mask for a swizzle operation
		/// </summary>
		/// <param name="x">The X component's source mask</param>
		/// <param name="y">The Y component's source mask</param>
		/// <returns>The byte mask for the swizzle operation</returns>
		public static byte GetMask(Mask x, Mask y)
		{
			return new Mask2D(x, y).ToMask();
		}

		/// <summary>
		/// Calculates the byte mask for a swizzle operation
		/// </summary>
		/// <param name="x">The X component's source mask</param>
		/// <param name="y">The Y component's source mask</param>
		/// <param name="z">The Z component's source mask</param>
		/// <returns>The byte mask for the swizzle operation</returns>
		public static byte GetMask(Mask x, Mask y, Mask z)
		{
			return new Mask3D(x, y, z).ToMask();
		}

		/// <summary>
		/// Calculates the byte mask for a swizzle operation
		/// </summary>
		/// <param name="x">The X component's source mask</param>
		/// <param name="y">The Y component's source mask</param>
		/// <param name="z">The Z component's source mask</param>
		/// <param name="w">The W component's source mask</param>
		/// <returns>The byte mask for the swizzle operation</returns>
		public static byte GetMask(Mask x, Mask y, Mask z, Mask w)
		{
			return new Mask4D(x, y, z, w).ToMask();
		}

		// ==============================
		// ============= 2D =============
		// ==============================

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector2 input, D2_D2 mask)
		{
			return To2D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector2 input, byte mask)
		{
			var comps = GetC2D(mask);
			return new Vector2(input[comps.x], input[comps.y]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector3 input, D3_D2 mask)
		{
			return To2D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector3 input, byte mask)
		{
			var comps = GetC2D(mask);
			return new Vector2(input[comps.x], input[comps.y]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector4 input, D4_D2 mask)
		{
			return To2D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector2
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector2 To2D(this Vector4 input, byte mask)
		{
			var comps = GetC2D(mask);
			return new Vector2(input[comps.x], input[comps.y]);
		}

		// ==============================
		// ============= 3D =============
		// ==============================

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector2 input, D2_D3 mask)
		{
			return To3D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector2 input, byte mask)
		{
			var comps = GetC3D(mask);
			return new Vector3(input[comps.x], input[comps.y], input[comps.z]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector3 input, D3_D3 mask)
		{
			return To3D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector3 input, byte mask)
		{
			var comps = GetC3D(mask);
			return new Vector3(input[comps.x], input[comps.y], input[comps.z]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector4 input, D4_D3 mask)
		{
			return To3D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector3
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector3 To3D(this Vector4 input, byte mask)
		{
			var comps = GetC3D(mask);
			return new Vector3(input[comps.x], input[comps.y], input[comps.z]);
		}

		// ==============================
		// ============= 4D =============
		// ==============================

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector2 input, D2_D4 mask)
		{
			return To4D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector2 input, byte mask)
		{
			var comps = GetC4D(mask);
			return new Vector4(input[comps.x], input[comps.y], input[comps.z], input[comps.w]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector3 input, D3_D4 mask)
		{
			return To4D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector3 input, byte mask)
		{
			var comps = GetC4D(mask);
			return new Vector4(input[comps.x], input[comps.y], input[comps.z], input[comps.w]);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector4 input, D4_D4 mask)
		{
			return To4D(input, (byte)mask);
		}

		/// <summary>
		/// Swizzle the vector <paramref name="input"/> to a Vector4
		/// </summary>
		/// <param name="input">The input vector</param>
		/// <param name="mask">The swizzle mask to convert</param>
		/// <returns>The indices for the vector</returns>
		public static Vector4 To4D(this Vector4 input, byte mask)
		{
			var comps = GetC4D(mask);
			return new Vector4(input[comps.x], input[comps.y], input[comps.z], input[comps.w]);
		}
	}
}
