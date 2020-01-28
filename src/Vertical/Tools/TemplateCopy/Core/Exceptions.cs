// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Vertical.CommandLine;

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Defines the application exceptions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Exceptions
    {
        public static Exception FileExists(string path) => new NotSupportedException(
            $"File {path} already exists and will not be overwritten (configured in options).");

        public static Exception NoSingleExportedClass(string path) => new ApplicationException(
            $"Extensible source file {path} does not contain a type definition. Source files must contain a single, exported type.");

        public static Exception ConstructorInvocationFailed(Type exportedType, string path, Exception exception) => new ApplicationException(
            $"Could not instantiate type {exportedType} defined in '{path}': {exception.Message}");
        
        public static Exception IncompatibleConstructor() => new NotSupportedException("No compatible constructor found.");

        public static Exception InvalidPropertyArgument(string s) => new UsageException(
            $"Property option (-p, --prop) requires format <key>=<value>, e.g.: -p color=blue");

        public static Exception NoSourcePaths() => new ApplicationException("One source path is required");
        
        public static Exception InvalidFileSystemObject(string path) => new ApplicationException(
            $"Failed to validate path {path} - object does not exist or current user does not have permission.");

        public static Exception InvalidSymbolPattern(string pattern) => new ApplicationException(
            $"Symbol matching pattern '{pattern}' (specified by --symbol) is not a valid regular expression.");

        public static Exception InvalidAssemblyReference(string assembly) => new FileNotFoundException(
            $"Could not find assembly reference '{assembly}'");
    }
}