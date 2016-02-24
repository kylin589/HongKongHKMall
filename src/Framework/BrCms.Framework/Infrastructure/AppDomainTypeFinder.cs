using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using BrCms.Framework.Logging;

namespace BrCms.Framework.Infrastructure
{
    /// <summary>
    ///     A class that finds types needed by Nop by looping assemblies in the
    ///     currently executing AppDomain. Only assemblies whose names matches
    ///     certain patterns are investigated and an optional list of assemblies
    ///     referenced by <see cref="AssemblyNames" /> are always investigated.
    /// </summary>
    public class AppDomainTypeFinder
    {
        #region Ctor

        /// <summary>Creates a new instance of the AppDomainTypeFinder.</summary>
        public AppDomainTypeFinder()
        {
            this._logger = BrEngineContext.Current.Resolve<ILogger>();
        }

        #endregion

        #region Fields

        private readonly bool ignoreReflectionErrors = true;
        private bool loadAppDomainAssemblies = true;

        private string assemblySkipLoadingPattern =
            "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        private string assemblyRestrictToLoadingPattern = ".*";
        private IList<string> assemblyNames = new List<string>();

        #endregion

        #region Properties

        private readonly ILogger _logger;

        /// <summary>The app domain to look for types in.</summary>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>
        ///     Gets or sets wether Nop should iterate assemblies in the app domain when loading Nop types. Loading patterns
        ///     are applied when loading these assemblies.
        /// </summary>
        public bool LoadAppDomainAssemblies
        {
            get { return this.loadAppDomainAssemblies; }
            set { this.loadAppDomainAssemblies = value; }
        }

        /// <summary>Gets or sets assemblies loaded a startup in addition to those loaded in the AppDomain.</summary>
        public IList<string> AssemblyNames
        {
            get { return this.assemblyNames; }
            set { this.assemblyNames = value; }
        }

        /// <summary>Gets the pattern for dlls that we know don't need to be investigated.</summary>
        public string AssemblySkipLoadingPattern
        {
            get { return this.assemblySkipLoadingPattern; }
            set { this.assemblySkipLoadingPattern = value; }
        }

        /// <summary>
        ///     Gets or sets the pattern for dll that will be investigated. For ease of use this defaults to match all but to
        ///     increase performance you might want to configure a pattern that includes assemblies and your own.
        /// </summary>
        /// <remarks>
        ///     If you change this so that Nop assemblies arn't investigated (e.g. by not including something like "^Nop|..."
        ///     you may break core functionality.
        /// </remarks>
        public string AssemblyRestrictToLoadingPattern
        {
            get { return this.assemblyRestrictToLoadingPattern; }
            set { this.assemblyRestrictToLoadingPattern = value; }
        }

        #endregion

        #region Methods

        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return this.FindClassesOfType(typeof (T), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return this.FindClassesOfType(assignTypeFrom, this.GetAssemblies(), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return this.FindClassesOfType(typeof (T), assemblies, onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies,
            bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                assemblies.ToList().ForEach(a =>
                {
                    try
                    {
                        a.GetTypes().Where(t => (assignTypeFrom.IsAssignableFrom(t)
                                                 || (assignTypeFrom.IsGenericTypeDefinition
                                                     && this.DoesTypeImplementOpenGeneric(t, assignTypeFrom))
                                                 || (t.BaseType != null
                                                     && t.BaseType.IsGenericType
                                                     && t.BaseType.GetGenericTypeDefinition() == assignTypeFrom))
                                                && !t.IsInterface
                                                && (!onlyConcreteClasses || (t.IsClass && !t.IsAbstract)))
                            .ToList()
                            .ForEach(result.Add);
                    }
                    catch
                    {
                        //Entity Framework 6 doesn't allow getting types (throws an exception)
                        if (!this.ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }
                });
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = ex.LoaderExceptions.Aggregate(string.Empty,
                    (current, e) => current + (e.Message + Environment.NewLine));

                var fail = new Exception(msg, ex);

                this._logger.Error(this.GetType(), "AppDomainTypeFinder", ex);

                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }

        /// <summary>Gets the assemblies related to the current implementation.</summary>
        /// <returns>A list of assemblies that should be loaded by the Nop factory.</returns>
        public virtual IList<Assembly> GetAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (this.LoadAppDomainAssemblies)
                this.AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            this.AddConfiguredAssemblies(addedAssemblyNames, assemblies);

            return assemblies;
        }

        #endregion

        #region Utilities

        /// <summary>
        ///     Iterates all assemblies in the AppDomain and if it's name matches the configured patterns add it to our list.
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (this.Matches(assembly.FullName))
                {
                    if (!addedAssemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);
                        addedAssemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        /// <summary>
        ///     Adds specificly configured assemblies.
        /// </summary>
        /// <param name="addedAssemblyNames"></param>
        /// <param name="assemblies"></param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assemblyName in this.AssemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                if (!addedAssemblyNames.Contains(assembly.FullName))
                {
                    assemblies.Add(assembly);
                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        /// <summary>
        ///     Check if a dll is one of the shipped dlls that we know don't need to be investigated.
        /// </summary>
        /// <param name="assemblyFullName">
        ///     The name of the assembly to check.
        /// </param>
        /// <returns>
        ///     True if the assembly should be loaded into Nop.
        /// </returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !this.Matches(assemblyFullName, this.AssemblySkipLoadingPattern)
                   && this.Matches(assemblyFullName, this.AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        ///     Check if a dll is one of the shipped dlls that we know don't need to be investigated.
        /// </summary>
        /// <param name="assemblyFullName">
        ///     The assembly name to match.
        /// </param>
        /// <param name="pattern">
        ///     The regular expression pattern to match against the assembly name.
        /// </param>
        /// <returns>
        ///     True if the pattern matches the assembly name.
        /// </returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        ///     Makes sure matching assemblies in the supplied folder are loaded in the app domain.
        /// </summary>
        /// <param name="directoryPath">
        ///     The physical path to a directory containing dlls to load in the app domain.
        /// </param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();
            foreach (var a in this.GetAssemblies())
            {
                loadedAssemblyNames.Add(a.FullName);
            }

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (var dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (this.Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        this.App.Load(an);
                    }

                    //old loading stuff
                    //Assembly a = Assembly.ReflectionOnlyLoadFrom(dllPath);
                    //if (Matches(a.FullName) && !loadedAssemblyNames.Contains(a.FullName))
                    //{
                    //    App.Load(a.FullName);
                    //}
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        ///     Does type implement generic?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="openGeneric"></param>
        /// <returns></returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}