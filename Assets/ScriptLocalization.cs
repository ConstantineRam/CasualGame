using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static string map_editor_warining_file_already_exists 		{ get{ return LocalizationManager.GetTranslation ("map_editor_warining_file_already_exists"); } }
		public static string Off 		{ get{ return LocalizationManager.GetTranslation ("Off"); } }
		public static string On 		{ get{ return LocalizationManager.GetTranslation ("On"); } }
	}

    public static class ScriptTerms
	{

		public const string map_editor_warining_file_already_exists = "map_editor_warining_file_already_exists";
		public const string Off = "Off";
		public const string On = "On";
	}
}