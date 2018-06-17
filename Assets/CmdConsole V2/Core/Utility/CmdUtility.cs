using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace CmdConsole
{
    public static class CmdUtility
    {
        public static object Parse(this string input, Type toType)
        {
            if (toType == typeof(Vector3))
            {
                string[] strings = Regex.Split(input, @"\s");
                return new Vector3(
                (float)strings[0].Parse(typeof(float)),
                (float)strings[1].Parse(typeof(float)),
                (float)strings[2].Parse(typeof(float)));
            }

            return System.Convert.ChangeType(input, toType);
        }

        public static int GetArgPartCount<T>()
        {
            if (typeof(T).IsValueType)
            {
                return Regex.Split(default(T).ToString(), @"\s").Length;
            }
            else if (typeof(T).IsSubclassOf(typeof(ArgBase)))
            {
                var arg = (T)Activator.CreateInstance(typeof(T)) as ArgBase;
                return arg.PartCount;
            }
            else
            {
                Debug.LogWarning("PartCount defaulting to 1 for unknown type: " + typeof(T).ToString());
                return 1;
            }
        }

        public static bool CanParse(this string input, Type type)
        {
            try
            {
                input.Parse(type);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public static string ColorString(this string text, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        }
    }
}