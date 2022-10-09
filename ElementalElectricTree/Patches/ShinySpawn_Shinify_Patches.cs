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
                __instance.SetPrivateField("curMid", Color.white);
                __instance.SetPrivateField("curTop", Color.white);
                __instance.SetPrivateField("curBot", Color.white);
                __instance.SetPrivateField("curColor", __instance.GetPrivateField<Color>("curTop"));

                __instance.SetColors();
            }

            if (__instance.gameObject.GetComponent<Identifiable>().id == Ids.FORM_2_ELECTRIC_SLIME)
            {
                __instance.SetPrivateField("curMid", Color.green);
                __instance.SetPrivateField("curTop", Color.green);
                __instance.SetPrivateField("curBot", Color.green);
                __instance.SetPrivateField("curColor", __instance.GetPrivateField<Color>("curTop"));

                __instance.SetColors();
            }
        }
    }
}
