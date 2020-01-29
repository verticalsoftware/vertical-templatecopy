# vertical-templatecopy

[![Build Status](https://dev.azure.com/vertical-software/vertical-templatecopy/_apis/build/status/verticalsoftware.vertical-templatecopy?branchName=master)](https://dev.azure.com/vertical-software/vertical-templatecopy/_build/latest?definitionId=4&branchName=master)
![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/vertical-software/vertical-templatecopy/2)
![Nuget](https://img.shields.io/nuget/v/vertical-templatecopy)

Copy the contents of one directory to the other, while having the ability to make content and filename substitutions along the way using `${handlebar}` templates.

**Note: Version 2.0 is breaking (see bottom)**

## Installation

```
dotnet tool install vertical-templatecopy -g
```

## Quick Start

The following command copies all files and subfolders from ~/templates/classlibrary to ~/src/libraries/newlibrary, and replaces the symbol `${solution}` in any of the files (or file or directory names) in the source directory with the value "Vertical.TemplateCopy".

```
t4copy ~/templates/classlibrary ~/src/libraries/newlibrary -p solution=Vertical.TemplateCopy
```

## Overview

A very simple file copy utility with a twist: it replaces handlebar templates with values you provide (see quick start above). When the utility encounters a handlebar, it looks in the symbol table for the handlebar property name. If it finds a matching key, it replaces the handlebar with the value of the property.

### Command line usage

```
t4copy <source-path[,source-path]> <target-path> [OPTIONS]
```

### Arguments

|Argument&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|---|---|
|&lt;source-path&gt;|One or more source paths that contains the directory objects to copy.
|&lt;target-path&gt;|The directory where the source assets are copied to (will be created if it does not exist).

### Common options

|Option&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|---|---|
|-h,--help|Display the help manual.|
|-p, --prop &lt;key=value&gt;|A repeatable option that associates a key with a value.
|--plan| Switch that causes t4copy to display what the utility would do, without making any modifications to the file system.
|-o, --overwrite| Switch that permits the utility to overwrite files that already exist. If not specified and an overwrite condition would occur, an error will be displayed and the operation will be stopped.
|--tx &lt;extensions&gt;| A semi-colon delimited list of file extensions that identify files that are inspected for template placeholders. Files that have other extensions not in this list are copied and not inspected for content. *Not specifying* this option means *all* files will be transformed. Be aware of this if you have big files in your template as the content will be loaded to memory.

## Advanced Usage

### Environment Variables

t4copy also builds the symbol table with environment variables. Therefore, you can place the name of an environment variable (case-sensitive) in a handlebar and the value will be resolved. For example ${USERNAME} on Windows or ${USER} on Linux will be replaced with the current user name.

### Extension Scripts

#### Basic Usage

Extension scripts are the most powerful way to provide dynamic property values, because they are backed using the .Net CLR and C# code. An extension script is a C# code file with an public class that has one or more string properties defined. The names of the properties are added to the symbol table, and when referenced by a template handlebar, are dynamically invoked to get the value. Since this is a proper C# class, and the code is compiled using Roslyn, you have the power of .Net available to your template.

Simple example:

```csharp
using System;

// The namespace and class names are unimportant
namespace T4Copy
{
    public class Props
    {
        public string Color => "blue";
        public string Material => "steel";
    }    
}
```

When this script is loaded, two additional properties will be available in the symbol table, and they can be included in template files using the handlebars `${Color}` and `${Material}`. The namespace name and the class name do not matter as long as the class is public and the properties are public (you can have private properties, methods, etc., but they won't be discovered by the symbol table). You instruct t4copy to load this script by using the repeatable `--script <path>` option. For instance, if your .cs file is located in /usr/src/templates/props.cs, you would use:

```
$ t4copy <source> <target> --script /usr/src/templates/props.cs
```

#### Constructor parameters

The class you define in a script file lives as a singleton instance during the utility's lifetime. This is by design so your class can maintain state, and therefore you can define a constructor. Your properties class can request objects from the utility runtime by introducing parameters in its public constructor. 

You can access the properties specified on the command line by introducing a parameter of type `IDictionary<string, string>` into the class constructor. In turn, when t4copy creates an instance of your class, it will supply a case-sensitive dictionary that contains the key/value pairs of properties given by the -p or --prop option. Other command line option values that aren't properties defined by the -p or --prop option are also populated in the dictionary, and are prepended with t4copy_option:&lt;name&gt;, where &lt;name&gt; is the option identifier. The values provided are closely named with their command line option counterpart.

Additionally, you can ask for a Serilog [ILogger](https://github.com/serilog/serilog/blob/dev/src/Serilog/ILogger.cs) in the event you wish to write trace/debug information to the console.

For example, to have access to the properties dictionary and the logging delegate, your class may look like this:

```csharp
using System;
using System.Collections.Generic;
using Serilog;

namespace T4Copy 
{
    public class Props 
    {
        public Props(IDictionary<string, string> properties, ILogger logger)
        {
            var myValue = properties["value"];
            logger.Information("Received {count} properties from t4copy!", properties.Count);
        }
    }
}
```

#### Additional assembly references

The CSharp compilation code adds the following assemblies as references to the script loader: mscorlib, netstandard, System, System.Core, System.Runtime, System.Collections, System.Data, System.Data.Common, System.IO, System.Linq, System.Net, System.Net.Http, System.Threading, and System.Xml. You can specify additional assembly names (for standard .Net assemblies), or you can specify the path to your own assemblies using the repeatable --asm option. Note though, that if you specify your own assemblies, you must also specify assemblies that are referenced anywhere in the dependency graph.

### Advanced Options

The following are advanced command line options:

|Option&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|Description|
|---|---|
|--asm|Name of additional .Net core assembly to add as a reference to the compiler for extension scripts, or, a fully qualified path to an assembly.|
|--symbol-pattern|A regular expression pattern that the utility will use to identify and replace template placeholders. If you define a custom pattern, you are required to have a capture group named 'symbol' in your pattern so the utility knows how to extract the property name from the placeholder.|
|-v,--verbosity|The console logging verbosity. May be (v)erbose, (d)ebug, ((i)nfo)rmation, ((w)arn)ing, (e)rror, or (f)atal.|
|-w,--warn-symbols|Displays a warning in the console log for any placeholders that do not map to a property in the symbol table.|

## What broke from 1.x

This was a *title update*, so although the intent of the utility is exactly the same, the API looks a lot different. The main driver was removing the out-of-box macros. Originally, I was solving my own specific use cases while not giving you the ability to solve yours.

- Removed all "out-of-box" macros
- Introduced CSharp extensible script support
- Introduced multi-valued source paths
- Introduced content file extension matching (--tx option)
- Merge command line and environment variables nomenclature to symbol table
- Renamed -a, --arg-pattern with --symbol-pattern
- Renamed -v, --var to -p, --prop
- Removed -e, --env-pattern as all templates are consistent
- Removed -m, --macro-pattern option (replaced with scripting)
- Renamed -l, --logger to -v, --verbosity
- Added -o as overwrite option synonym
- Added --script option
- Changed -o, --output to unnamed argument
- Changed -t, --template to unnamed argument

## Finally

### Sample

Look for the sample folder in the root directory that contains the template of a class library. You can find a powershell script that demonstrates command line usage, a properties class file, and a full template that illustrates template placeholders on files and file names.

### Contributing

Yes - jump on in! Please create an [issue](https://github.com/verticalsoftware/vertical-templatecopy/issues/new) for enhancements or bugs so we can discuss.