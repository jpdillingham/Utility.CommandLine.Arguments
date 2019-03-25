/*
  █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
  █  The MIT License (MIT)
  █
  █  Copyright (c) 2017 JP Dillingham (jp@dillingham.ws)
  █
  █  Permission is hereby granted, free of charge, to any person obtaining a copy
  █  of this software and associated documentation files (the "Software"), to deal
  █  in the Software without restriction, including without limitation the rights
  █  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  █  copies of the Software, and to permit persons to whom the Software is
  █  furnished to do so, subject to the following conditions:
  █
  █  The above copyright notice and this permission notice shall be included in all
  █  copies or substantial portions of the Software.
  █
  █  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  █  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  █  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  █  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  █  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  █  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  █  SOFTWARE.
  █
  ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀  ▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀██
                                                                                               ██
                                                                                           ▀█▄ ██ ▄█▀
                                                                                             ▀████▀
                                                                                               ▀▀                            */

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Utility.CommandLine.Tests
{
    [Collection("ArgumentAttribute")]
    public class ArgumentAttribute
    {
        [Fact]
        public void Constructor()
        {
            CommandLine.ArgumentAttribute test = new CommandLine.ArgumentAttribute('n', "name", "help");

            Assert.Equal('n', test.ShortName);
            Assert.Equal("name", test.LongName);
            Assert.Equal("help", test.HelpText);
        }

    }

    [Collection("ArgumentInfo")]
    public class ArgumentInfo
    {
        [Fact]
        public void Constructor()
        {
            CommandLine.ArgumentInfo test = new CommandLine.ArgumentInfo()
            {
                ShortName = 'a',
                LongName = "aa",
                HelpText = "help",
            };

            Assert.Equal('a', test.ShortName);
            Assert.Equal("aa", test.LongName);
            Assert.Equal("help", test.HelpText);
        }

    }

    [Collection("Arguments")]
    public class Arguments
    {
        public enum Enums
        {
            Foo = 1,
            Bar = 2,
        }

        [CommandLine.Argument('b', "bool", "help")]
        private static bool Bool { get; set; }

        [CommandLine.Argument('d', "decimal")]
        private static decimal Decimal { get; set; }

        [CommandLine.Argument('i', "integer")]
        private static int Integer { get; set; }

        [CommandLine.Argument('c', "case-sensitive")]
        private static string LowerCase { get; set; }

        private static string NonArgumentProperty { get; set; }

        [CommandLine.Operands]
        private static List<string> Operands { get; set; }

        [Obsolete]
        private static string PlainProperty { get; set; }

        [CommandLine.Argument('t', "test-prop")]
        private static string TestProp { get; set; }

        [CommandLine.Argument('C', "CASE-SENSITIVE")]
        private static string UpperCase { get; set; }

        [CommandLine.Argument('e', "enum")]
        private static Enums Enum { get; set; }

        [Fact]
        public void GetArgumentInfo()
        {
            var help = CommandLine.Arguments.GetArgumentInfo(typeof(Arguments)).ToList();

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Single(help.Where(h => h.LongName == "test-prop"));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        /// <summary>
        ///     Tests <see cref="CommandLine.Arguments.GetArgumentHelp"/>.
        /// </summary>
        [Fact]
        public void GetArgumentHelp()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var help = CommandLine.Arguments.GetArgumentHelp(typeof(Arguments)).ToList();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Single(help.Where(h => h.LongName == "test-prop"));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        [Fact]
        public void GetArgumentInfoNull()
        {
            var help = CommandLine.Arguments.GetArgumentInfo().ToList();

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        [Fact]
        public void GetArgumentHelpNull()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var help = CommandLine.Arguments.GetArgumentHelp().ToList();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        [Fact]
        public void Indexer()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one --two three");

            Assert.Equal("one", test["test"]);
            Assert.Equal("three", test["two"]);
        }

        [Fact]
        public void Parse()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse().ArgumentDictionary;

            Assert.NotEmpty(test);
        }

        [Fact]
        public void ParseCaseSensitive()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--TEST -aBc").ArgumentDictionary;

            Assert.True(test.ContainsKey("TEST"));
            Assert.False(test.ContainsKey("test"));

            Assert.True(test.ContainsKey("a"));
            Assert.False(test.ContainsKey("A"));

            Assert.True(test.ContainsKey("B"));
            Assert.False(test.ContainsKey("b"));

            Assert.True(test.ContainsKey("c"));
            Assert.False(test.ContainsKey("C"));
        }

        [Fact]
        public void ParseDashedOperand()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("hello-world");

            Assert.Equal("hello-world", test.OperandList[0]);
        }

        [Fact]
        public void ParseDecimal()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--decimal 1.1");

            Assert.Equal("1.1", test["decimal"]);
        }

        [Fact]
        public void ParseEmpty()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse(string.Empty));

            Assert.Null(ex);
        }

        [Fact]
        public void ParseInnerQuotedStrings()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test1 \"test \'1\'\" --test2 \'test \"2\"\'").ArgumentDictionary;

            Assert.Equal("test \'1\'", test["test1"]);
            Assert.Equal("test \"2\"", test["test2"]);
        }

        [Fact]
        public void ParseLongAndShortMix()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--one=1 -ab 2 /three:3 -4 4").ArgumentDictionary;

            Assert.Equal("1", test["one"]);
            Assert.True(test.ContainsKey("a"));
            Assert.True(test.ContainsKey("b"));
            Assert.Equal("2", test["b"]);
            Assert.Equal("3", test["three"]);
            Assert.Equal("4", test["4"]);
        }

        [Fact]
        public void ParseMixedArgumentsAndOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two --three four");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("four", test.ArgumentDictionary["three"]);
        }

        [Fact]
        public void ParseMultipleQuotes()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test1 \"1\" --test2 \"2\" --test3 \'3\' --test4 \'4\'").ArgumentDictionary;

            Assert.Equal("1", test["test1"]);
            Assert.Equal("2", test["test2"]);
            Assert.Equal("3", test["test3"]);
            Assert.Equal("4", test["test4"]);
        }

        [Fact]
        public void ParseNoArgument()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse());

            Assert.Null(ex);
        }

        [Fact]
        public void ParseNull()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse(null));

            Assert.Null(ex);
        }

        [Fact]
        public void ParseOnlyOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("hello world!");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("hello", test.OperandList[0]);
            Assert.Equal("world!", test.OperandList[1]);
        }

        [Fact]
        public void ParseOperand()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
        }

        [Fact]
        public void ParseOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two three four");

            Assert.Equal(3, test.OperandList.Count);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("three", test.OperandList[1]);
            Assert.Equal("four", test.OperandList[2]);
        }

        [Fact]
        public void ParseShorts()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("-abc 'hello world'").ArgumentDictionary;

            Assert.True(test.ContainsKey("a"));
            Assert.Equal(string.Empty, test["a"]);

            Assert.True(test.ContainsKey("b"));
            Assert.Equal(string.Empty, test["b"]);

            Assert.True(test.ContainsKey("c"));
            Assert.Equal("hello world", test["c"]);
        }

        [Fact]
        public void ParseStrictOperandDelimiterOnly()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--");

            Assert.Empty(test.OperandList);
        }

        [Fact]
        public void ParseStrictOperandMultipleDelimiter()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("one -- two -- three");

            Assert.Equal(4, test.OperandList.Count);
            Assert.Equal("one", test.OperandList[0]);
            Assert.Equal("two", test.OperandList[1]);
            Assert.Equal("--", test.OperandList[2]);
            Assert.Equal("three", test.OperandList[3]);
        }

        [Fact]
        public void ParseStrictOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two -- three -four --five /six \"seven eight\" 'nine ten'");

            Assert.Equal(7, test.OperandList.Count);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("three", test.OperandList[1]);
            Assert.Equal("-four", test.OperandList[2]);
            Assert.Equal("--five", test.OperandList[3]);
            Assert.Equal("/six", test.OperandList[4]);
            Assert.Equal("\"seven eight\"", test.OperandList[5]);
            Assert.Equal("'nine ten'", test.OperandList[6]);
        }

        [Fact]
        public void ParseStrictOperandsEmpty()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two --");

            Assert.Single(test.OperandList);
            Assert.Equal("two", test.OperandList[0]);
        }

        [Fact]
        public void ParseStrictOperandsStart()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("-- one two");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("one", test.OperandList[0]);
            Assert.Equal("two", test.OperandList[1]);
        }

        [Fact]
        public void ParseStringOfLongs()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--one 1 --two=2 /three:3 --four \"4 4\" --five='5 5'").ArgumentDictionary;

            Assert.NotEmpty(test);
            Assert.Equal(5, test.Count);
            Assert.Equal("1", test["one"]);
            Assert.Equal("2", test["two"]);
            Assert.Equal("3", test["three"]);
            Assert.Equal("4 4", test["four"]);
            Assert.Equal("5 5", test["five"]);
        }

        [Fact]
        public void ParseValueBeginningWithSlash()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--file=/mnt/data/test.xml").ArgumentDictionary;

            Assert.Equal("/mnt/data/test.xml", test["file"]);
        }

        [Fact]
        public void ParseValueWithQuotedPeriod()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test \"test.test\" --test2 'test2.test2'").ArgumentDictionary;

            Assert.Equal("test.test", test["test"]);
            Assert.Equal("test2.test2", test["test2"]);
        }

        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate("-b"));

            Assert.Null(ex);
        }

        [Fact]
        public void PopulateBogusCaller()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate("-b", true, Guid.NewGuid().ToString()));

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void PopulateCaseSensitive()
        {
            CommandLine.Arguments.Populate("-c lower -C upper");

            Assert.Equal("lower", LowerCase);
            Assert.Equal("upper", UpperCase);

            CommandLine.Arguments.Populate("--case-sensitive lower --CASE-SENSITIVE upper");

            Assert.Equal("lower", LowerCase);
            Assert.Equal("upper", UpperCase);
        }

        [Fact]
        public void PopulateDecimal()
        {
            CommandLine.Arguments.Populate("--decimal 1.1");

            Assert.Equal(1.1M, Decimal);
        }

        [Fact]
        public void PopulateEnum()
        {
            CommandLine.Arguments.Populate("--enum bar");

            Assert.Equal(Enums.Bar, Enum);
        }

        [Fact]
        public void PopulateDisableClearing()
        {
            Bool = true;
            Decimal = 3.5m;
            Integer = 42;

            CommandLine.Arguments.Populate(string.Empty, false);

            Assert.True(Bool);
            Assert.Equal(3.5m, Decimal);
            Assert.Equal(42, Integer);
        }

        [Fact]
        public void PopulateDuplicateProperties()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(typeof(TestClassDuplicateProperties), "--hello world"));

            Assert.Null(ex);

            Assert.Equal("world", TestClassDuplicateProperties.Test1);
            Assert.Equal(default(string), TestClassDuplicateProperties.Test2);
        }

        [Fact]
        public void PopulateExternalClass()
        {
            CommandLine.Arguments.Populate(typeof(TestClassPublicProperties), "--test test! operand1 operand2");

            Assert.Equal("test!", TestClassPublicProperties.Test);
            Assert.Equal("operand1", TestClassPublicProperties.Operands[0]);
            Assert.Equal("operand2", TestClassPublicProperties.Operands[1]);
        }

        [Fact]
        public void PopulateMultipleValuesNotCollectionBacked()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--integer 1 --integer 2"));

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        [Fact]
        public void PopulateOperands()
        {
            CommandLine.Arguments.Populate(GetType(), "--test one two three");

            Assert.Equal("two", Operands[0]);
            Assert.Equal("three", Operands[1]);
        }

        [Fact]
        public void PopulateShortNames()
        {
            CommandLine.Arguments.Populate("-bi 3");

            Assert.True(Bool);
            Assert.Equal(3, Integer);
        }

        [Fact]
        public void PopulateString()
        {
            CommandLine.Arguments.Populate("--test-prop 'hello world!' --bool --integer 5");

            Assert.Equal("hello world!", TestProp);
            Assert.True(Bool);
            Assert.Equal(5, Integer);
        }

        [Fact]
        public void PopulateType()
        {
            CommandLine.Arguments.Populate(GetType(), "--integer 5");

            Assert.Equal(5, Integer);
        }

        [Fact]
        public void PopulateTypeMismatch()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--integer five"));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void SetsDefaultValuesOnPopulate()
        {
            CommandLine.Arguments.Populate(typeof(TestClassWithDefaultValues));

            Assert.Null(TestClassWithDefaultValues.String);
            Assert.Equal(0, TestClassWithDefaultValues.Int);
            Assert.False(TestClassWithDefaultValues.Bool);
        }

    }

    [Collection("OperandsAttribute")]
    public class OperandsAttribute
    {
        [Fact]
        public void Constructor()
        {
            CommandLine.OperandsAttribute test = new CommandLine.OperandsAttribute();
        }

    }

    public class TestClassDuplicateProperties
    {
        [CommandLine.Argument('h', "hello")]
        public static string Test1 { get; set; }

        [CommandLine.Argument('h', "hello")]
        public static string Test2 { get; set; }
    }

    public class TestClassPublicProperties
    {
        [CommandLine.Operands]
        public static string[] Operands { get; set; }

        [CommandLine.Argument('t', "test")]
        public static string Test { get; set; }
    }

    [Collection("Arguments")]
    public class TestClassWithArrayOperands
    {
        [CommandLine.Operands]
        public static string[] Operands { get; set; }

        [Fact]
        public void PopulateOperands()
        {
            CommandLine.Arguments.Populate(GetType(), "one two");

            Assert.Equal("one", Operands[0]);
            Assert.Equal("two", Operands[1]);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithArrayProperty
    {
        [CommandLine.Argument('a', "array")]
        private static string[] Array { get; set; }

        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a one -a two -a three"));

            Assert.Null(ex);
            Assert.Equal(3, Array.Length);
            Assert.Equal("one", Array[0]);
            Assert.Equal("two", Array[1]);
            Assert.Equal("three", Array[2]);
        }

        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a one"));

            Assert.Null(ex);
            Assert.Single(Array);
            Assert.Equal("one", Array[0]);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithBadOperands
    {
        [CommandLine.Operands]
        private static bool Operands { get; set; }

        [Fact]
        public void PopulateOperands()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "one two"));

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithListProperty
    {
        [CommandLine.Argument('l', "list")]
        private static List<string> List { get; set; }

        [Fact]
        public void PopulateShort()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-l one -l two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateLong()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--list one --list two --list three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateLongAndShort()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-l one --list two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-l one"));

            Assert.Null(ex);
            Assert.Single(List);
            Assert.Equal("one", List[0]);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithNoProperties
    {
        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--hello world one two"));

            Assert.Null(ex);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithDefaultValues
    {
        [CommandLine.Argument('a', "aa")]
        public static string String { get; set; } = "foo";

        [CommandLine.Argument('b', "bb")]
        public static int Int { get; set; } = 42;

        [CommandLine.Argument('c', "cc")]
        public static bool Bool { get; set; } = true;
    }

    [Collection("Arguments")]
    public class TestClassWithBoolProperty
    {
        [CommandLine.Argument('a', "aa")]
        private static bool A { get; set; }

        [CommandLine.Operands]
        private static List<string> Operands { get; set; }

        [Fact]
        public void PopulateBoolFollowedByOperand()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a operand"));

            Assert.Null(ex);
            Assert.True(A);
            Assert.Single(Operands);
            Assert.Equal("operand", Operands[0]);
        }

        [Fact]
        public void PopulateBoolFollowedByTwoOperands()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a operand1 operand2"));

            Assert.Null(ex);
            Assert.True(A);
            Assert.Equal(2, Operands.Count);
            Assert.Equal("operand1", Operands[0]);
            Assert.Equal("operand2", Operands[1]);
        }
    }
}