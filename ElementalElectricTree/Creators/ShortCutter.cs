using UnityEngine;
using SRML.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ElementalElectricTree
{
    public class ShortCutter
    {
        public static void Log(object message) => SRML.Console.Console.Log(message.ToString());

/*        public static void RegisterSecretStyle(Identifiable.Id slimeId, SlimeAppearance secretAppearance, SlimeDefinition slimeDefinition, string translationName)
        {
            Log(secretAppearance);
            Log(slimeDefinition);
            Log(slimeId);
            Log(translationName);
            
            secretAppearance.NameXlateKey = "l.secret_style_electric_slime";
            SRML.SR.TranslationPatcher.AddActorTranslation("l.secret_style_electric_slime", translationName);

            secretAppearance.SaveSet = SlimeAppearance.AppearanceSaveSet.SECRET_STYLE;

            GameContext.Instance.DLCDirector.onPackageInstalled += delegate(DLCPackage.Id package)
            {
                if (package != DLCPackage.Id.SECRET_STYLE)
                    return;

                Log("SECRET STYLES LOADED");
                slimeDefinition.RegisterDynamicAppearance(secretAppearance);
                Log("I am still alive ?");
                //SceneContext.Instance.SlimeAppearanceDirector.UnlockAppearance(slimeDefinition, secretAppearance);
            };
        }
*/
    }
}
