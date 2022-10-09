using Shinies;
using UnityEngine;
using SRML.Console;

namespace ElementalElectricTree.Patches
{
    public class ShinySpawn_ShinyCheck_Patches
    {
        public static bool ShinyCheck(ShinySpawn __instance, ref ShinySpawn.Skin __result)
        {
            Identifiable.Id id = Identifiable.GetId(__instance.gameObject);

            if (id == Ids.ELECTRIC_SLIME || id == Ids.FORM_2_ELECTRIC_SLIME)
            {
                int num = Random.Range(1, 4);
                bool flag = num == 1;
                ShinySpawn.Skin result = ShinySpawn.Skin.Normal;
                if (flag)
                {
                    result = ShinySpawn.Skin.Shiny;
                }

                __result = result;
                return false;
            }

            return true;
        }

    }
}
