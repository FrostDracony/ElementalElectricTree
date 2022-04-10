using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Console = SRML.Console.Console;
using System.Linq;

namespace ElementalElectricTree.Other
{
    public static class HelperClass
    {

        /*public static void ExpelAmmo(this WeaponVacuum weaponVacuum, int decrementAmount)
        {
            GameObject selectedStored = weaponVacuum.player.Ammo.GetSelectedStored();
            Identifiable component = selectedStored.GetComponent<Identifiable>();

            weaponVacuum.Expel(selectedStored, false);
            weaponVacuum.player.Ammo.DecrementSelectedAmmo(decrementAmount);

            if (component != null)
            {
                weaponVacuum.tutDir.OnShoot(component.id);
            }

            weaponVacuum.ShootEffect();
        }*/

        public static void Log(object message) => Console.Log(message.ToString());

        public static GameObject FindChild(this GameObject @object, string childsName) => @object.transform.Find(childsName).gameObject;

        public static RancherChatMetadata.Entry[] CreateRancherChatConversation(this ExchangeDirector exchangeDirector, string rancherId, string[] messages)
        {
            List<RancherChatMetadata.Entry> entryToReturn = new List<RancherChatMetadata.Entry>();
            Array.ForEach(messages, text =>
            {
                entryToReturn.Add(new RancherChatMetadata.Entry
                {
                    rancherName = (RancherChatMetadata.Entry.RancherName)Enum.Parse(typeof(RancherChatMetadata.Entry.RancherName), rancherId.ToUpperInvariant()),
                    rancherImage = exchangeDirector.GetRancherImage(rancherId),
                    messageBackground = exchangeDirector.GetRancher(rancherId).chatBackground,
                    messageText = text
                });

            });

            return entryToReturn.ToArray();
        }

        public static RancherChatMetadata.Entry[] CreateRancherChatConversation(this ExchangeDirector exchangeDirector, string rancherId, string[] messages, Sprite[] sprites)
        {
            List<RancherChatMetadata.Entry> entryToReturn = new List<RancherChatMetadata.Entry>();
            int i = 0;
            Array.ForEach(messages, text =>
            {
                entryToReturn.Add(new RancherChatMetadata.Entry
                {
                    rancherName = (RancherChatMetadata.Entry.RancherName)Enum.Parse(typeof(RancherChatMetadata.Entry.RancherName), rancherId.ToUpperInvariant()),
                    rancherImage = sprites[i],
                    messageBackground = exchangeDirector.GetRancher(rancherId).chatBackground,
                    messageText = text
                });

                i++;
            });

            return entryToReturn.ToArray();
        }

        public static RancherChatMetadata.Entry CreateRancherChatMetadataEntry(this ExchangeDirector exchangeDirector, string rancherId, string message)
        {
            return new RancherChatMetadata.Entry
            {
                rancherName = (RancherChatMetadata.Entry.RancherName)Enum.Parse(typeof(RancherChatMetadata.Entry.RancherName), rancherId.ToUpperInvariant()),
                rancherImage = exchangeDirector.GetRancherImage(rancherId),
                messageBackground = exchangeDirector.GetRancher(rancherId).chatBackground,
                messageText = message
            };
        }

        public static RancherChatMetadata CreateRancherChat(this ExchangeDirector exchangeDirector, string rancherId, string[] messages)
        {
            List<RancherChatMetadata.Entry> entryToReturn = new List<RancherChatMetadata.Entry>();

            Array.ForEach(messages, text =>
            {
                entryToReturn.Add(new RancherChatMetadata.Entry
                {
                    rancherName = (RancherChatMetadata.Entry.RancherName)Enum.Parse(typeof(RancherChatMetadata.Entry.RancherName), rancherId.ToUpperInvariant()),
                    rancherImage = exchangeDirector.GetRancherImage(rancherId),
                    messageBackground = exchangeDirector.GetRancher(rancherId).chatBackground,
                    messageText = text
                });

            });

            RancherChatMetadata rancherChatMetadata = ScriptableObject.CreateInstance<RancherChatMetadata>();
            rancherChatMetadata.entries = entryToReturn.ToArray();

            return rancherChatMetadata;
        }

