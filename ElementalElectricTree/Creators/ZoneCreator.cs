using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SRML.Utils;
using ElementalElectricTree.Other;
using UnityEngine;

namespace ElementalElectricTree.Creators
{
    public class LoadZone
    {
        public class MappingSave
        {
            // Token: 0x04000041 RID: 65

            public string Category { get; set; }
            public string Path { get; set; }
            public bool GadgetSite { get; set; }
            public bool AccessDoor { get; set; }
            public bool LiquidSource { get; set; }

            public string Teleportation { get; set; }
            public string Source { get; set; }

            public string HobsonNote { get; set; }



            public Vector3Save Position { get; set; }
            public Vector3Save Rotation { get; set; }
            public Vector3Save Scale { get; set; }
        }
        public class Vector3Save
        {
            // Token: 0x06000141 RID: 321 RVA: 0x0000D448 File Offset: 0x0000B648
            public Vector3Save(Vector3 vector)
            {
                this.x = vector.x;
                this.y = vector.y;
                this.z = vector.z;
            }

            // Token: 0x06000142 RID: 322 RVA: 0x0000D480 File Offset: 0x0000B680
            public Vector3 ToVector3()
            {
                return new Vector3(this.x, this.y, this.z);
            }

            // Token: 0x0400006A RID: 106
            public float x;

            // Token: 0x0400006B RID: 107
            public float y;

            // Token: 0x0400006C RID: 108
            public float z;

            public override string ToString()
            {
                string o = x + "f, " + y + "f, " + z + "f";

                return o;
            }
        }
        internal static Transform ParentCreating(string category, GameObject gameObject)
        {
            if (!gameObject.transform.Find(category))
            {
                GameObject categoryGameObject = new GameObject(category)
                {
                    transform =
                    {
                        parent = gameObject.transform
                    }
                };
                return categoryGameObject.transform;
            }
            if (gameObject.transform.Find(category))
            {
                return gameObject.transform.Find(category);
                //go.transform.parent = cellMud_Start.transform.Find(VARIABLE.Value.Category).transform;
            }

            return null;
        }
        internal static void Load(GameObject Sector, string file)
        {
            var json = JsonConvert.DeserializeObject<Dictionary<ulong, MappingSave>>(file);
            foreach (var variable in json)
            {
                ShortCutter.Log(variable.Value.Path);
                ShortCutter.Log(variable.Value.Category);
                if (variable.Value.Category == "Constructs" && variable.Value.Path == "zoneDESERT/cellDesert_WaystationTempleEndOutside/Sector/Constructs/UnderConstructionBarrier")
                    continue;

                if (variable.Value.AccessDoor)
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    if (VariableName.GetComponentsInChildren<Replacer>().Length == 0) VariableName.AddComponent<Replacer>();
                    VariableName.GetComponent<Replacer>().BuildAccessDoor();
                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }
                else if (!string.IsNullOrEmpty(variable.Value.HobsonNote))
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    //VariableName.GetComponentsInChildren<Replacer>().Length.Log();
                    if (VariableName.GetComponentsInChildren<Replacer>().Length == 0)
                        VariableName.AddComponent<Replacer>();
                    VariableName.GetComponent<Replacer>().BuildHobsonNote(variable.Value.HobsonNote);

                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }
                else if (!string.IsNullOrEmpty(variable.Value.Teleportation))
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    //VariableName.GetComponentsInChildren<Replacer>().Length.Log();
                    if (VariableName.GetComponentsInChildren<Replacer>().Length == 0)
                        VariableName.AddComponent<Replacer>();
                    VariableName.GetComponent<Replacer>().BuildTeleport(variable.Value.Teleportation, variable.Value.Source);

                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }

                else if (variable.Value.LiquidSource)
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    if (VariableName.GetComponentsInChildren<Replacer>().Length == 0) VariableName.AddComponent<Replacer>();
                    VariableName.GetComponent<Replacer>().BuildLiquidSource();
                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }

                else if (variable.Value.GadgetSite)
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    if (VariableName.GetComponentsInChildren<Replacer>().Length == 0) VariableName.AddComponent<Replacer>();
                    VariableName.GetComponent<Replacer>().BuildGadgetSite();
                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }
                else
                {
                    GameObject VariableName = GameObjectUtils.InstantiateInactive(GameObject.Find(variable.Value.Path));
                    VariableName.transform.SetParent(LoadZone.ParentCreating(variable.Value.Category, Sector));
                    VariableName.transform.localPosition = variable.Value.Position.ToVector3();
                    var VariablelocalRotation = VariableName.transform.localRotation;
                    VariablelocalRotation.eulerAngles = variable.Value.Rotation.ToVector3();
                    VariableName.transform.localRotation = VariablelocalRotation;
                    VariableName.transform.localScale = variable.Value.Scale.ToVector3();
                    VariableName.SetActive(true);
                }
            }
        }
    }
}