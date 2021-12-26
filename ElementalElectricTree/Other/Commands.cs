using System;

using SRML.Console;
using Console = SRML.Console.Console;
using SRML.SR.Utils.Debug;
using System.Collections.Generic;

using UnityEngine;

using System.Linq;

namespace ElementalElectricTree.Other
{
    class PrintMaterialCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_material";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "PrintMaterial <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints the material of some slime";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            Material material;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                material = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(id)?.AppearancesDefault[0].Structures[0].DefaultMaterials[0];
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing: ", true);
            material.PrintContent();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return Identifiable.SLIME_CLASS.Select(x => x.ToString()).ToList();
        }
    }

    class PrintMaterialInfosCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_material_infos";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "PrintMaterialInfos [shaderIncluded]";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints the material's infos of a GameObject we are looking to and the shader too if you set the optional argument to true";

        RaycastHit mainHit;

        public override bool Execute(string[] args)
        {
            try
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                bool def = Physics.queriesHitTriggers;
                bool printShader = false;
                Physics.queriesHitTriggers = true;

                foreach (RaycastHit hit in Physics.RaycastAll(ray, 5f))
                {
                    if (hit.collider.GetComponent<DebugMarker>() != null)
                        hit.collider.GetComponent<DebugMarker>().SetHover();
                }

                Physics.queriesHitTriggers = def;

                Physics.Raycast(ray, out mainHit, 3000f);

                if(args != null)
                {
                    printShader = bool.Parse(args[0]);
                }

                if(mainHit.collider != null) {

                    if(mainHit.collider.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                    {
                        Console.Log("");
                        Console.Log("");
                        Console.Log("");

                        Console.Log("Printing Material: " + meshRenderer.sharedMaterial.name + " with Shader: " + meshRenderer.sharedMaterial.shader.name + " of Object: " + mainHit.collider.name);

                        if (printShader)
                        {
                            Console.Log("");

                            meshRenderer.sharedMaterial.shader.PrintContent();

                            Console.Log("");
                        }

                        Console.Log("");
                        Console.Log("");
                        Console.Log("");

                    }
                    else
                    {
                        Console.Log("No Material and/or Shader found in object " + mainHit.collider.name);
                    }

                }
                else
                {
                    Console.Log("No object found");
                }
            }
            catch(Exception e)
            {
                Console.Log(e.Message);
                return false;
            }

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return base.GetAutoComplete(argIndex, argText);
        }

    }

    class ChangeComponenPropertieCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "change_component_property";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "Change ComponentProperty <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Changes the property of a choiced components";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            GameObject prefab;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                prefab = GameContext.Instance.LookupDirector.GetPrefab(id);
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing: ", true);

            prefab.PrintComponent();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.GetPrivateField<PrefabList>("identifiablePrefabs").Select(x => Identifiable.GetId(x).ToString()).ToList();
        }
    }

    class PrintPlayerComponentsCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_players_components";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "Print Playyer's Components <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints Components of the Player";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Console.Log("Starting printing: ", true);

            SceneContext.Instance.Player.PrintComponent();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.GetPrivateField<PrefabList>("identifiablePrefabs").Select(x => Identifiable.GetId(x).ToString()).ToList();
        }
    }

    class PrintComponentsCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_components";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "Print Component <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints Components of something";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            GameObject prefab;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                prefab = GameContext.Instance.LookupDirector.GetPrefab(id);
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing: ", true);

            prefab.PrintComponent();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.GetPrivateField<PrefabList>("identifiablePrefabs").Select(x => Identifiable.GetId(x).ToString()).ToList();
        }
    }

    class PrintPlotComponentsCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_components_of_plot";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "PrintComponentOfPlot <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints Components of some Plot type";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            LandPlot.Id id;
            GameObject prefab;

            try
            {
                id = (LandPlot.Id)Enum.Parse(typeof(LandPlot.Id), args[0], true);
                prefab = GameContext.Instance.LookupDirector.GetPlotPrefab(id);
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing: ", true);

            prefab.PrintChildren();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            Func<GameObject, LandPlot.Id> GetId = (GameObject gameObject) => {
                LandPlot component = gameObject.GetComponent<LandPlot>();
                if (component != null)
                {
                    return component.typeId;
                }
                return LandPlot.Id.EMPTY;
            };

            return GameContext.Instance.LookupDirector.plotPrefabs.Select(x => GetId(x).ToString()).ToList();
        }
    }

    class PrintSlimeAppearanceContentCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_slimeappearance";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "Print SlimeAppearance's Content <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints the content of a SlimeAppearance";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            SlimeAppearance appearance;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                appearance = UnityEngine.Object.Instantiate(SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.GetChosenSlimeAppearance(id));
                if(id == Identifiable.Id.QUICKSILVER_SLIME)
                {
                    appearance = UnityEngine.Object.Instantiate(SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.GetChosenSlimeAppearance(Identifiable.Id.QUICKSILVER_SLIME).ShockedAppearance);
                    Console.Log("Edge case here, the appearance's name is: " + SRSingleton<SceneContext>.Instance.SlimeAppearanceDirector.GetChosenSlimeAppearance(Identifiable.Id.QUICKSILVER_SLIME).ShockedAppearance.name);
                }
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing appearance: " + appearance.name);

            int i3 = 0;
            int structureIndex = 0;
            foreach (SlimeAppearanceStructure structure in appearance.Structures)
            {
                int num1 = structure.Element.Prefabs.Length;
                Console.Log("StructureIndex: " + structureIndex);
                for (int j = 0; j < num1; j++)
                {
                    SlimeAppearanceObject prefab = structure.Element.Prefabs[j];
                    SlimeAppearanceObject component = SRML.Utils.PrefabUtils.CopyPrefab(prefab.gameObject).GetComponent<SlimeAppearanceObject>();

                    var returnvalue = prefab.gameObject.PrintParent();
                    if (returnvalue != null)
                        returnvalue.PrintParent();
                    Console.Log("");
                    Console.Log("");
                    Console.Log("Prefab in custom mesh is: " + prefab);
                    prefab.PrintComponents<Component>();
                    Console.Log("");
                    Console.Log("");
                    if (component.TryGetComponent<MeshFilter>(out MeshFilter filter))
                    {
                        Console.Log("");
                        Console.Log("Printing MeshFilter");
                        Console.Log(filter.ToString());
                        Console.Log("");
                    }

                    if (component.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    {
                        Console.Log("");
                        Console.Log("Printing MeshRenderer");
                        Console.Log(renderer.ToString());
                        Console.Log(renderer.material.ToString());
                        Console.Log(renderer.material.GetColor(Shader.PropertyToID("_TopColor")).ToString());
                        Console.Log(renderer.material.GetColor(Shader.PropertyToID("_MiddleColor")).ToString());
                        Console.Log(renderer.material.GetColor(Shader.PropertyToID("_BottomColor")).ToString());
                        //Console.Log(renderer.material.GetTexture(Shader.PropertyToID("_CubemapOverride")).name);

                        Console.Log("");
                    }

                    if (component.TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer skinned))
                    {
                        Console.Log("");
                        Console.Log("Printing SkinnedMeshRenderer");
                        Console.Log(skinned.ToString());
                        Console.Log(skinned.sharedMesh.ToString());
                        Console.Log(skinned.material.GetColor(Shader.PropertyToID("_TopColor")).ToString());
                        Console.Log(skinned.material.GetColor(Shader.PropertyToID("_MiddleColor")).ToString());
                        Console.Log(skinned.material.GetColor(Shader.PropertyToID("_BottomColor")).ToString());
                        //Console.Log(skinned.material.GetTexture(Shader.PropertyToID("_CubemapOverride")).name);
                        Console.Log("");
                    }

                    component.PrintComponents<Component>();

                    //appearance.Structures[1].Element.Prefabs[i3] = component;

                    i3++;
                }
                structureIndex++;
            }

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.GetSlimeIDs().Select(x => x.ToString()).ToList();
        }
    }

    class PrintLODContent : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_slime_LOD";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "Print Slime's LOD's Content <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints the content of the LOD Group of a slime";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            LODGroup lodGroup;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                lodGroup = GameContext.Instance.LookupDirector.GetPrefab(id).GetComponentInChildren<LODGroup>();
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing LOD Group");

            foreach (LOD lod in lodGroup.GetLODs())
            {
                Console.Log("Being at LOD: " + lod);
                foreach (Renderer renderer in lod.renderers)
                {
                    Console.Log(renderer.ToString());
                    try
                    {
                        Console.Log(renderer.gameObject.ToString());
                    }
                    catch (Exception)
                    {

                        Console.Log("LOD failed");
                    }

                }
            }

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.GetSlimeIDs().Select(x => x.ToString()).ToList();
        }
    }

}