using System;
using System.Collections.Generic;
using System.Reflection;

namespace XFramework.StoryKit
{
    public static class Utility
    {
        public static Type[] GetSonTypes(Type typeBase, string assemblyName = "Assembly-CSharp")
        {
            List<Type> typeNames = new List<Type>();
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch
            {
                return new Type[0];
            }

            if (assembly == null)
            {
                return new Type[0];
            }

            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeBase))
                {
                    typeNames.Add(type);
                }
            }
            //typeNames.Sort();
            return typeNames.ToArray();
        }
    }
}