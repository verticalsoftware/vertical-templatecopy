// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;

namespace Vertical.Tools.TemplateCopy.Scripting
{
    /// <summary>
    /// Abstracts the invocation of a constructor
    /// </summary>
    public interface IExtensionTypeActivator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of object to create.</param>
        /// <param name="injectables">Injectable parameter values.</param>
        /// <returns>Object instance.</returns>
        object CreateInstance(Type type, IDictionary<Type, object> injectables);
    }
}