using UnityEngine;
using Shinies;

namespace ElementalElectricTree.Patches
{
    public class ShinySpawn_Shinify_Patches
    {
        public static void Shinify(ShinySpawn __instance)
        {
            if( __instance.gameObject.GetComponent<Identifiable>().id == Ids.ELECTRIC_SLIME)
            {
                __instance.SetPrivateField("curMid", Color.cyan);
                __instance.SetPrivateField("curTop", Color.cyan);
                __instance.SetPrivateField("curBot", Color.cyan);
                __instance.SetPrivateField("curColor", __instance.GetPrivateField<Color>("curTop"));

                __instance.SetColors();
            }

            if (__instance.gameObject.GetComponent<Identifiable>().id == Ids.FORM_2_ELECTRIC_SLIME)
            {
                __instance.SetPrivateField("curMid", Color.magenta);
                __instance.SetPrivateField("curTop", Color.magenta);
                __instance.SetPrivateField("curBot", Color.magenta);
                __instance.SetPrivateField("curColor", __instance.GetPrivateField<Color>("curTop"));

                __instance.SetColors();
            }
        }
    }
}
