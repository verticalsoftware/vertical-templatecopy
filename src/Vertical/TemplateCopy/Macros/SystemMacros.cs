// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.TemplateCopy.Abstractions;

namespace Vertical.TemplateCopy.Macros
{
    /// <summary>
    /// Defines system macros
    /// </summary>
    public static class SystemMacros
    {        
        /// <summary>
        /// Returns the current date.
        /// </summary>
        public static readonly IMacro Now = new DelegateMacro(arg => string.IsNullOrWhiteSpace(arg)
            ? DateTime.Now.ToString()
            : DateTime.Now.ToString(arg));

        /// <summary>
        /// Returns the current date in UTC>
        /// </summary>
        public static readonly IMacro UtcNow = new DelegateMacro(arg => string.IsNullOrWhiteSpace(arg)
            ? DateTime.UtcNow.ToString()
            : DateTime.UtcNow.ToString(arg));

        /// <summary>
        /// Returns the current directory.
        /// </summary>
        public static readonly IMacro CurrentDirectory = new DelegateMacro(_ => Environment.CurrentDirectory);

        /// <summary>
        /// Returns the machine name.
        /// </summary>
        public static readonly IMacro MachineName = new DelegateMacro(_ => Environment.MachineName);

        /// <summary>
        /// Returns the system directory.
        /// </summary>
        public static readonly IMacro SystemDirectory = new DelegateMacro(_ => Environment.SystemDirectory);

        /// <summary>
        /// Returns the domain name.
        /// </summary>
        public static readonly IMacro UserDomainName = new DelegateMacro(_ => Environment.UserDomainName);

        /// <summary>
        /// Returns the user name.
        /// </summary>
        public static readonly IMacro UserName = new DelegateMacro(_ => Environment.UserName);
    }
}
