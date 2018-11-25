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

The code is also designed to be incorporated into your project as a single source file (Arguments.cs).

## Quick Start

Create private static properties in the class containing your ```Main()``` and mark them with the ```Argument``` attribute, assigning short and long names.  Invoke
the ```Arguments.Populate()``` method within ```Main()```, then implement the rest of your logic.  

The library will populate your properties with the values
specified in the command line arguments.

```c#
internal class Program
{
    // [Argument(short name (char), long name (string))]
    [Argument('b', "myBool")]
    private static bool Bool { get; set; }

    [Argument('f', "someFloat")]
    private static double Double { get; set; }

    [Argument('i', "anInteger")]
    private static int Int { get; set; }

    [Argument('s', "aString")]
    private static string[] String { get; set; }

    [Operands]
    private static string[] Operands { get; set; }

    private static void Main(string[] args)
    {
        Arguments.Populate();

        Console.WriteLine("Bool: " + Bool);
        Console.WriteLine("Int: " + Int);
        Console.WriteLine("Double: " + Double);

        foreach (string s in String)
        {
            Console.WriteLine("String: " + s);
        }

        foreach (string operand in Operands) 
        {
            Console.WriteLine("\r\n Operand:" + operand);
        }
    }
}
```

## Grammar

The grammar supported by this library is designed to follow the guidelines set forth in the publication 
*The Open Group Base Specifications Issue 7*, specifically the content of Chapter 12, *Utility Conventions*, 
located [here](http://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html).

Each argument is treated as a key-value pair, regardless of whether a value is present.  The general format is as follows:

```<-|--|/>argument-name<=|:| >["|']value['|"] [--] [operand] ... [operand]```

The key-value pair may begin with a single hyphen, a pair of hyphen, or a forward slash.  Single and double dashes indicate the use of short or long names, respectively, which are covered below.  The forward slash may represent either a sort or long name but does not allow for the grouping of parameterless arguments (e.g. /abc is not equivalent to -abc, but rather --abc).

The argument name may be a single character when using short names, or any alphanumeric sequence not including spaces if using long names.

The value delimiter may be an equals sign, a colon, or a space.

Values may be any alphanumeric sequence, however if a value contains a space it must be enclosed in either single or double quotes.

Any word, or phrase enclosed in single or double quotes, will be parsed as an operand.  The official specification requires operands to appear last, however this library will parse them in any position.

A double-hyphen ```--``` not enclosed in single or double quotes and appearing with whitespace on either side designates the end of the argument list and beginning of the explicit operand list.  Anything appearing after this delimiter is treated as an operand, even if it begins with a hyphen, double-hyphen or forward slash.

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

### Multiple Values

Arguments can accept multiple values, and when parsed a ```List<object>``` is returned if more than one value is specified.  When using 
the ```Populate()``` method the underlying property for an argument accepting multiple values must be an array or List, otherwise
an ```InvalidCastException``` is thrown.

#### Example

```--list 1 --list 2 --list 3```

Key | Value
--- | ---
list | 1,2,3

### Operands

Any text in the string that doesn't match the argument-value format is considered an operand.  Any text which appears after a double-hyphen ```--``` not enclosed in single or double quotes and with spaces on either side is treated as an operand regardless of whether it matches the argument-value format.

#### Example

```-a foo bar "hello world" -b -- -explicit operand```

Key | Value
--- | ---
a | foo
b | 

Operands
1. bar
2. "hello world"
3. -explicit
4. operand

## Parsing

Argument key-value pairs can be parsed from any string using the ```Parse(string)``` method.  This method returns a 
```Dictionary<string, object>``` containing all argument-value pairs.

If the string parameter is omitted, the value of ```Environment.CommandLine``` is used.

Note that passing the ```args``` variable, or the result of ```String.Join()``` on ```args```, will prevent the library
from property handling quoted strings.  There are generally very few instance in which ```Environment.CommandLine``` should not be used.

#### Example

```c#
Dictionary<string, object> args = Arguments.Parse("-ab --foo bar");
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

The list of operands is placed into a single property marked with the ```Operands``` attribute.  This property must be
of type ```string[]``` or ```List<string>```.

### Creating Target Properties

Use the ```Argument``` attribute to designate properties to be populated.  The constructor of ```Argument``` accepts a ```char``` and a 
```string```, representing the short and long names of the argument, respectively.

Note that the name of the property doesn't matter; only the attribute values are used to match an argument key to a property.

The Type of the property does matter; the code attempts to convert argument values from string to the specified Type, and if the conversion fails
an ```ArgumentException``` is thrown.  

Properties can accept lists of parameters, as long as they are backed by an array or ```List<>```.  Specifying multiple parameters for an argument backed 
by an atomic Type (e.g. not an array or List) will result in an ```InvalidCastException```.

The ```Operands``` property accepts no parameters.  If the property type is not ```string[]``` or ```List<string>```, an 
```InvalidCastException``` will be thrown.

#### Examples

```c#
[Argument('f', "foo")]
private static string Foo { get; set; }

[Argument('n', "number")]
private static integer MyNumber { get; set; }

[Argument('b', "bool")]
private static bool TrueOrFalse { get; set; }

[Argument('l', "list")]
private static string[] List { get; set; }

[Operands]
private static List<string> Operands { get; set; }
```

Given the argument string ```-bf "bar" --number=5 --list 1 --list 2```, the resulting property values would be as follows:

Property | Value
--- | ---
Foo | bar
MyNumber | 5
TrueOrFalse | true
List | 1,2

 
