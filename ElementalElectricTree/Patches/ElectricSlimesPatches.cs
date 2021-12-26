using HarmonyLib;

namespace ElementalElectricTree.Patches
{
	[HarmonyPatch(typeof(SlimeEmotions))]
	[HarmonyPatch("GetRecoveryFactor")]
	public static class ElectricSlimeRecovery
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00005DD8 File Offset: 0x00003FD8
		public static bool Prefix(SlimeEmotions __instance, ref float __result, SlimeEmotions.Emotion emotion)
		{
			bool flag = emotion == SlimeEmotions.Emotion.HUNGER && (Identifiable.GetId(__instance.gameObject) == Ids.ELECTRIC_SLIME || Identifiable.GetId(__instance.gameObject) == Ids.FORM_2_ELECTRIC_SLIME);
			bool result;
			if (flag)
			{
				__result = 1 / 3f;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}

	[HarmonyPatch(typeof(SlimeEat))]
	[HarmonyPatch("OnEat")]
	public static class ElectricSlimeHunger
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00005E14 File Offset: 0x00004014
		public static void Prefix(SlimeEat __instance, SlimeEmotions.Emotion driver)
		{
			bool flag = driver == SlimeEmotions.Emotion.HUNGER && (Identifiable.GetId(__instance.gameObject) == Ids.ELECTRIC_SLIME || Identifiable.GetId(__instance.gameObject) == Ids.FORM_2_ELECTRIC_SLIME);
			if (flag)
			{
				__instance.drivePerEat = 1f;
			}
		}
	}
}
