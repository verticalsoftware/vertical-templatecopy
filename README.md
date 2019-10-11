# vertical-templatecopy

Copy the contents of one directory to the other, while having the ability to make content and filename substitutions along the way using variables.

## Installation

```
dotnet tool install vertical-templatecopy -g
```

## Quick Start

The following example creates a simple directory structure with three files. Each file has variable placeholders which will be transposed with real values.

Create the following folder structure with the three files indicated:

```
    /templates
      /example
        /$(@machineName())
          user.properties
          environment.properties
          system.properties
```

Add the following content to user.properties:

```
    favoriteColor=$(favoriteColor)
    age=$(age)
```

Add the following content to environment.properties:

(For Windows)

```
    appData=$(env:LOCALAPPDATA)
    path=$(env:PATH)
```
(For *nix)

```
    home=$(env:HOME)
    hostName=$(env:HOSTNAME)
```

Add the following content to system.properties:

```
    today=$(@now(MM-dd-yyyy))
    file=$(@templateContext())
```

Invoke the tool using the following:

```
t4copy -t ./templates/example -o ./ -v favoriteColor=greeen -v age=21
```

In the current directory, the utility will make a directory called your machine name (whatever that is), and if you inspect the files you will notice the placeholders in the files have been replaced with real values.

## Placeholder types

### User variables supplied on the command line

The placeholders for these variables take the form of $(&lt;name&gt;), where &lt;name&gt; is the variable key. On the command line, you provide the value using the -v option in the form of -v key=value. You can repeat this option for as many variables that need to be passed. Placeholders that take this form but do not match a supplied value are ignored.

### Environment variables

The tool will replace $(env:&lt;key&gt;) with a matching environment variable value. If the key is not found, the placeholder is ignored.

### Macros

The tool has several built in macros, some of which take arguments. See `t4copy --help` for a list of out-of-box macros.

### Customizing matching patterns

Out-of-box, the tool matches placeholders using the following:

```csharp
// Matches user variables:
Regex.Replace(@"\$\((?<key>[^@)]+)\)", ...)

// Matches environment variables
Regex.Replace(@"\$\(env:(?<key>[^@)]+)\)", ...)

// Matches macros:
Regex.Replace(@"\$\(@(?<fn>[\w]+)\((?<arg>[^\)]+)?\)\)", ...)
```

These patterns can be customized using patterns given on the command line (see `t4copy -help`), but remember to use the same capture group names otherwise the tool will not behave as you expect.

