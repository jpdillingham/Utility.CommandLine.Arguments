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

namespace Utility.CommandLine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    [Collection("ArgumentAttribute")]
    public class ArgumentAttributeTests
    {
        [Fact]
        public void Constructor()
        {
            ArgumentAttribute test = new ArgumentAttribute('n', "name", "help");

            Assert.Equal('n', test.ShortName);
            Assert.Equal("name", test.LongName);
            Assert.Equal("help", test.HelpText);
        }
    }

    [Collection("ArgumentInfo")]
    public class ArgumentInfoTests
    {
        private string Test { get; set; }

        [Fact]
        public void Constructor()
        {
            ArgumentInfo test = new ArgumentInfo('a', "aa", "help", GetType().GetProperty("Test", BindingFlags.Instance | BindingFlags.NonPublic));

            Assert.Equal('a', test.ShortName);
            Assert.Equal("aa", test.LongName);
            Assert.Equal("help", test.HelpText);
            Assert.Equal("Test", test.Property.Name);
        }
    }

    [Collection("Arguments")]
    public class ArgumentsTests
    {
        public enum Enums
        {
            Foo = 1,
            Bar = 2,
        }

        [Argument('b', "bool", "help")]
        private static bool Bool { get; set; }

        [Argument('d', "decimal")]
        private static decimal Decimal { get; set; }

        [Argument('i', "integer")]
        private static int Integer { get; set; }

        [Argument('c', "case-sensitive")]
        private static string LowerCase { get; set; }

        private static string NonArgumentProperty { get; set; }

        [Operands]
        private static List<string> Operands { get; set; }

        [Obsolete]
        private static string PlainProperty { get; set; }

        [Argument('t', "test-prop")]
        private static string TestProp { get; set; }

        [Argument('C', "CASE-SENSITIVE")]
        private static string UpperCase { get; set; }

        [Argument('e', "enum")]
        private static Enums Enum { get; set; }

        [Fact]
        public void GetArgumentInfo()
        {
            var help = Arguments.GetArgumentInfo(typeof(ArgumentsTests)).ToList();

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Single(help.Where(h => h.LongName == "test-prop"));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        [Fact]
        public void GetArgumentInfoNull()
        {
            var help = Arguments.GetArgumentInfo().ToList();

            Assert.Equal(7, help.Count);
            Assert.Single(help.Where(h => h.ShortName == 'b'));
            Assert.Equal("help", help.Where(h => h.ShortName == 'b').FirstOrDefault().HelpText);
        }

        [Fact]
        public void Indexer()
        {
            Arguments test = Arguments.Parse("--test one --two three");

            Assert.Equal("one", test["test"]);
            Assert.Equal("three", test["two"]);
        }

        [Fact]
        public void Parse()
        {
            Dictionary<string, object> test = Arguments.Parse().ArgumentDictionary;

            Assert.NotEmpty(test);
        }

        [Fact]
        public void ParseCaseSensitive()
        {
            Dictionary<string, object> test = Arguments.Parse("--TEST -aBc").ArgumentDictionary;

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
            Arguments test = Arguments.Parse("hello-world");

            Assert.Equal("hello-world", test.OperandList[0]);
        }

        [Fact]
        public void ParseDecimal()
        {
            Arguments test = Arguments.Parse("--decimal 1.1");

            Assert.Equal("1.1", test["decimal"]);
        }

        [Fact]
        public void ParseEmpty()
        {
            Exception ex = Record.Exception(() => Arguments.Parse(string.Empty));

            Assert.Null(ex);
        }

        [Fact]
        public void ParseInnerQuotedStrings()
        {
            Dictionary<string, object> test = Arguments.Parse("--test1 \"test \'1\'\" --test2 \'test \"2\"\'").ArgumentDictionary;

            Assert.Equal("test \'1\'", test["test1"]);
            Assert.Equal("test \"2\"", test["test2"]);
        }

        [Fact]
        public void ParseLongAndShortMix()
        {
            Dictionary<string, object> test = Arguments.Parse("--one=1 -ab 2 --three:3 -4 4").ArgumentDictionary;

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
            Arguments test = Arguments.Parse("--test one two --three four");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("four", test.ArgumentDictionary["three"]);
        }

        [Fact]
        public void ParseMultipleQuotes()
        {
            Dictionary<string, object> test = Arguments.Parse("--test1 \"1\" --test2 \"2\" --test3 \'3\' --test4 \'4\'").ArgumentDictionary;

            Assert.Equal("1", test["test1"]);
            Assert.Equal("2", test["test2"]);
            Assert.Equal("3", test["test3"]);
            Assert.Equal("4", test["test4"]);
        }

        [Fact]
        public void ParseNoArgument()
        {
            Exception ex = Record.Exception(() => Arguments.Parse());

            Assert.Null(ex);
        }

        [Fact]
        public void ParseNull()
        {
            Exception ex = Record.Exception(() => Arguments.Parse(null));

            Assert.Null(ex);
        }

        [Fact]
        public void ParseOnlyOperands()
        {
            Arguments test = Arguments.Parse("hello world!");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("hello", test.OperandList[0]);
            Assert.Equal("world!", test.OperandList[1]);
        }

        [Fact]
        public void ParseOperand()
        {
            Arguments test = Arguments.Parse("--test one two");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
        }

        [Fact]
        public void ParseSingleCharacterLong()
        {
            Arguments test = Arguments.Parse("--t one");

            Assert.Single(test.ArgumentDictionary);
            Assert.Equal("one", test.ArgumentDictionary["t"]);
        }

        [Fact]
        public void ParseOperands()
        {
            Arguments test = Arguments.Parse("--test one two three four");

            Assert.Equal(3, test.OperandList.Count);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("three", test.OperandList[1]);
            Assert.Equal("four", test.OperandList[2]);
        }

        [Fact]
        public void ParseShorts()
        {
            Dictionary<string, object> test = Arguments.Parse("-abc 'hello world'").ArgumentDictionary;

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
            Arguments test = Arguments.Parse("--");

            Assert.Empty(test.OperandList);
        }

        [Fact]
        public void ParseStrictOperandMultipleDelimiter()
        {
            Arguments test = Arguments.Parse("one -- two -- three");

            Assert.Equal(4, test.OperandList.Count);
            Assert.Equal("one", test.OperandList[0]);
            Assert.Equal("two", test.OperandList[1]);
            Assert.Equal("--", test.OperandList[2]);
            Assert.Equal("three", test.OperandList[3]);
        }

        [Fact]
        public void ParseStrictOperands()
        {
            Arguments test = Arguments.Parse("--test one two -- three -four --five /six \"seven eight\" 'nine ten'");

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
            Arguments test = Arguments.Parse("--test one two --");

            Assert.Single(test.OperandList);
            Assert.Equal("two", test.OperandList[0]);
        }

        [Fact]
        public void ParseStrictOperandsStart()
        {
            Arguments test = Arguments.Parse("-- one two");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("one", test.OperandList[0]);
            Assert.Equal("two", test.OperandList[1]);
        }

        [Fact]
        public void ParseStringOfLongs()
        {
            Dictionary<string, object> test = Arguments.Parse("--one 1 --two=2 --three:3 --four \"4 4\" --five='5 5'").ArgumentDictionary;

            Assert.NotEmpty(test);
            Assert.Equal(5, test.Count);
            Assert.Equal("1", test["one"]);
            Assert.Equal("2", test["two"]);
            Assert.Equal("3", test["three"]);
            Assert.Equal("4 4", test["four"]);
            Assert.Equal("5 5", test["five"]);
        }

        [Fact]
        public void ParseMultiples_No_CombineAllMultiples()
        {
            Dictionary<string, object> test = Arguments.Parse("--test 1 --test 2", options => options.CombineAllMultiples = false).ArgumentDictionary;

            Assert.NotEmpty(test);
            Assert.Single(test);
            Assert.Equal("2", test["test"]);
        }

        [Fact]
        public void ParseMultiples_With_CombineAllMultiples()
        {
            Dictionary<string, object> test = Arguments.Parse("--test 1 --test 2", options => options.CombineAllMultiples = true).ArgumentDictionary;

            Assert.NotEmpty(test);
            Assert.Single(test);

            var values = (List<object>)test["test"];

            Assert.Equal(2, values.Count);
            Assert.Equal("1", values[0]);
            Assert.Equal("2", values[1]);
        }

        [Fact]
        public void ParseMultiples_With_Specific_CombinableArguments()
        {
            Dictionary<string, object> test = Arguments.Parse("--test 1 --test 2 --foo foo --foo bar", options => options.CombinableArguments = new[] { "test" }).ArgumentDictionary;

            Assert.NotEmpty(test);
            Assert.Equal(2, test.Count);

            var values = (List<object>)test["test"];

            Assert.Equal(2, values.Count);
            Assert.Equal("1", values[0]);
            Assert.Equal("2", values[1]);

            Assert.Equal("bar", test["foo"]);
        }

        [Fact]
        public void ParseValueBeginningWithSlash()
        {
            Dictionary<string, object> test = Arguments.Parse("--file='/mnt/data/test.xml'").ArgumentDictionary;

            Assert.Equal("/mnt/data/test.xml", test["file"]);
        }

        [Fact]
        public void ParseValueWithQuotedPeriod()
        {
            Dictionary<string, object> test = Arguments.Parse("--test \"test.test\" --test2 'test2.test2'").ArgumentDictionary;

            Assert.Equal("test.test", test["test"]);
            Assert.Equal("test2.test2", test["test2"]);
        }

        [Theory]
        [InlineData(".")]
        [InlineData("..")]
        [InlineData("..\\")]
        [InlineData(".\\foo")]
        [InlineData("..\\foo")]
        [InlineData("..\\..")]
        [InlineData("\\")]
        [InlineData("\\foo")]
        [InlineData("\\foo\\bar")]
        [InlineData("\\\\foo")]
        [InlineData("\\\\foo\\bar")]
        [InlineData("..//")]
        [InlineData(".//foo")]
        [InlineData("..//foo")]
        [InlineData("..//..")]
        [InlineData("//")]
        [InlineData("//foo")]
        [InlineData("//foo//bar")]
        [InlineData("////foo")]
        [InlineData("////foo//bar")]
        public void ParseValueStartingWithNonWord(string value)
        {
            Dictionary<string, object> test = Arguments.Parse($"-f {value}").ArgumentDictionary;

            Assert.Equal(value, test["f"]);
        }

        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => Arguments.Populate("-b"));

            Assert.Null(ex);
        }

        [Fact]
        public void PopulateBogusCaller()
        {
            Exception ex = Record.Exception(() => Arguments.Populate("-b", true, Guid.NewGuid().ToString()));

            Assert.NotNull(ex);
            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void PopulateCaseSensitive()
        {
            Arguments.Populate("-c lower -C upper");

            Assert.Equal("lower", LowerCase);
            Assert.Equal("upper", UpperCase);

            Arguments.Populate("--case-sensitive lower --CASE-SENSITIVE upper");

            Assert.Equal("lower", LowerCase);
            Assert.Equal("upper", UpperCase);
        }

        [Fact]
        public void PopulateDecimal()
        {
            Arguments.Populate("--decimal 1.1");

            Assert.Equal(1.1M, Decimal);
        }

        [Fact]
        public void PopulateEnum()
        {
            Arguments.Populate("--enum bar");

            Assert.Equal(Enums.Bar, Enum);
        }

        [Fact]
        public void PopulateDisableClearing()
        {
            Bool = true;
            Decimal = 3.5m;
            Integer = 42;

            Arguments.Populate(string.Empty, false);

            Assert.True(Bool);
            Assert.Equal(3.5m, Decimal);
            Assert.Equal(42, Integer);
        }

        [Fact]
        public void PopulateDuplicateProperties()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(typeof(TestClassDuplicateProperties), "--hello world"));

            Assert.Null(ex);

            Assert.Equal("world", TestClassDuplicateProperties.Test1);
            Assert.Equal(default(string), TestClassDuplicateProperties.Test2);
        }

        [Fact]
        public void PopulateExternalClass()
        {
            Arguments.Populate(typeof(TestClassPublicProperties), "--test test! operand1 operand2");

            Assert.Equal("test!", TestClassPublicProperties.Test);
            Assert.Equal("operand1", TestClassPublicProperties.Operands[0]);
            Assert.Equal("operand2", TestClassPublicProperties.Operands[1]);
        }

        [Fact]
        public void PopulateMultipleValuesNotCollectionBacked()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--integer 1 --integer 2"));

            Assert.Null(ex);

            Assert.Equal(2, Integer);
        }

        [Fact]
        public void PopulateMultipleValuesNotCollectionBacked_Last_Value_Wins()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--integer 2 --integer 1"));

            Assert.Null(ex);

            Assert.Equal(1, Integer);
        }

        [Fact]
        public void PopulateOperands()
        {
            Arguments.Populate(GetType(), "--test one two three");

            Assert.Equal("two", Operands[0]);
            Assert.Equal("three", Operands[1]);
        }

        [Fact]
        public void PopulateShortNames()
        {
            Arguments.Populate("-bi 3");

            Assert.True(Bool);
            Assert.Equal(3, Integer);
        }

        [Fact]
        public void PopulateString()
        {
            Arguments.Populate("--test-prop 'hello world!' --bool --integer 5");

            Assert.Equal("hello world!", TestProp);
            Assert.True(Bool);
            Assert.Equal(5, Integer);
        }

        [Fact]
        public void PopulateType()
        {
            Arguments.Populate(GetType(), "--integer 5");

            Assert.Equal(5, Integer);
        }

        [Fact]
        public void PopulateTypeMismatch()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--integer five"));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void SetsDefaultValuesOnPopulate()
        {
            Arguments.Populate(typeof(TestClassWithDefaultValues));

            Assert.Null(TestClassWithDefaultValues.String);
            Assert.Equal(0, TestClassWithDefaultValues.Int);
            Assert.False(TestClassWithDefaultValues.Bool);
        }
    }

    [Collection("OperandsAttribute")]
    public class OperandsAttributeTests
    {
        [Fact]
        public void Constructor()
        {
            OperandsAttribute test = new CommandLine.OperandsAttribute();
        }
    }

    public class TestClassDuplicateProperties
    {
        [Argument('h', "hello")]
        public static string Test1 { get; set; }

        [Argument('h', "hello")]
        public static string Test2 { get; set; }
    }

    public class TestClassPublicProperties
    {
        [CommandLine.Operands]
        public static string[] Operands { get; set; }

        [Argument('t', "test")]
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
            Arguments.Populate(GetType(), "one two");

            Assert.Equal("one", Operands[0]);
            Assert.Equal("two", Operands[1]);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithArrayProperty
    {
        [Argument('a', "array")]
        private static string[] Array { get; set; }

        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-a one -a two -a three"));

            Assert.Null(ex);
            Assert.Equal(3, Array.Length);
            Assert.Equal("one", Array[0]);
            Assert.Equal("two", Array[1]);
            Assert.Equal("three", Array[2]);
        }

        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-a one"));

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
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "one two"));

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithListProperty
    {
        [Argument('l', "list")]
        private static List<string> List { get; set; }

        [Fact]
        public void PopulateShort()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-l one -l two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateLong()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--list one --list two --list three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateLongAndShort()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-l one --list two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateShortAndLong()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--list one -l two --list three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateLongThenShort()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--list one --list two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateShortThenLong()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-l one -l two --list three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-l one"));

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
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "--hello world one two"));

            Assert.Null(ex);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithDefaultValues
    {
        [Argument('a', "aa")]
        public static string String { get; set; } = "foo";

        [Argument('b', "bb")]
        public static int Int { get; set; } = 42;

        [Argument('c', "cc")]
        public static bool Bool { get; set; } = true;
    }

    [Collection("Arguments")]
    public class TestClassWithListAndPrimitive
    {
        [Argument('b', "bb")]
        public static int Int { get; set; } = 42;

        [Argument('l', "list")]
        private static List<string> List { get; set; }

        [Fact]
        public void List_Is_Appended_Given_Two_Short_Args()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("l", "foo"),
                new KeyValuePair<string, string>("l", "bar")
            };

            var a = Arguments.Parse("-l foo -l bar", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            List<object> argList = null;
            var ex = Record.Exception(() => argList = (List<object>)a.ArgumentDictionary["l"]);

            Assert.Single(dict);

            Assert.Null(ex);
            Assert.Equal(2, argList.Count);
            Assert.Equal("foo", argList[0]);
            Assert.Equal("bar", argList[1]);
        }

        [Fact]
        public void List_Is_Appended_Given_Two_Long_Args()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("list", "foo"),
                new KeyValuePair<string, string>("list", "bar")
            };

            var a = Arguments.Parse("--list foo --list bar", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            List<object> argList = null;
            var ex = Record.Exception(() => argList = (List<object>)a.ArgumentDictionary["list"]);

            Assert.Single(dict);

            Assert.Null(ex);
            Assert.Equal(2, argList.Count);
            Assert.Equal("foo", argList[0]);
            Assert.Equal("bar", argList[1]);
        }

        [Fact]
        public void List_Is_Appended_Given_Mixed_Args_Short_First()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("l", "foo"),
                new KeyValuePair<string, string>("list", "bar")
            };

            var a = Arguments.Parse("-l foo --list bar", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            List<object> argList = null;
            var ex = Record.Exception(() => argList = (List<object>)a.ArgumentDictionary["l"]);

            Assert.Single(dict);

            Assert.Null(ex);
            Assert.Equal(2, argList.Count);
            Assert.Equal("foo", argList[0]);
            Assert.Equal("bar", argList[1]);
        }

        [Fact]
        public void List_Is_Appended_Given_Mixed_Args_Long_First()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("list", "foo"),
                new KeyValuePair<string, string>("l", "bar")
            };

            var a = Arguments.Parse("--list foo -l bar", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            List<object> argList = null;
            var ex = Record.Exception(() => argList = (List<object>)a.ArgumentDictionary["list"]);

            Assert.Single(dict);

            Assert.Null(ex);
            Assert.Equal(2, argList.Count);
            Assert.Equal("foo", argList[0]);
            Assert.Equal("bar", argList[1]);
        }

        [Fact]
        public void List_Is_Not_Appended_Given_Mixed_Args_Long_First_No_Type()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("list", "foo"),
                new KeyValuePair<string, string>("l", "bar")
            };

            var a = Arguments.Parse("--list foo -l bar");
            var dict = a.ArgumentDictionary;

            Assert.Equal(2, dict.Count);
            Assert.True(dict.ContainsKey("list"));
            Assert.True(dict.ContainsKey("l"));
            Assert.Equal("foo", dict["list"]);
            Assert.Equal("bar", dict["l"]);
        }

        [Fact]
        public void Value_Is_Replaced_Given_Multiple_Short()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("b", "1"),
                new KeyValuePair<string, string>("b", "2")
            };

            var a = Arguments.Parse("-b 1 -b 2", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            Assert.Single(dict);

            Assert.Equal("2", dict["b"]);
        }

        [Fact]
        public void Value_Is_Replaced_Given_Multiple_Long()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            Assert.Single(dict);

            Assert.Equal("2", dict["bb"]);
        }

        [Fact]
        public void Value_Is_Replaced_Given_Multiple_Long_No_Type()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2");
            var dict = a.ArgumentDictionary;

            Assert.Single(dict);
            Assert.Equal("2", dict["bb"]);

            Assert.Equal(2, a.ArgumentList.Count);
            Assert.True(a.ArgumentList.Where(r => r.Key == "bb" && r.Value == "1").Any());
            Assert.True(a.ArgumentList.Where(r => r.Key == "bb" && r.Value == "2").Any());
        }

        [Fact]
        public void ArgumentList_Retains_Replaced_Arguments()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2");

            Assert.Equal(2, a.ArgumentList.Count);
            Assert.True(a.ArgumentList.Where(r => r.Key == "bb" && r.Value == "1").Any());
            Assert.True(a.ArgumentList.Where(r => r.Key == "bb" && r.Value == "2").Any());
        }

        [Fact]
        public void Arguments_Returns_ArgumentList_Elements_Via_Indexer()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2");

            Assert.Equal("1", a[0]);
            Assert.Equal("2", a[1]);
        }

        [Fact]
        public void Arguments_Sets_TargetType_Given_Type()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2", options => options.TargetType = GetType());

            Assert.NotNull(a.TargetType);
            Assert.Equal(GetType(), a.TargetType);
        }

        [Fact]
        public void Arguments_Does_Not_Set_TargetType_Given_No_Type()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("--bb 1 --bb 2");

            Assert.Null(a.TargetType);
        }

        [Fact]
        public void Value_Is_Replaced_Given_Mixed_Args_Long_First()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("bb", "1"),
                new KeyValuePair<string, string>("b", "2")
            };

            var a = Arguments.Parse("--bb 1 -b 2", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            Assert.Single(dict);

            Assert.Equal("2", dict["bb"]);
        }

        [Fact]
        public void Value_Is_Replaced_Given_Mixed_Args_Short_First()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("b", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("-b 1 --bb 2", options => options.TargetType = GetType());
            var dict = a.ArgumentDictionary;

            Assert.Single(dict);

            Assert.Equal("2", dict["b"]);
        }

        [Fact]
        public void Value_Is_Not_Replaced_Given_Mixed_Args_Short_First_No_Type()
        {
            var list = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("b", "1"),
                new KeyValuePair<string, string>("bb", "2")
            };

            var a = Arguments.Parse("-b 1 --bb 2");
            var dict = a.ArgumentDictionary;

            Assert.Equal(2, dict.Count);
            Assert.True(dict.ContainsKey("b"));
            Assert.True(dict.ContainsKey("bb"));
            Assert.Equal("1", dict["b"]);
            Assert.Equal("2", dict["bb"]);
        }
    }

    [Collection("Arguments")]
    public class TestClassWithBoolProperty
    {
        [Argument('a', "aa")]
        private static bool A { get; set; }

        [Operands]
        private static List<string> Operands { get; set; }

        [Fact]
        public void PopulateBoolFollowedByOperand()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-a operand"));

            Assert.Null(ex);
            Assert.True(A);
            Assert.Single(Operands);
            Assert.Equal("operand", Operands[0]);
        }

        [Fact]
        public void PopulateBoolFollowedByTwoOperands()
        {
            Exception ex = Record.Exception(() => Arguments.Populate(GetType(), "-a operand1 operand2"));

            Assert.Null(ex);
            Assert.True(A);
            Assert.Equal(2, Operands.Count);
            Assert.Equal("operand1", Operands[0]);
            Assert.Equal("operand2", Operands[1]);
        }
    }

    [Collection("Arguments")]
    public class TestWithAllBools
    {
        [Argument('a', "aa")]
        private static bool A { get; set; }

        [Argument('b', "bb")]
        private static bool B { get; set; }

        [Argument('c', "cc")]
        private static bool C { get; set; }

        [Theory]
        [InlineData("-abc")]
        [InlineData("-a -b -c")]
        [InlineData("--aa --bb --cc")]
        [InlineData("-a -b -c")]
        [InlineData("--aa --bb --cc")]
        [InlineData("-a --bb -c")]
        [InlineData("--aa -b -c")]
        [InlineData("-a --bb --cc")]
        public void PopulateStackedBools(string str)
        {
            Arguments.Populate(GetType(), str);

            Assert.True(A && B && C);
        }
    }

    [Collection("Arguments")]
    public class TestListProp
    {
        [Argument('a', "aa")]
        private static string A { get; set; }

        [Argument('b', "bb")]
        private static List<string> B { get; set; }

        [Fact]
        public void PopulateSingle()
        {
            var ex = Record.Exception(() => Arguments.Populate(GetType(), "--aa one --aa two"));

            Assert.Null(ex);
            Assert.Equal("two", A);
        }

        [Fact]
        public void PopulateList()
        {
            var ex = Record.Exception(() => Arguments.Populate(GetType(), "--bb one --bb two"));

            Assert.Null(ex);
            Assert.Equal(2, B.Count);
            Assert.Equal("one", B[0]);
            Assert.Equal("two", B[1]);
        }
    }
}