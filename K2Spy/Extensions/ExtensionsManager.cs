using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.Extensions
{
    internal class ExtensionsManager
    {
        #region Private Fields

        private static readonly object m_LockObject = new object();
        private static List<Model.IExtension> m_Extensions;

        #endregion

        #region Public Properties

        public static bool Initialized { get; private set; }

        public static Model.IExtension[] Extensions
        {
            get
            {
                if (!ExtensionsManager.Initialized)
                    throw new Exception("The PluginManager has not yet been initialized");
                return ExtensionsManager.m_Extensions.ToArray();
            }
        }

        #endregion

        #region Public Methods

        public static TExtension GetExtension<TExtension>(bool requireExactlyOne = true, bool throwOnNotFound = true)
            where TExtension : Model.IExtension
        {
            IEnumerable<TExtension> extensions = ExtensionsManager.GetExtensions<TExtension>(true);
            if (requireExactlyOne && extensions.Count() > 1)
                throw new Exception($"Expected to find a single extension of the type {typeof(TExtension)}, found {extensions.Count()}");

            return extensions.FirstOrDefault();
        }

        public static IEnumerable<TExtension> GetExtensions<TExtension>(bool throwOnNotFound = true)
            where TExtension : Model.IExtension
        {
            IEnumerable<TExtension> result = ExtensionsManager.Extensions.OfType<TExtension>();
            if (throwOnNotFound && result.FirstOrDefault() == null)
                throw new Exception($"No extensions of the type {typeof(TExtension)} were found");
            return result;
        }

        public static async Task InitializeAsync()
        {
            await Task.Run((Action)ExtensionsManager.Initialize);
        }

        public static void Initialize()
        {
            lock (ExtensionsManager.m_LockObject)
            {
                if (!ExtensionsManager.Initialized)
                {
                    List<Type> allExtensionTypes = new List<Type>();

                    // load embedded plugins
                    Type[] types = ExtensionsManager.GetExtensionsFromAssembly(typeof(ExtensionsManager).Assembly);// typeof typeof(PluginManager).Assembly.GetExportedTypes().Where(key => key.IsClass && !key.IsAbstract).Where(key => typeof(Model.IExtension).IsAssignableFrom(key)).Where(key => key.GetCustomAttribute<ObsoleteAttribute>()?.IsError != true).ToArray();
                    allExtensionTypes.AddRange(types);

                    // load external plugins
                    string pluginsDirectory = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "plugins");
                    if (System.IO.Directory.Exists(pluginsDirectory))
                    {
                        List<System.Reflection.Assembly> assemblies = new List<Assembly>();
                        foreach (string file in System.IO.Directory.GetFiles(pluginsDirectory, "*.exe"))
                            assemblies.Add(System.Reflection.Assembly.LoadFrom(file));
                        foreach (System.Reflection.Assembly assembly in assemblies)
                        {
                            types = ExtensionsManager.GetExtensionsFromAssembly(assembly);
                            allExtensionTypes.AddRange(types);
                        }
                    }


                    // initialize plugins
                    ExtensionsManager.m_Extensions = allExtensionTypes.Select(key => (Model.IExtension)Activator.CreateInstance(key)).Cast<Model.IExtension>().OrderByDescending(key => (key as Model.IExtensionPriority)?.Priority ?? 0).ToList();
                    ExtensionsManager.Initialized = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private static Type[] GetExtensionsFromAssembly(System.Reflection.Assembly assembly)
        {
            Type[] types = null;
            if (assembly == typeof(ExtensionsManager).Assembly)
                types = assembly.GetTypes();
            else
                types = assembly.GetExportedTypes();
            return types.Where(key => key.IsClass && !key.IsAbstract)
                .Where(key => typeof(Model.IExtension).IsAssignableFrom(key))
                .Where(key => key.GetCustomAttribute<ObsoleteAttribute>()?.IsError != true)
                .Where(key => key.GetCustomAttribute<Model.IgnoreExtensionAttribute>() == null)
                .ToArray();
        }

        #endregion
    }
}