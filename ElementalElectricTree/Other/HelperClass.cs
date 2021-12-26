using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Console = SRML.Console.Console;

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

    }

}
