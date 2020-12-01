using BitsCore.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BitsCore.Utils.Reflection
{
    public static class ReflectionUtils
    {
        //source: https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class
        /// <summary> 
        /// Returns an <see cref="IEnumerable{T}"/> with an instance of all the subclasses of the specified type <typeparamref name="T"/>.
        /// <para> Passes <paramref name="constructorArgs"/> as the connstructor arguments, so they need to match one of the type's constructors. </para>
        /// </summary>
        /// <typeparam name="T"> The type to be checked for subclasses. </typeparam>
        /// <param name="constructorArgs"> The arguments used to construct the instances. </param>
        public static IEnumerable<T> GetInheritedOfTypeInstance<T>(params object[] constructorArgs) where T : class, IComparable<T>
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            objects.Sort();
            return objects;
        }
        /// <summary> Returns an <see cref="IEnumerable{Type}"/> of all the subclasses of the specified type <typeparamref name="T"/>. </summary>
        /// <typeparam name="T"> The type to be checked for subclasses. </typeparam>
        public static List<Type> GetInheritedOfType<T>() where T : class
        {
            List<Type> types = new List<Type>();
            //foreach (Type type in
            //    Assembly.GetAssembly(typeof(T)).GetTypes()
            //    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            //{
            //    types.Add(type);
            //}

            //go through all currently loaded assemblies and get the types, then check if their a subclass of the requested class
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly asm in assemblies)
            {
                types.AddRange( 
                    asm.GetTypes()
                    .Where(myType => myType.IsClass && myType.IsSubclassOf(typeof(T)) && !myType.IsAbstract).ToList() //&& !myType.IsAbstract && myType.IsSubclassOf(typeof(T)) &&&& myType.IsAssignableFrom(typeof(T))
                ); 

            }
            return types;
        }

        /// <summary> 
        /// Returns an <see cref="IEnumerable{T}"/> of type <see cref="object"/> with <paramref name="obj"/>s variables. 
        /// <para> <see cref="BindingFlags.Instance"/> => the variables defined at the start of the class without a public/private/static/const preface. </para>
        /// <para> <see cref="BindingFlags.Public"/> => the variables defined at the start of the class with a public preface. </para>
        /// <para> <see cref="BindingFlags.NonPublic"/> => the variables defined at the start of the class with a private preface. </para>
        /// <para> <see cref="BindingFlags.Static"/> => the variables defined at the start of the class with a static preface. </para>
        /// <para> Reference: https://docs.microsoft.com/de-de/dotnet/api/system.reflection.bindingflags?view=net-5.0 </para>
        /// </summary>
        /// <param name="obj"> The object whoose variables should be extracted. </param>
        /// <param name="bindingFlags"> The flags defining what types of variables should be extracted. </param>
        public static IEnumerable<object> GetVariablesInClass(object obj, BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            IEnumerable<object> result = obj.GetType()
                                            .GetFields(bindingFlags)
                                            .Select(field => field.GetValue(obj))
                                            .ToList();
            return result;
        }

        /// <summary> 
        /// Returns an <see cref="IEnumerable{T}"/> of type <see cref="FieldInfo"/> with <paramref name="obj"/>s variables. 
        /// <para> <see cref="BindingFlags.Instance"/> => the variables defined at the start of the class without a public/private/static/const preface. </para>
        /// <para> <see cref="BindingFlags.Public"/> => the variables defined at the start of the class with a public preface. </para>
        /// <para> <see cref="BindingFlags.NonPublic"/> => the variables defined at the start of the class with a private preface. </para>
        /// <para> <see cref="BindingFlags.Static"/> => the variables defined at the start of the class with a static preface. </para>
        /// </summary>
        /// <param name="obj"> The object whoose variables should be extracted. </param>
        /// <param name="bindingFlags"> The flags defining what types of variables should be extracted. </param>
        public static IEnumerable<FieldInfo> GetVariablesInClassInfo(object obj, BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            IEnumerable<FieldInfo> result = obj.GetType()
                                            .GetFields(bindingFlags);
            return result;
        }
    }
}
