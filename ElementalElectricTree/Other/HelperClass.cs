using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using Console = SRML.Console.Console;
using SRML.SR;

using System.Reflection;
using SRML.Console;
using UnityEngine.SceneManagement;

namespace ElementalElectricTree.Other
{
    public static class HelperClass
    {
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
        public static void PrintComponent(this GameObject obj)
        {
            foreach (Component component in obj.GetComponentsInChildren<Component>(true))
            {
                Console.Log("   " + component.ToString());
            }
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

        public static (bool,T) HasComponent<T>(this Component obj) where T : Component
        {
            bool Bool = obj.TryGetComponent<T>(out T returnValue);
            return (Bool,returnValue);
        }

        public static void PrintContent(this Array array)
        {
            //bool Bool = obj.TryGetComponent<T>(out T returnValue);
            //return (Bool, returnValue);
            foreach(var content in array)
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
        public static void PrintContent(this Shader shader)
        {
            Console.Log(shader.name);
            for (int i2 = 0; i2 < shader.GetPropertyCount(); i2++)
            {
                Console.Log(shader.GetPropertyName(i2) + ", " + shader.GetPropertyDescription(i2) + " : " + shader.GetPropertyType(i2));
                foreach (string str in shader.GetPropertyAttributes(i2))
                {
                    Console.Log("   " + str);
                }
            }
        }

    }

}
