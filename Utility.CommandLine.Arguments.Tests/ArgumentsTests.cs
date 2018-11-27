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
using System.Diagnostics.CodeAnalysis;
using Xunit;

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]

namespace Utility.CommandLine.Tests
{
    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.ArgumentAttribute"/> class.
    /// </summary>
    [Collection("ArgumentAttribute")]
    public class ArgumentAttribute
    {
        #region Public Methods

        /// <summary>
        ///     Tests the constructor and all properties.
        /// </summary>
        [Fact]
        public void Constructor()
        {
            CommandLine.ArgumentAttribute test = new CommandLine.ArgumentAttribute('n', "name");

            Assert.Equal('n', test.ShortName);
            Assert.Equal("name", test.LongName);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="Utility.CommandLine.Arguments"/> class.
    /// </summary>
    [Collection("Arguments")]
    public class Arguments
    {
        #region Private Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the test property has been set.
        /// </summary>
        [CommandLine.Argument('b', "bool")]
        private static bool Bool { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('d', "decimal")]
        private static decimal Decimal { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('i', "integer")]
        private static int Integer { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('c', "case-sensitive")]
        private static string LowerCase { get; set; }

        /// <summary>
        ///     Gets or sets a property with no attribute.
        /// </summary>
        private static string NonArgumentProperty { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Operands]
        private static List<string> Operands { get; set; }

        /// <summary>
        ///     Gets or sets a property with an attribute other than Argument
        /// </summary>
        [Obsolete]
        private static string PlainProperty { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('t', "test-prop")]
        private static string TestProp { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('C', "CASE-SENSITIVE")]
        private static string UpperCase { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Indexer"/> of <see cref="Utility.CommandLine.Arguments"/>.
        /// </summary>
        [Fact]
        public void Indexer()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one --two three");

            Assert.Equal("one", test["test"]);
            Assert.Equal("three", test["two"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with the default values.
        /// </summary>
        [Fact]
        public void Parse()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse().ArgumentDictionary;

            Assert.NotEmpty(test);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing a mixture of upper and lower case arguments.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a string coning a single operand
        ///     which contains a dash.
        /// </summary>
        [Fact]
        public void ParseDashedOperand()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("hello-world");

            Assert.Equal("hello-world", test.OperandList[0]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a decimal value.
        /// </summary>
        [Fact]
        public void ParseDecimal()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--decimal 1.1");

            Assert.Equal("1.1", test["decimal"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an empty string.
        /// </summary>
        [Fact]
        public void ParseEmpty()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse(string.Empty));

            Assert.Null(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing values with inner quoted strings.
        /// </summary>
        [Fact]
        public void ParseInnerQuotedStrings()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test1 \"test \'1\'\" --test2 \'test \"2\"\'").ArgumentDictionary;

            Assert.Equal("test \'1\'", test["test1"]);
            Assert.Equal("test \"2\"", test["test2"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing a mixture of short and long arguments.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a mixture of arguments and operands.
        /// </summary>
        [Fact]
        public void ParseMixedArgumentsAndOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two --three four");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("four", test.ArgumentDictionary["three"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing multiple pairs of arguments containing quoted values.
        /// </summary>
        [Fact]
        public void ParseMultipleQuotes()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test1 \"1\" --test2 \"2\" --test3 \'3\' --test4 \'4\'").ArgumentDictionary;

            Assert.Equal("1", test["test1"]);
            Assert.Equal("2", test["test2"]);
            Assert.Equal("3", test["test3"]);
            Assert.Equal("4", test["test4"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with no argument.
        /// </summary>
        [Fact]
        public void ParseNoArgument()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse());

            Assert.Null(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a null argument.
        /// </summary>
        [Fact]
        public void ParseNull()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Parse(null));

            Assert.Null(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a string containing only a series
        ///     of operands.
        /// </summary>
        [Fact]
        public void ParseOnlyOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("hello world!");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("hello", test.OperandList[0]);
            Assert.Equal("world!", test.OperandList[1]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a string containing an operand.
        /// </summary>
        [Fact]
        public void ParseOperand()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two");

            Assert.Equal("one", test.ArgumentDictionary["test"]);
            Assert.Equal("two", test.OperandList[0]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a string containing multiple operands.
        /// </summary>
        [Fact]
        public void ParseOperands()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two three four");

            Assert.Equal(3, test.OperandList.Count);
            Assert.Equal("two", test.OperandList[0]);
            Assert.Equal("three", test.OperandList[1]);
            Assert.Equal("four", test.OperandList[2]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing only short parameters.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a a string containing only the
        ///     strict operand delimiter.
        /// </summary>
        [Fact]
        public void ParseStrictOperandDelimiterOnly()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--");

            Assert.Empty(test.OperandList);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a a string containing multiple
        ///     strict operand delimiters.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit operand delimiter.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit operand delimiter, and
        ///     nothing after the delimiter.
        /// </summary>
        [Fact]
        public void ParseStrictOperandsEmpty()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("--test one two --");

            Assert.Single(test.OperandList);
            Assert.Equal("two", test.OperandList[0]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with a a string beginning with the
        ///     explicit operand delimiter.
        /// </summary>
        [Fact]
        public void ParseStrictOperandsStart()
        {
            CommandLine.Arguments test = CommandLine.Arguments.Parse("-- one two");

            Assert.Equal(2, test.OperandList.Count);
            Assert.Equal("one", test.OperandList[0]);
            Assert.Equal("two", test.OperandList[1]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing only long parameters.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing a value beginning with a slash.
        /// </summary>
        [Fact]
        public void ParseValueBeginningWithSlash()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--file=/mnt/data/test.xml").ArgumentDictionary;

            Assert.Equal("/mnt/data/test.xml", test["file"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing arguments with values enclosed in quotes and containing a period.
        /// </summary>
        [Fact]
        public void ParseValueWithQuotedPeriod()
        {
            Dictionary<string, object> test = CommandLine.Arguments.Parse("--test \"test.test\" --test2 'test2.test2'").ArgumentDictionary;

            Assert.Equal("test.test", test["test"]);
            Assert.Equal("test2.test2", test["test2"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with the default values.
        /// </summary>
        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate("-b"));

            Assert.Null(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit command line string
        ///     containing both upper and lower case arguments.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Dictionary{string, object})"/> method with a decimal value.
        /// </summary>
        [Fact]
        public void PopulateDecimal()
        {
            CommandLine.Arguments.Populate("--decimal 1.1");

            Assert.Equal(1.1M, Decimal);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Dictionary{string, object})"/> method.
        /// </summary>
        [Fact]
        public void PopulateDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "i", "1" },
            };

            CommandLine.Arguments.Populate(dict);

            Assert.Equal(1, Integer);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit command line
        ///     string and with a class containing duplicate properties.
        /// </summary>
        [Fact]
        public void PopulateDuplicateProperties()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(typeof(TestClassDuplicateProperties), "--hello world"));

            Assert.Null(ex);

            Assert.Equal("world", TestClassDuplicateProperties.Test1);
            Assert.Equal(default(string), TestClassDuplicateProperties.Test2);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with a Type external to the
        ///     calling class and with an explicit string.
        /// </summary>
        [Fact]
        public void PopulateExternalClass()
        {
            CommandLine.Arguments.Populate(typeof(TestClassPublicProperties), "--test test! operand1 operand2");

            Assert.Equal("test!", TestClassPublicProperties.Test);
            Assert.Equal("operand1", TestClassPublicProperties.Operands[0]);
            Assert.Equal("operand2", TestClassPublicProperties.Operands[1]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with a string containing
        ///     multiple values for an argument which is not backed by a collection.
        /// </summary>
        [Fact]
        public void PopulateMultipleValuesNotCollectionBacked()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--integer 1 --integer 2"));

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit command line
        ///     string containing two operands.
        /// </summary>
        [Fact]
        public void PopulateOperands()
        {
            CommandLine.Arguments.Populate(GetType(), "--test one two three");

            Assert.Equal("two", Operands[0]);
            Assert.Equal("three", Operands[1]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit command line string
        ///     containing multiple short names.
        /// </summary>
        [Fact]
        public void PopulateShortNames()
        {
            CommandLine.Arguments.Populate("-bi 3");

            Assert.True(Bool);
            Assert.Equal(3, Integer);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit command line string.
        /// </summary>
        [Fact]
        public void PopulateString()
        {
            CommandLine.Arguments.Populate("--test-prop 'hello world!' --bool --integer 5");

            Assert.Equal("hello world!", TestProp);
            Assert.True(Bool);
            Assert.Equal(5, Integer);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit type.
        /// </summary>
        [Fact]
        public void PopulateType()
        {
            CommandLine.Arguments.Populate(GetType(), "--integer 5");

            Assert.Equal(5, Integer);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit type and command
        ///     line string, where the string contains a value which does not match the type of the destination property.
        /// </summary>
        [Fact]
        public void PopulateTypeMismatch()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--integer five"));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.OperandsAttribute"/> class.
    /// </summary>
    [Collection("OperandsAttribute")]
    public class OperandsAttribute
    {
        #region Public Methods

        /// <summary>
        ///     Tests the constructor and all properties.
        /// </summary>
        [Fact]
        public void Constructor()
        {
            CommandLine.OperandsAttribute test = new CommandLine.OperandsAttribute();
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class with duplicate properties.</remarks>
    public class TestClassDuplicateProperties
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('h', "hello")]
        public static string Test1 { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('h', "hello")]
        public static string Test2 { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class with public properties.</remarks>
    public class TestClassPublicProperties
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Operands]
        public static string[] Operands { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('t', "test")]
        public static string Test { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>
    ///     Used to facilitate testing of a property marked with the Operands attribute which is not of type string[] or List{string}
    /// </remarks>
    [Collection("Arguments")]
    public class TestClassWithArrayOperands
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Operands]
        public static string[] Operands { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing two operands, and with a Type containing a property of type <see langword="string[]"/> marked with the
        ///     <see cref="Operands"/> attribute.
        /// </summary>
        [Fact]
        public void PopulateOperands()
        {
            CommandLine.Arguments.Populate(GetType(), "one two");

            Assert.Equal("one", Operands[0]);
            Assert.Equal("two", Operands[1]);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class with an array property.</remarks>
    [Collection("Arguments")]
    public class TestClassWithArrayProperty
    {
        #region Private Properties

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('a', "array")]
        private static string[] Array { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing multiple instances of the same argument.
        /// </summary>
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing a single a single instance of an array-backed argument.
        /// </summary>
        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a one"));

            Assert.Null(ex);
            Assert.Single(Array);
            Assert.Equal("one", Array[0]);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>
    ///     Used to facilitate testing of a property marked with the Operands attribute which is not of type string[] or List{string}
    /// </remarks>
    [Collection("Arguments")]
    public class TestClassWithBadOperands
    {
        #region Private Properties

        /// <summary>
        ///     Gets or sets a value indicating whether .. nothing actually. this is a test and this documentation is appeasing StyleCop.
        /// </summary>
        [CommandLine.Operands]
        private static bool Operands { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing two operands, and with a Type containing a property marked with the <see cref="Operands"/> attribute but
        ///     that is not of type <see cref="T:string[]"/> or <see cref="List{T}"/>}"/&gt; .
        /// </summary>
        [Fact]
        public void PopulateOperands()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "one two"));

            Assert.NotNull(ex);
            Assert.IsType<InvalidCastException>(ex);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class with a List{T} property.</remarks>
    [Collection("Arguments")]
    public class TestClassWithListProperty
    {
        #region Private Properties

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('l', "list")]
        private static List<string> List { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing multiple instances of a list-backed argument.
        /// </summary>
        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-l one -l two -l three"));

            Assert.Null(ex);
            Assert.Equal(3, List.Count);
            Assert.Equal("one", List[0]);
            Assert.Equal("two", List[1]);
            Assert.Equal("three", List[2]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing a single a single instance of a list-backed argument.
        /// </summary>
        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-l one"));

            Assert.Null(ex);
            Assert.Single(List);
            Assert.Equal("one", List[0]);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class with no properties.</remarks>
    [Collection("Arguments")]
    public class TestClassWithNoProperties
    {
        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing a single argument/value pair, and with a Type containing no properties.
        /// </summary>
        [Fact]
        public void Populate()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "--hello world one two"));

            Assert.Null(ex);
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     Unit tests for the <see cref="CommandLine.Arguments"/> class.
    /// </summary>
    /// <remarks>Used to facilitate testing of a class a bool property, specifically to test parsing of a bool followed by an operand.</remarks>
    [Collection("Arguments")]
    public class TestClassWithBoolProperty
    {
        [CommandLine.Argument('a', "aa")]
        private static bool A { get; set; }

        [CommandLine.Operands]
        private static List<string> Operands { get; set; }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(Type, string)"/> method with an explicit string
        ///     containing a single boolean followed by a single operand.
        /// </summary>
        [Fact]
        public void PopulateSingle()
        {
            Exception ex = Record.Exception(() => CommandLine.Arguments.Populate(GetType(), "-a operand"));

            Assert.Null(ex);
            Assert.True(A);
            Assert.Equal(1, Operands.Count);
            Assert.Equal("operand", Operands[0]);
        }
    }
}