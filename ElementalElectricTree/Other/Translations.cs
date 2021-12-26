using SRML.SR;
using SRML.SR.Translation;

namespace ElementalElectricTree.Other
{
    class Translations
    {
        public static void Translate()
        {
            new SlimePediaEntryTranslation(Ids.ELECTRIC_SLIME_ENTRY)
                .SetTitleTranslation("Electric Slime (Form 1)")
                .SetIntroTranslation("An dangerous slime with a dangerous high volt, watch out! This is the native form of the electric slimes.")
                .SetDietTranslation("Veggies, Fruits and Meat")
                .SetFavoriteTranslation("Electric Veggies/Fruits/Meat")
                .SetSlimeologyTranslation("This slime is really mysterious. It seems to emmit a high concentration of electricity, and when it gets angry it can concentrate all his energy in one, unique electric ball that is probably the deadliest shoot a slime can do! They seem to be made out of pure electricity, because they reacted to all electricity tests we made with them. And they have to be powerfull, because they can shock quicksilvers! Such a slime was never be seen before, and by its form we can think it's one of the ancestors of the quicksilvers! Then, it seem to react to the energy of the energy reactor in the Nimble Valley, so when they are there, they take the form of a arrowshaped slime, also called \"Quicksilver form\". In this form the slime is way faster than it's cousins, the Quicksilver slimes, can jump higher and has 1 extra ability, in wich it transforms itself into a electricity arrow, this is one of the biggest mysteries of evolution! Who knows how many secrets he can learn from it and it's origins...")
                .SetRisksTranslation("Because of all the electricity, touching this slime can hurt the rancher and, if you are unlucky, even electroshocking you(the same effects of beign stunned, only that you need more time to recover and it dosen't recover immediately). This slime can shoot extrelmy powerfull electroshock balls to the rancher, to avoid at all costs (WIP In the future it will be able to disable corrals, so think twice where you place it)!")
                .SetPlortonomicsTranslation("A good energy source that can be used to create new types of advanced machines, Mochi and Viktor and their new machines are the perfect example for it. But how exactly this plort works is unknown to us. \"It seems to contain the essence of a lightning\" tells us Thora");

            new SlimePediaEntryTranslation(Ids.FORM_2_ELECTRIC_SLIME_ENTRY)
                   .SetTitleTranslation("Electric Slime (Form 2)")
                   .SetIntroTranslation("An dangerous slime with a dangerous high volt, watch out! This is the morphed, adapted form of the electric slimes.")
                   .SetDietTranslation("Veggies, Fruits and Meat")
                   .SetFavoriteTranslation("Electric Veggies/Fruits/Meat")
                   .SetSlimeologyTranslation("This slime is really mysterious. It seems to emmit a high concentration of electricity, and when it gets angry it can concentrate all his energy in one, unique electric ball that is probably the deadliest shoot a slime can do! They seem to be made out of pure electricity, because they reacted to all electricity tests we made with them. And they have to be powerfull, because they can shock quicksilvers! Such a slime was never be seen before, and by its form we can think it's one of the ancestors of the quicksilvers! Then, they seem to react to the energy of the energy reactor in the Nimble Valley, so when they aren't there, they take the form of a normal slime. In this form the slime is way slower than it's cousins, the Quicksilver slimes, but it can jump higher and has more powerfull attacks to defend itself, like creating a single, big \"cannon\" shot, creating a shockwave that electrocutes every nearby living creature, deactivating corrals and more that we still need to know, This is one of the biggest mysteries of evolution! Who knows how many secrets he can learn from it and it's origins...")
                   .SetRisksTranslation("Because of all the electricity, touching this slime can hurt the rancher and, if you are unlucky, even electroshocking you(the same effects of beign stunned, only that you need more time to recover and it dosen't recover immediately). This slime can shoot extrelmy powerfull electroshock balls to the rancher, to avoid at all costs (WIP In the future it will be able to disable corrals, so think twice where you place it)!")
                   .SetPlortonomicsTranslation("A good energy source that can be used to create new types of advanced machines, Mochi and Viktor and their new machines are the perfect example for it. But how exactly this plort works is unknown to us. \"It seems to contain the essence of a lightning\" tells us Thora");

            TranslationPatcher.AddPediaTranslation("m.upgrade.name.corral.electrometer", "ElectroMeter");
            TranslationPatcher.AddPediaTranslation("m.upgrade.desc.corral.electrometer", "A regulerator, created from the \"Miles Tech\" that allows to ranch electric slimes. It limits their powers and makes them containable.");

            TranslationPatcher.AddTranslationKey("exchange", "m.offer.mochi_rewards_level2.intro.1", "INTRO, GO BRRRRR");
            TranslationPatcher.AddTranslationKey("exchange", "m.offer.mochi_rewards_level2.repeat.1", "REPEAT, GO BRRRRR");

            TranslationPatcher.AddActorTranslation("l.electric_plort", "Electric Plort");

            TranslationPatcher.AddActorTranslation("l.secret_style_electric_slime", "Blue Electric Slime");
        }
    }
}