        public static void PrintChildren(this GameObject gameObject)
        {
            Component[] allChildren = gameObject.GetComponents<Component>();
            foreach (Component child in allChildren)
            {
                Console.Log(child.ToString());
                if (child.TryGetComponent<Component>(out Component transform))
                {
                    foreach (Component child2 in child.gameObject.GetComponentsInChildren<Component>())
                    {
                        Console.Log("   " + child2);
                    }
                }
            }
        }

        public static Vector3 EnsureNotShootingIntoRock(Vector3 startPos, Ray ray, float objRad, ref Vector3 vel)
        {
            float d = 0.5f;
            Ray ray2 = new Ray(ray.origin - ray.direction * d, ray.direction);
            float magnitude = (startPos - ray2.origin).magnitude;
            int layerMask = 270572033;
            RaycastHit raycastHit;
            Physics.Raycast(ray2, out raycastHit, magnitude, layerMask, QueryTriggerInteraction.Ignore);
            if (raycastHit.collider != null)
            {
                startPos = raycastHit.point - ray.direction * objRad;
                vel -= Vector3.Project(vel, raycastHit.normal);
            }
            return startPos;
        }

        public static IEnumerator Wait(int delay, bool ready)
        {
            int i = 0;
            while (i < delay++) {
                ready = i == delay;

                if (ready)
                    break;

                yield return new WaitForSeconds(1);
                i++;
            }
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static bool HasComponent<T>(this GameObject obj) where T:Component
        {
            return obj.TryGetComponent<T>(out T returnValue);
        }
        public static void PrintComponents<T>(this GameObject obj) where T : Component
        {
            Console.Log("Printing components of: " + obj.ToString());
            foreach (Component component in obj.GetComponentsInChildren<T>())
            {
                Console.Log("   " + component);
            }
            Console.Log("End");
        }

        public static bool FindContentOfType(this Array array, Type type)
        {
            Console.Log("FindContentOfType                                                  yes");
            foreach (var item in array)
            {
                Console.Log("item: " + item);

                if (item.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static void PrintComponents<T>(this SlimeAppearanceObject obj) where T : Component
        {
            Console.Log("Printing components of: " + obj.ToString());
            foreach (Component component in obj.GetComponentsInChildren<T>())
            {
                Console.Log("   " + component);
            }
            Console.Log("End");
        }

        public static void PrintComponent(this GameObject obj)
        {
            Console.Log("");
            Console.Log("");
            foreach (Component component in obj.GetComponentsInChildren<Component>(true))
            {
                Console.Log("   " + component.ToString());
            }
            Console.Log("");
            Console.Log("");
        }

        public static GameObject PrintParent(this GameObject gameObject)
        {
            if (gameObject.transform.parent != null)
            {
                Console.Log(gameObject.ToString() + "Parent's is: " + gameObject.transform.parent.gameObject.ToString());
                return gameObject.transform.parent.gameObject;
            }
            return null;
        }
        public static bool FindContent(this Array obj, Identifiable.Id comparator) //This is an extension (it's called so in C#) for arrays that allows me to search if something is in that array or not (without erroring).
        {
            foreach (Collider content in obj)
            {
                if (content.gameObject.transform.parent.gameObject.GetComponentInChildren<Identifiable>().id == comparator)
                {
                    return true;
                }
            }
            return false;
        }

        public static GameObject GetContent(this Array obj, Identifiable.Id comparator)
        {
            foreach (Collider content in obj)
            {
                if (content.gameObject.transform.parent.gameObject.GetComponentInChildren<Identifiable>().id == comparator)
                {
                    return content.gameObject;
                }
            }
            return null;
        }

        public static GameObject PrintParent(this SlimeAppearanceObject slimeAppearanceObject)
        {
            if (slimeAppearanceObject.transform.parent != null)
            {
                Console.Log(slimeAppearanceObject.ToString() + "Parent's is: " + slimeAppearanceObject.transform.parent.gameObject.ToString());
                return slimeAppearanceObject.transform.parent.gameObject;
            }
            return null;
        }

        public static bool HasComponentOnlyBool<T>(this Component obj) where T : Component
        {
            bool Bool = obj.TryGetComponent<T>(out T returnValue);
            return Bool;
        }

        public static (bool,T) HasComponent<T>(this Component obj) where T : Component
        {
            bool Bool = obj.TryGetComponent<T>(out T returnValue);
            return (Bool,returnValue);
        }

        public static void PrintContent<T>(this Array array)
        {
            //bool Bool = obj.TryGetComponent<T>(out T returnValue);
            //return (Bool, returnValue);
            //Console.Log("Printing Array!");
            foreach(T content in array)
            {
                Console.Log(content.ToString());
            }

        }

        public static void PrintContent(this Array array)
        {
            //bool Bool = obj.TryGetComponent<T>(out T returnValue);
            //return (Bool, returnValue);
            //Console.Log("Printing Array!");
            foreach (var content in array)
            {
                Console.Log(content.ToString());
            }

        }

        /*
         for (int i2 = 0; i2 < material.shader.GetPropertyCount(); i2++)
                    {
                        Console.Log(material.shader.GetPropertyName(i2) + ", " + material.shader.GetPropertyDescription(i2) + " : " + material.shader.GetPropertyType(i2));
                        foreach(string str in material.shader.GetPropertyAttributes(i2))
                        {
                            Console.Log("   " + str);
                        }
                    }
         */

        public static void PrintContent(this Material material, bool parameter = false)
        {
            if (parameter)
            {
                material.shader.PrintContent(material);
            }
            else
            {
                material.shader.PrintContent();
            }
        }

        public static void PrintContent(this Shader shader, Material material = null)
        {
            Console.Log("");
            Console.Log("");

            if (material == null)
                Console.Log("Printing Shader: " + shader.name);
            if (material != null)
                Console.Log("Printing Shader: " + shader.name + " of Material: " + material);

            for (int i2 = 0; i2 < shader.GetPropertyCount(); i2++)
            {
                if(material != null)
                    Console.Log(shader.GetPropertyName(i2) + ", " + shader.GetPropertyDescription(i2) + " : " + shader.GetPropertyType(i2) + " = " + material.GetType().GetMethod("Get" + shader.GetPropertyType(i2)));
                if(material == null)
                    Console.Log(shader.GetPropertyName(i2) + ", " + shader.GetPropertyDescription(i2) + " : " + shader.GetPropertyType(i2));
                foreach (string str in shader.GetPropertyAttributes(i2))
                {
                    Console.Log("   " + str);
                }
            }

            Console.Log("");
            Console.Log("");

        }

        public static T CreatePrefab<T>(this T obj) where T : UnityEngine.Object => UnityEngine.Object.Instantiate<T>(obj, Main.prefabParent, false);

        public static T Copy<T>(this T obj) where T : UnityEngine.Object => UnityEngine.Object.Instantiate<T>(obj, Main.prefabParent, false);

        public static SkinnedMeshRenderer ConvertToSkinned(this GameObject gameObject)
        {
            GameObject.Destroy(gameObject.GetComponent<MeshFilter>());
            GameObject.Destroy(gameObject.GetComponent<MeshRenderer>());
            return gameObject.AddComponent<SkinnedMeshRenderer>();
        }
        public static void GenerateBoneData(SlimeAppearanceApplicator slimePrefab, SlimeAppearanceObject bodyApp, float jiggleAmount = 1, float scale = 1, Mesh[] AdditionalMesh = null, params SlimeAppearanceObject[] appearanceObjects)
        {
            if (AdditionalMesh == null)
                AdditionalMesh = new Mesh[0];
            var mesh = bodyApp.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            bodyApp.AttachedBones = new SlimeAppearance.SlimeBone[] { SlimeAppearance.SlimeBone.Slime, SlimeAppearance.SlimeBone.JiggleRight, SlimeAppearance.SlimeBone.JiggleLeft, SlimeAppearance.SlimeBone.JiggleTop, SlimeAppearance.SlimeBone.JiggleBottom, SlimeAppearance.SlimeBone.JiggleFront, SlimeAppearance.SlimeBone.JiggleBack };
            foreach (var a in appearanceObjects)
                a.AttachedBones = bodyApp.AttachedBones;
            var v = mesh.vertices;
            var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var sum = Vector3.zero;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
                if (v[i].x > max.x)
                    max.x = v[i].x;
                if (v[i].x < min.x)
                    min.x = v[i].x;
                if (v[i].y > max.y)
                    max.y = v[i].y;
                if (v[i].y < min.y)
                    min.y = v[i].y;
                if (v[i].z > max.z)
                    max.z = v[i].z;
                if (v[i].z < min.z)
                    min.z = v[i].z;
            }
            var center = sum / v.Length;
            var dis = 0f;
            foreach (var ver in v)
                dis += (ver - center).magnitude;
            dis /= v.Length;

            foreach (var m in new Mesh[] { mesh }.Concat(appearanceObjects.All((x) => true, (x) => x.GetComponent<SkinnedMeshRenderer>().sharedMesh)).Concat(AdditionalMesh))
            {
                Log(m.name);
                var v2 = m.vertices;
                var b = new BoneWeight[v2.Length];
                for (int i = 0; i < v2.Length; i++)
                {
                    var r = v2[i] - center;
                    var o = Mathf.Clamp01((r.magnitude - (dis / 4)) / (dis / 2) * jiggleAmount);
                    b[i] = new BoneWeight();
                    if (o == 0)
                        b[i].weight0 = 1;
                    else
                    {
                        b[i].weight0 = 1 - o;
                        b[i].boneIndex1 = r.x >= 0 ? 1 : 2;
                        b[i].boneIndex2 = r.y >= 0 ? 3 : 4;
                        b[i].boneIndex3 = r.z >= 0 ? 5 : 6;
                        var n = r.Multiply(r).Multiply(r).Abs();
                        var s = n.ToArray().Sum();
                        b[i].weight1 = n.x / s * o;
                        b[i].weight2 = n.y / s * o;
                        b[i].weight3 = n.z / s * o;
                    }
                    b[i].weight0 *= scale;
                    b[i].weight1 *= scale;
                    b[i].weight2 *= scale;
                    b[i].weight3 *= scale;
                }
                m.boneWeights = b;

                var p = new Matrix4x4[bodyApp.AttachedBones.Length];
                for (int i = 0; i < bodyApp.AttachedBones.Length; i++)
                    p[i] = slimePrefab.Bones.First((x) => x.Bone == bodyApp.AttachedBones[i]).BoneObject.transform.worldToLocalMatrix * slimePrefab.Bones.First((x) => x.Bone == SlimeAppearance.SlimeBone.Root).BoneObject.transform.localToWorldMatrix;
                m.bindposes = p;
            }
        }


        public static float[] ToArray(this Vector3 value) => new float[] { value.x, value.y, value.z };
        public static Vector3 Abs(this Vector3 value) => new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
        public static Vector3 Multiply(this Vector3 value, float x, float y, float z) => new Vector3(value.x * x, value.y * y, value.z * z);
        public static Vector3 Multiply(this Vector3 value, Vector3 scale) => value.Multiply(scale.x, scale.y, scale.z);
        public static List<Y> All<X, Y>(this IEnumerable<X> c, Predicate<X> predicate, Func<X, Y> converter, bool enforceUnique = false)
        {
            var l = new List<Y>();
            foreach (var i in c)
                if (predicate(i))
                {
                    if (enforceUnique)
                        l.AddUnique(converter(i));
                    else
                        l.Add(converter(i));
                }
            return l;
        }
        public static bool AddUnique<T>(this List<T> l, T value)
        {
            if (l.Contains(value))
                return false;
            l.Add(value);
            return true;
        }

        public static Mesh CreateMesh(
  IEnumerable<Vector3> vertices,
  IEnumerable<int> triangles,
  IEnumerable<Vector2> uv,
  Predicate<Vector3> removeAt,
  Func<Vector3, Vector3> modify,
  string name = "mesh")
        {
            Mesh mesh = new Mesh();
            mesh.name = name;
            List<Vector3> list1 = vertices.ToList<Vector3>();
            List<int> list2 = triangles.ToList<int>();
            List<Vector2> list3 = uv.ToList<Vector2>();
            for (int index1 = list1.Count - 1; index1 >= 0; --index1)
            {
                if (removeAt(list1[index1]))
                {
                    list1.RemoveAt(index1);
                    list3.RemoveAt(index1);
                    for (int index2 = list2.Count - 3; index2 >= 0; index2 -= 3)
                    {
                        if (list2[index2] == index1 || list2[index2 + 1] == index1 || list2[index2 + 2] == index1)
                        {
                            list2.RemoveRange(index2, 3);
                        }
                        else
                        {
                            if (list2[index2] > index1)
                                list2[index2]--;
                            if (list2[index2 + 1] > index1)
                                list2[index2 + 1]--;
                            if (list2[index2 + 2] > index1)
                                list2[index2 + 2]--;
                        }
                    }
                }
                else
                    list1[index1] = modify(list1[index1]);
            }
            mesh.vertices = list1.ToArray();
            mesh.triangles = list2.ToArray();
            mesh.uv = list3.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }
    }

}
