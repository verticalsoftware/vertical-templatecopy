// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Vertical.CommandLine.Configuration;
using Vertical.CommandLine.Help;

namespace Vertical.Tools.TemplateCopy
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
                    
            PositionArgument(arg => arg.MapMany.ToSet(opt => opt.SourcePaths));
            Option("-t|--target", arg => arg.Map.ToProperty(opt => opt.TargetPath));
            Option("--tx", arg => arg.MapMany.Using(MapContentExtensions));
            Option<LogEventLevel>("-v|--verbosity", arg => arg.Map.ToProperty(opt => opt.Verbosity));
            Option("--script", arg => arg.MapMany.ToCollection(opt => opt.ExtensionScriptPaths));
            Option("-p|--prop", arg => arg.MapMany.Using(MapProperty));
            Option("--asm", arg => arg.MapMany.ToCollection(opt => opt.AssemblyReferences));
            Option("--symbol", arg => arg.Map.ToProperty(opt => opt.SymbolPattern));
            Switch("--plan", arg => arg.Map.ToProperty(opt => opt.PlanOnly)); 
            Switch("-o|--overwrite", arg => arg.Map.ToProperty(opt => opt.OverwriteFiles));
            Switch("-w|--warn-symbols", arg => arg.Map.ToProperty(opt => opt.WarnSymbolsMissing));

            HelpOption("-h|--help", InteractiveConsoleHelpWriter.Default);
            Help.UseFile(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources", "help.txt"));
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
                options.ContentFileExtensions.Add(extension);
        }

        /// <summary>
        /// Prints options.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="string"/></returns>
        public static IEnumerable<(string key, string value)> ExplainOptions(Options options)
        {
            return new[]
            {
                (key: "-s|--src|--source", value: string.Join(";", options.SourcePaths))
                , (key: "-t|--target", value: options.TargetPath)
                , (key: "--script", value: string.Join(";", options.ExtensionScriptPaths))
                , (key: "--asm", value: string.Join(";", options.AssemblyReferences))
                , (key: "-v|--verbosity", value: options.Verbosity.ToString())
                , (key: "--plan", value: options.PlanOnly.ToString())
                , (key: "-o|--overwrite", value: options.OverwriteFiles.ToString())
                , (key: "--symbol", value: options.SymbolPattern)
                , (key: "--tx", value: string.Join(";", options.ContentFileExtensions))
                , (key: "-w|--warn-symbols", value: options.WarnSymbolsMissing.ToString())
            }.Concat(options.Properties.Select(prop => (key: "-p|--prop", value: $"{prop.Key}={prop.Value}")));
        }
    }
}