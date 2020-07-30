using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;
using System.Collections;
using GameBase.Localization;
using Assets.Scripts.Utils.ExtensionMethods;

namespace I2.Loc
{
  public enum LocIDType 
  {
    [Description("_name")]
    name = 0,
    [Description("_desc")]
    description = 1,
    [Description("_map")]
    mapDesc = 1,

  }
    public static partial class LocalizationManager
    {

        #region Variables: Misc

        public delegate void OnLocalizeCallback();
        public static event OnLocalizeCallback OnLocalizeEvent;

        static bool mLocalizeIsScheduled = false;
        static bool mLocalizeIsScheduledWithForcedValue = false;

        public static bool HighlightLocalizedTargets = false;


    #endregion

    //========= cat mod ===========
    private static bool IsNull(this ILocalizationParamsManager lp)
    {
      return lp == null;
    }
    //---------------------------------------------------------------------------------------------------------------
    public static string Localize(this string st, LocIDType locID)
    {
      st = st + locID.GetDesc();
   //   
      return st.Localize();
    }
    //---------------------------------------------------------------------------------------------------------------
    public static string Localize(this string st, LocIDType locID, string p0)
    {
      st = st + locID.GetDesc();
      return st.Localize(p0);
    }

    //---------------------------------------------------------------------------------------------------------------
    public static string Localize(this string st)
    {
    
      string Translation = GetTranslation(st);
      return Translation;
    }

    //---------------------------------------------------------------------------------------------------------------
    public static string Localize(this string st, int p0)
    {
     return st.Localize(p0.ToString());
    }
    //---------------------------------------------------------------------------------------------------------------
      public static string Localize(this string st, string p0)
    {
      string Translation = null;
      ILocalizationParamsManager ParamManager = new LocParam() {p0 = p0};
      TryGetTranslation(st, out Translation, ParamManager, FixForRTL : true, maxLineLengthForRTL : 0, ignoreRTLnumbers : true, overrideLanguage : null);
      return Translation;
    }

    public static string Localize(this string st, string p0, string p1 = "", string p2 = "", string p3 = "", string p4 = "", string p5 = "")
    {
      string Translation = null;
      ILocalizationParamsManager ParamManager = new LocParam()
      { p0 = p0,
        p1 = p1,
        p2 = p2,
        p3 = p3,
        p4 = p4,
        p5 = p5

      };
      TryGetTranslation(st, out Translation, ParamManager, FixForRTL: true, maxLineLengthForRTL: 0, ignoreRTLnumbers: true, overrideLanguage: null);
      return Translation;
    }



    public static bool TryGetTranslation(string Term, out string Translation, ILocalizationParamsManager ParamManager = null, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, string overrideLanguage = null)
    {
      Translation = null;
      if (string.IsNullOrEmpty(Term))
      {
        return false;
      }
      

      InitializeIfNeeded();

      for (int i = 0, imax = Sources.Count; i < imax; ++i)
      {
        if (Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage))
        {
          if (!ParamManager.IsNull())
          {
  
            ApplyLocalizationParams(ref Translation, ParamManager, allowLocalizedParameters: true);
          }


          if (IsRight2Left && FixForRTL)
            Translation = ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
          return true;
        }
      }

      return false;
    }
    //=============================

