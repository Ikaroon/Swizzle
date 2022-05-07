using System.Text;

namespace Ikaroon.Swizzle.Generation
{
	/// <summary>
	/// This is only included for transparency.
	/// Once this is updated to unity 2021.2 or higher this will be replaced with a Rosalyn SourceGenerator.
	/// </summary>
	internal class SwizzleGeneration
	{
		static string[] m_compToName = new string[] { "X", "Y", "Z", "W" };

		public string GetEnums()
		{
			string source = $@"
				{GenerateEnum(2, 2)}
				{GenerateEnum(2, 3)}
				{GenerateEnum(2, 4)}
				{GenerateEnum(3, 2)}
				{GenerateEnum(3, 3)}
				{GenerateEnum(3, 4)}
				{GenerateEnum(4, 2)}
				{GenerateEnum(4, 3)}
				{GenerateEnum(4, 4)}";

			return source;
		}

		public string GenerateEnum(int source, int target)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"public enum D{source}_D{target} : byte");
			sb.AppendLine("{");
			GetValues(sb, string.Empty, 0, 0, target, source);
			sb.AppendLine("}");
			return sb.ToString();
		}

		void GetValues(StringBuilder sb, string name, byte value, int level, int maxLevel, int availableComponents)
		{
			if (level == maxLevel)
			{
				sb.AppendLine($"{name} = {value},");
				return;
			}

			var actualLevel = level * 2;

			for (int i = 0; i < availableComponents; i++)
			{
				string subName = $"{name}{m_compToName[i]}";
				byte subValue = (byte)(i << actualLevel | value);
				GetValues(sb, subName, subValue, level + 1, maxLevel, availableComponents);
			}
		}
	}
}
