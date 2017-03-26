# Utility.CommandLine.Arguments

[![Build status](https://ci.appveyor.com/api/projects/status/936bilffko47p63b?svg=true)](https://ci.appveyor.com/project/jpdillingham/utility-commandline-arguments)
[![Build Status](https://travis-ci.org/jpdillingham/Utility.CommandLine.Arguments.svg?branch=master)](https://travis-ci.org/jpdillingham/Utility.CommandLine.Arguments)
[![codecov](https://codecov.io/gh/jpdillingham/Utility.CommandLine.Arguments/branch/master/graph/badge.svg)](https://codecov.io/gh/jpdillingham/Utility.CommandLine.Arguments)
[![NuGet version](https://img.shields.io/nuget/v/Utility.CommandLine.Arguments.svg)](https://www.nuget.org/packages/Utility.CommandLine.Arguments/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/jpdillingham/Utility.CommandLine.Arguments/blob/master/LICENSE)

A C# .NET Class Library containing tools for parsing the command line arguments of console applications.

## Why?

I needed a solution for parsing command line arguments and didn't like the existing options.

## Installation

Install from the NuGet gallery GUI or with the Package Manager Console using the following command:

```Install-Package Utility.CommandLine.Arguments```

## Quick Start

Create private static properties in the class containing your ```Main()``` and mark them with the ```Argument``` attribute, assigning short and long names.  Invoke
the ```Arguments.Populate()``` method within ```Main()```, then implement the rest of your logic.  

The library will populate your properties with the values
specified in the command line arguments.

```c#
internal class Program
{
    [Argument('b', "boolean")]
    private static bool Bool { get; set; }

    [Argument('f', "float")]
    private static double Double { get; set; }

    [Argument('i', "integer")]
    private static int Int { get; set; }

    [Argument('s', "string")]
    private static string String { get; set; }

    private static void Main(string[] args)
    {
        Arguments.Populate();

        Console.WriteLine("String: " + String);
        Console.WriteLine("Bool: " + Bool);
        Console.WriteLine("Int: " + Int);
        Console.WriteLine("Double: " + Double);
    }
}
```

## Grammar

The grammar supported by this library is designed to follow the guidelines set forth in the publication 
*The Open Group Base Specifications Issue 7*, specifically the content of Chapter 12, *Utility Conventions*, 
located [here](http://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html).

Each argument is treated as a key-value pair, regardless of whether a value is present.  The general format is as follows:

```<-|--|/>argument-name<=|:| >["|']value['|"]```

The key-value pair may begin with a single dash, a pair of dashes (double dash), or a forward slash.  Single and double dashes indicate the use of short
or long names, respectively, which are covered below.  The forward slash may represent either, but does not allow for the grouping of parameterless
arguments.

The argument name may be a single character when using short names, or any alphanumeric sequence not including spaces if using long names.

The value delimiter may be an equals sign, a colon, or a space.

Values may be any alphanumeric sequence, however if a value contains a space it must be enclosed in either single or double quotes.

### Short Names

Short names consist of a single character, and arguments without parameters may be grouped.  A grouping may be terminated with a single argument containing
a parameter.  Arguments using short names must be preceded by a single dash.

#### Examples

Single argument with a parameter: ```-a foo```

Key | Value
--- | ---
a | foo

Two parameterless arguments: ```-ab```

Key | Value
--- | ---
a | 
b | 

Three arguments; two parameterless followed by a third argument with a parameter: ```-abc bar```

Key | Value
--- | ---
a | 
b | 
c | bar

### Long Names

Long names can consist of any alphanumeric string not containing a space.  Arguments using long names must be preceded by two dashes.

#### Examples

Single argument with a parameter: ```--foo bar```

Key | Value
--- | ---
foo | bar

Two parameterless arguments: ```--foo --bar```

Key | Value
--- | ---
foo |
bar |

Two arguments with parameters: ```--foo bar --hello world```

Key | Value
--- | ---
foo | bar
hello | world

### Mixed Naming

Any combination of short and long names are supported.

#### Example

```-abc foo --hello world /new="slashes are ok too"```

Key | Value
--- | ---
a |
b | 
c | foo
hello | world
new | slashes are ok too

## Parsing

Argument key-value pairs can be parsed from any string using the ```Parse(string)``` method.  This method returns a 
```Dictionary<string, string>``` containing all argument-value pairs.

If the string parameter is omitted, the value of ```Environment.CommandLine``` is used.

Note that passing the ```args``` variable, or the result of ```String.Join()``` on ```args```, will prevent the library
from property handling quoted strings.  There are generally very few instance in which ```Environment.CommandLine``` should not be used.

#### Example

```c#
Dictionary<string, string> args = Arguments.Parse("-ab --foo bar");
```

The example above would result in a dictionary ```args``` containing:

Key | Value
--- | ---
a | 
b |
foo | bar

Note that boolean values should be checked with ```Dictionary.ContainsKey("name")```; the result will indicate
whether the argument was encountered in the command line arguments.  All other values are retrieved with ```Dictionary["key"]```.

## Populating

The ```Populate()``` method uses reflection to populate private static properties in the target Type with the argument
values matching properties marked with the ```Argument``` attribute.

### Creating Target Properties

Use the ```Argument``` attribute to designate properties to be populated.  The constructor of ```Argument``` accepts a ```char``` and a 
```string```, representing the short and long names of the argument, respectively.

Note that the name of the property doesn't matter; only the attribute values are used to match an argument key to a property.

The Type of the property does matter; the code attempts to convert argument values from string to the specified Type, and if the conversion fails
an ```ArgumentException``` is thrown.

#### Examples

```c#
[Argument('f', "foo")]
private static string Foo { get; set; }

[Argument('n', "number")]
private static integer MyNumber { get; set; }

[Argument('b', "bool")]
private static bool TrueOrFalse { get; set; }
```

Given the argument string ```-bf "bar" --number=5```, the resulting property values would be as follows:

Property | Value
--- | ---
Foo | bar
MyNumber | 5
TrueOrFalse | true

 