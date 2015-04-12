﻿using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using FusionMVVM.Common;

namespace FusionMVVM.Service
{
    public class WindowLocator : IWindowLocator
    {
        private readonly Assembly _assembly;
        private readonly IMetric _metric;
        private readonly IFilter<Type> _filter;

        private readonly ConcurrentDictionary<Type, Type> _registeredTypes = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Gets a read-only collection of registered types.
        /// </summary>
        public ReadOnlyDictionary<Type, Type> RegisteredTypes
        {
            get { return new ReadOnlyDictionary<Type, Type>(_registeredTypes); }
        }

        /// <summary>
        /// Initializes a new instance of the WindowLocator class.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="metric"></param>
        /// <param name="filter"></param>
        public WindowLocator(Assembly assembly, IMetric metric, IFilter<Type> filter)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (metric == null) throw new ArgumentNullException("metric");
            if (filter == null) throw new ArgumentNullException("filter");

            _assembly = assembly;
            _metric = metric;
            _filter = filter;
        }

        /// <summary>
        /// Registers a ViewModel and View with the same base name.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        public void Register<TViewModel>()
        {
            Register(typeof(TViewModel));
        }

        /// <summary>
        /// Registers a ViewModel and View as a pair.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        public void Register<TViewModel, TView>()
        {
            Register(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Registers a ViewModel and View as a pair. If a ViewType is not
        /// specified, a View with the same base name will be registered to
        /// the provided ViewModel.
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        public void Register(Type viewModelType, Type viewType = null)
        {
            if (viewModelType == null) throw new ArgumentNullException("viewModelType");

            if (viewType == null)
            {
                var assembly = viewModelType.Assembly;
                var viewName = GetViewName(viewModelType.Name);
                viewType = assembly.GetType(_metric, viewName);
            }

            if (viewType != null)
            {
                _registeredTypes.AddOrUpdate(viewModelType, k => viewType, (k, v) => viewType);
            }
        }

        /// <summary>
        /// Registers all ViewModels and Views with matching names as pairs, in
        /// the provided assembly. If includeReferencedAssemblies is true, all
        /// referenced assemblies are also searched.
        /// </summary>
        /// <param name="includeReferencedAssemblies"></param>
        public void RegisterAll(bool includeReferencedAssemblies = false)
        {
            var assemblies = _assembly.GetAssemblyCluster(includeReferencedAssemblies);

            foreach (var assembly in assemblies)
            {
                foreach (var viewModelType in _filter.ApplyFilter("ViewModel", assembly.GetTypes(), StringComparison.OrdinalIgnoreCase))
                {
                    Register(viewModelType);
                }
            }
        }

        public void ShowWindow(ViewModelBase viewModel, ViewModelBase owner = null)
        {
            throw new NotImplementedException();
        }

        public void ShowDialogWindow(ViewModelBase viewModel, ViewModelBase owner = null)
        {
            throw new NotImplementedException();
        }

        public void CloseWindow(ViewModelBase viewModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes 'Model' from the ViewModel name and returns the expected View name.
        /// </summary>
        /// <param name="viewModelName"></param>
        /// <returns></returns>
        public string GetViewName(string viewModelName)
        {
            if (viewModelName == null) throw new ArgumentNullException("viewModelName");

            if (viewModelName == string.Empty) return string.Empty;

            return Regex.Replace(viewModelName, "Model", string.Empty, RegexOptions.IgnoreCase);
        }
    }
}
