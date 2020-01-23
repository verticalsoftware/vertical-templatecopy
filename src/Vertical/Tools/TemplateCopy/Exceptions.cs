// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using Vertical.CommandLine;

namespace Vertical.Tools.TemplateCopy
{
    /// <summary>
    /// Defines the application exceptions.
    /// </summary>
    public static class Exceptions
    {
        public static Exception FileExists(string path) => new NotSupportedException(
            $"File {path} already exists and will not be overwritten (configured in options).");

        public static Exception NoSingleExportedClass(string path) => new ApplicationException(
            $"Extensible source file {path} does not contain a type definition. Source files must contain a single, exported type.");

        public static Exception IncompatibleTypeConstructor(Type exportedType, string path) => new ApplicationException(
            $"Extensible source file {path} defines class {exportedType}, but the type does not have a compatible constructor.");

        public static Exception InvalidPropertyArgument(string s) => new UsageException(
            $"Property option (-p, --prop) requires format <key>=<value>, e.g.: -p color=blue");

        public static Exception NoSourcePaths() => new ApplicationException("One source path is required");
        
        public static Exception InvalidFileSystemObject(string path) => new ApplicationException(
            $"Failed to validate path {path} - object does not exist or current user does not have permission.");

        public static Exception InvalidSymbolPattern(string pattern) => new ApplicationException(
            $"Symbol matching pattern '{pattern}' (specified by --symbol) is not a valid regular expression.");
    }
}