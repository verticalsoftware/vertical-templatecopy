// Copyright(c) 2018-2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Events;
using Shouldly;
using Vertical.CommandLine;
using Xunit;

namespace Vertical.TemplateCopy.Configuration
{
    public class OptionsConfigurationTest
    {
        [Theory, MemberData(nameof(Theories))]
        public void ParseArguments_Creates_Expected_Options(string args, Options expected)
        {
            var split = args?.Split(' ') ?? Array.Empty<string>();
            var options = CommandLineApplication.ParseArguments<Options>(
                new OptionsConfiguration(_ => { }), split);

            options.OutputPath.ShouldBe(expected.OutputPath);
            options.ArgumentVariableMatchPattern.ShouldBe(expected.ArgumentVariableMatchPattern);
            options.CleanOverwrite.ShouldBe(expected.CleanOverwrite);
            options.EnvironmentVariableMatchPattern.ShouldBe(expected.EnvironmentVariableMatchPattern);
            options.LoggerLevel.ShouldBe(expected.LoggerLevel);
            options.MacroMatchPattern.ShouldBe(expected.MacroMatchPattern);
            options.TemplatePath.ShouldBe(expected.TemplatePath);
            options.Variables.SequenceEqual(expected.Variables).ShouldBeTrue();
        }

        public static IEnumerable<object[]> Theories => new[]
        {
            new object[]{ "-l v" , new Options{ LoggerLevel = LogEventLevel.Verbose }},
            new object[]{ "-l verbose" , new Options{ LoggerLevel = LogEventLevel.Verbose }},
            new object[]{ "-l d" , new Options{ LoggerLevel = LogEventLevel.Debug }},
            new object[]{ "-l debug" , new Options{ LoggerLevel = LogEventLevel.Debug }},
            new object[]{ "-l i" , new Options{ LoggerLevel = LogEventLevel.Information }},
            new object[]{ "-l info" , new Options{ LoggerLevel = LogEventLevel.Information }},
            new object[]{ "-l information" , new Options{ LoggerLevel = LogEventLevel.Information }},
            new object[]{ "-l q" , new Options{ LoggerLevel = LogEventLevel.Error }},
            new object[]{ "-l quiet" , new Options{ LoggerLevel = LogEventLevel.Error }},
            new object[]{ null , new Options{ CleanOverwrite = false}},
            new object[]{ "--overwrite" , new Options{ CleanOverwrite = true}},
            new object[]{ "-o path" , new Options{ OutputPath = Path.GetFullPath("path") }},
            new object[]{ "--output path" , new Options{ OutputPath = Path.GetFullPath("path") }},
            new object[]{ "-t path" , new Options{ TemplatePath = Path.GetFullPath("path") }},
            new object[]{ "--template path" , new Options { TemplatePath = Path.GetFullPath("path") }},
            new object[]{ "-v key=value" , new Options { Variables = { ["key"]="value" }}},
            new object[]{ "--var key=value" , new Options { Variables = { ["key"]="value" }}},
            new object[]{ "-a pattern" , new Options { ArgumentVariableMatchPattern = "pattern" }},
            new object[]{ "--arg-pattern pattern" , new Options { ArgumentVariableMatchPattern = "pattern" }},
            new object[]{ "-e pattern" , new Options { EnvironmentVariableMatchPattern = "pattern" }},
            new object[]{ "--env-pattern pattern" , new Options { EnvironmentVariableMatchPattern = "pattern" }},
            new object[]{ "-m pattern" , new Options { MacroMatchPattern = "pattern" }},
            new object[]{ "--macro-pattern pattern" , new Options { MacroMatchPattern = "pattern" }}
        };

        [Fact]
        public void Parser_Throws_For_Unresolvable_Path()
        {
            Should.Throw<UsageException>(() => CommandLineApplication.ParseArguments<Options>(
                new OptionsConfiguration(_ => { }), new[] {"-o", new string(Path.GetInvalidPathChars()) }));
        }
    }
}
