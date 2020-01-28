// Copyright(c) 2019-2020 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vertical.Tools.TemplateCopy.Core
{
    [ExcludeFromCodeCoverage]
    public static class OptionsExtensions
    {
        /// <summary>
        /// Prints options.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns><see cref="string"/></returns>
        public static IEnumerable<(string key, string value)> Explain(this Options options)
        {
            return new[]
            {
                (key: "source", value: string.Join(";", options.SourcePaths))
                , (key: "target", value: options.TargetPath)
                , (key: "--script", value: string.Join(";", options.ExtensionScriptPaths))
                , (key: "--asm", value: string.Join(";", options.AssemblyReferences))
                , (key: "-v|--verbosity", value: options.Verbosity.ToString())
                , (key: "--plan", value: options.PlanOnly.ToString())
                , (key: "-o|--overwrite", value: options.OverwriteFiles.ToString())
                , (key: "--symbol-pattern", value: options.SymbolPattern)
                , (key: "--tx", value: string.Join(";", options.ContentFileExtensions))
                , (key: "-w|--warn-symbols", value: options.WarnSymbolsMissing.ToString())
            }.Concat(options.Properties.Select(prop => (key: "-p|--prop", value: $"{prop.Key}={prop.Value}")));
        }

        /// <summary>
        /// Gets options as key/value pairs.
        /// </summary>
        /// <param name="options">Options</param>
        /// <returns>Key value pairs</returns>
        public static IDictionary<string, string> AsDictionary(this Options options)
        {
            const string prefix = "option";

            static IEnumerable<(string, string)> GetRange(string key, IEnumerable<string> values)
            {
                var array = values.ToArray();
                return Enumerable.Range(0, array.Length).Select(i => ($"{key}{i}", array[i]));
            }

            var entries = new List<(string,string)>(40);

            entries.AddRange(GetRange("source", options.SourcePaths));
            entries.AddRange(GetRange("--script", options.ExtensionScriptPaths));
            entries.AddRange(GetRange("--asm", options.AssemblyReferences));
            entries.AddRange(GetRange("--tx", options.ContentFileExtensions.SelectMany(str => str.Split(';'))));
            entries.Add(("target", options.TargetPath));
            entries.Add(("-v", options.Verbosity.ToString()));
            entries.Add(("--plan", options.PlanOnly.ToString()));
            entries.Add(("-o", options.PlanOnly.ToString()));
            entries.Add(("--symbol-pattern", options.PlanOnly.ToString()));
            entries.Add(("-w", options.WarnSymbolsMissing.ToString()));

            var keyValuePairs = entries
                .Select(e => new KeyValuePair<string, string>($"{prefix}:{e.Item1}", e.Item2))
                .Concat(options.Properties);
            
            return new Dictionary<string, string>(keyValuePairs);
        }
    }
}