    public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
        {
            string Translation = null;
            TryGetTranslation(Term, out Translation, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);

            return Translation;
        }
        public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
        {
            return GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage);
        }


        public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null)
        {
            Translation = null;
            if (string.IsNullOrEmpty(Term))
                return false;

            InitializeIfNeeded();

            for (int i = 0, imax = Sources.Count; i < imax; ++i)
            {
                if (Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage))
                {
          if (applyParameters)
          {
            ApplyLocalizationParams(ref Translation, localParametersRoot, allowLocalizedParameters: true);
          }
                        

                    if (IsRight2Left && FixForRTL)
                        Translation = ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
                    return true;
                }
            }

            return false;
        }

        public static T GetTranslatedObject<T>( string AssetName, Localize optionalLocComp=null) where T : Object
        {
            if (optionalLocComp != null)
            {
                return optionalLocComp.FindTranslatedObject<T>(AssetName);
            }
            else
            {
                T obj = FindAsset(AssetName) as T;
                if (obj)
                    return obj;

                obj = ResourceManager.pInstance.GetAsset<T>(AssetName);
                return obj;
            }
        }
        
        public static T GetTranslatedObjectByTermName<T>( string Term, Localize optionalLocComp=null) where T : Object
        {
            string    translation = GetTranslation(Term, FixForRTL: false);
            return GetTranslatedObject<T>(translation);
        }
        

        public static string GetAppName(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                for (int i = 0; i < Sources.Count; ++i)
                {
                    if (string.IsNullOrEmpty(Sources[i].mTerm_AppName))
                        continue;

                    int langIdx = Sources[i].GetLanguageIndexFromCode(languageCode, false);
                    if (langIdx < 0)
                        continue;

                    var termData = Sources[i].GetTermData(Sources[i].mTerm_AppName);
                    if (termData == null)
                        continue;

                    var appName = termData.GetTranslation(langIdx);
                    if (!string.IsNullOrEmpty(appName))
                        return appName;
                }
            }

            return Application.productName;
        }

        public static void LocalizeAll(bool Force = false)
		{
            LoadCurrentLanguage();

            if (!Application.isPlaying)
			{
				DoLocalizeAll(Force);
				return;
			}
			mLocalizeIsScheduledWithForcedValue |= Force;
            if (mLocalizeIsScheduled)
            {
                return;
            }
			I2.Loc.CoroutineManager.Start(Coroutine_LocalizeAll());
		}

		static IEnumerator Coroutine_LocalizeAll()
		{
			mLocalizeIsScheduled = true;
            yield return null;
            mLocalizeIsScheduled = false;
            var force = mLocalizeIsScheduledWithForcedValue;
			mLocalizeIsScheduledWithForcedValue = false;
			DoLocalizeAll(force);
		}

		static void DoLocalizeAll(bool Force = false)
		{
			Localize[] Locals = (Localize[])Resources.FindObjectsOfTypeAll( typeof(Localize) );
			for (int i=0, imax=Locals.Length; i<imax; ++i)
			{
				Localize local = Locals[i];
				//if (ObjectExistInScene (local.gameObject))
				local.OnLocalize(Force);
			}
			if (OnLocalizeEvent != null)
				OnLocalizeEvent ();
			//ResourceManager.pInstance.CleanResourceCache();
            #if UNITY_EDITOR
                RepaintInspectors();
            #endif
        }

        #if UNITY_EDITOR
        static void RepaintInspectors()
        {
            var assemblyEditor = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var typeInspectorWindow = assemblyEditor.GetType("UnityEditor.InspectorWindow");
            if (typeInspectorWindow != null)
            {
                typeInspectorWindow.GetMethod("RepaintAllInspectors", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, null);
            }
        }
        #endif


        public static List<string> GetCategories ()
		{
			List<string> Categories = new List<string> ();
			for (int i=0, imax=Sources.Count; i<imax; ++i)
				Sources[i].GetCategories(false, Categories);
			return Categories;
		}



		public static List<string> GetTermsList ( string Category = null )
		{
			if (Sources.Count==0)
				UpdateSources();

			if (Sources.Count==1)
				return Sources[0].GetTermsList(Category);

			HashSet<string> Terms = new HashSet<string> ();
			for (int i=0, imax=Sources.Count; i<imax; ++i)
				Terms.UnionWith( Sources[i].GetTermsList(Category) );
			return new List<string>(Terms);
		}

		public static TermData GetTermData( string term )
		{
            InitializeIfNeeded();

			TermData data;
			for (int i=0, imax=Sources.Count; i<imax; ++i)
			{
				data = Sources[i].GetTermData(term);
				if (data!=null)
					return data;
			}

			return null;
		}
        public static TermData GetTermData(string term, out LanguageSourceData source)
        {
            InitializeIfNeeded();

            TermData data;
            for (int i = 0, imax = Sources.Count; i < imax; ++i)
            {
                data = Sources[i].GetTermData(term);
                if (data != null)
                {
                    source = Sources[i];
                    return data;
                }
            }

            source = null;
            return null;
        }

    }
}
