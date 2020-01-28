// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.IO;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Help;

namespace Vertical.Tools.TemplateCopy.Core
{
    /// <summary>
    /// Command line parameters configuration.
    /// </summary>
    public class OptionsConfiguration : ApplicationConfiguration<Options>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public OptionsConfiguration(Action<Options> onExecute)
        {
            OnExecute(onExecute);
                    
            PositionArgument(arg => arg.MapMany.Using(MapPath));
            Option("--tx", arg => arg.Map.Using(MapContentExtensions));
            Option("-v|--verbosity", arg => arg.Map.Using(MapLoggingLevel));
            Option("--script", arg => arg.MapMany.ToCollection(opt => opt.ExtensionScriptPaths));
            Option("-p|--prop", arg => arg.MapMany.Using(MapProperty));
            Option("--asm", arg => arg.MapMany.ToCollection(opt => opt.AssemblyReferences));
            Option("--symbol-pattern", arg => arg.Map.ToProperty(opt => opt.SymbolPattern));
            Switch("--plan", arg => arg.Map.ToProperty(opt => opt.PlanOnly)); 
            Switch("-o|--overwrite", arg => arg.Map.ToProperty(opt => opt.OverwriteFiles));
            Switch("-w|--warn-symbols", arg => arg.Map.ToProperty(opt => opt.WarnSymbolsMissing));

            HelpOption("-h|--help", InteractiveConsoleHelpWriter.Default);
            Help.UseFile(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources", "help.txt"));
        }

        private static void MapLoggingLevel(Options options, string value)
        {
            options.Verbosity = LogEventLevelConverter.Default.GetLevel(value);
        }

        private static void MapPath(Options option, string value)
        {
            if (!string.IsNullOrWhiteSpace(option.TargetPath))
            {
                option.SourcePaths.Add(option.TargetPath);
            }

            option.TargetPath = value;
        }

        private static void MapProperty(Options options, string arg)
        {
            var kvp = arg.Split('=');

            if (kvp.Length != 2) { throw Exceptions.InvalidPropertyArgument(arg); }

            options.Properties[kvp[0]] = kvp[1];
        }

        private static void MapContentExtensions(Options options, string arg)
        {
            foreach (var extension in arg.Split(';'))
            {
                options.ContentFileExtensions.Add(extension);
            }
        }
    }
}