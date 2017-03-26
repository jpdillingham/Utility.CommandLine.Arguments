/*
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀  ▀  ▀      ▀▀
      █
      █     ▄████████
      █     ███    ███
      █     ███    ███    █████    ▄████▄  ██   █     ▄▄██▄▄▄     ▄█████ ██▄▄▄▄      ██      ▄█████
      █     ███    ███   ██  ██   ██    ▀  ██   ██  ▄█▀▀██▀▀█▄   ██   █  ██▀▀▀█▄ ▀███████▄   ██  ▀
      █   ▀███████████  ▄██▄▄█▀  ▄██       ██   ██  ██  ██  ██  ▄██▄▄    ██   ██     ██  ▀   ██
      █     ███    ███ ▀███████ ▀▀██ ███▄  ██   ██  ██  ██  ██ ▀▀██▀▀    ██   ██     ██    ▀███████
      █     ███    ███   ██  ██   ██    ██ ██   ██  ██  ██  ██   ██   █  ██   ██     ██       ▄  ██
      █     ███    █▀    ██  ██   ██████▀  ██████    █  ██  █    ███████  █   █     ▄██▀    ▄████▀
      █
      █       ███
      █   ▀█████████▄
      █      ▀███▀▀██    ▄█████   ▄█████     ██      ▄█████
      █       ███   ▀   ██   █    ██  ▀  ▀███████▄   ██  ▀
      █       ███      ▄██▄▄      ██         ██  ▀   ██
      █       ███     ▀▀██▀▀    ▀███████     ██    ▀███████
      █       ███       ██   █     ▄  ██     ██       ▄  ██
      █      ▄████▀     ███████  ▄████▀     ▄██▀    ▄████▀
      █
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █
      ▄
      █  Unit tests for the Arguments class.
      █
      █▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀ ▀▀▀▀▀▀▀▀▀▀▀ ▀ ▀▀▀     ▀▀               ▀
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
using Xunit;

namespace Utility.CommandLine.Tests
{
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
        [CommandLine.Argument('i', "integer")]
        private static int Integer { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('p', "parentProcessId")]
        private static string ParentProcessID { get; set; }

        /// <summary>
        ///     Gets or sets a test property.
        /// </summary>
        [CommandLine.Argument('t', "test-prop")]
        private static string TestProp { get; set; }

        [CommandLine.Argument('C', "CASE-SENSITIVE")]
        private static string UpperCase { get; set; }

        [CommandLine.Argument('c', "case-sensitive")]
        private static string LowerCase { get; set; }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with the default values.
        /// </summary>
        [Fact]
        public void Parse()
        {
            Dictionary<string, string> test = CommandLine.Arguments.Parse();

            Assert.NotEmpty(test);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Parse(string)"/> method with an explicit command line string
        ///     containing only long parameters.
        /// </summary>
        [Fact]
        public void ParseStringOfLongs()
        {
            Dictionary<string, string> test = CommandLine.Arguments.Parse("--one 1 --two=2 /three:3 --four \"4 4\" --five='5 5'");

            Assert.NotEmpty(test);
            Assert.Equal(5, test.Count);
            Assert.Equal("1", test["one"]);
            Assert.Equal("2", test["two"]);
            Assert.Equal("3", test["three"]);
            Assert.Equal("4 4", test["four"]);
            Assert.Equal("5 5", test["five"]);
        }

        [Fact]
        public void ParseShorts()
        {
            Dictionary<string, string> test = CommandLine.Arguments.Parse("-abc 'hello world'");

            Assert.True(test.ContainsKey("a"));
            Assert.Equal(string.Empty, test["a"]);

            Assert.True(test.ContainsKey("b"));
            Assert.Equal(string.Empty, test["b"]);

            Assert.True(test.ContainsKey("c"));
            Assert.Equal("hello world", test["c"]);
        }

        [Fact]
        public void ParseCaseSensitive()
        {
            Dictionary<string, string> test = CommandLine.Arguments.Parse("--TEST -aBc");

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
        public void ParseMix()
        {
            Dictionary<string, string> test = CommandLine.Arguments.Parse("--one=1 -ab 2 /three:3 -4 4");

            Assert.Equal("1", test["one"]);
            Assert.True(test.ContainsKey("a"));
            Assert.True(test.ContainsKey("b"));
            Assert.Equal("2", test["b"]);
            Assert.Equal("3", test["three"]);
            Assert.Equal("4", test["4"]);
        }

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with the default values.
        /// </summary>
        [Fact]
        public void Populate()
        {
            CommandLine.Arguments.Populate();

            Assert.NotEqual(string.Empty, ParentProcessID);
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

        [Fact]
        public void PopulateShortNames()
        {
            CommandLine.Arguments.Populate("-bi 3");

            Assert.True(Bool);
            Assert.Equal(3, Integer);
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

        /// <summary>
        ///     Tests the <see cref="Utility.CommandLine.Arguments.Populate(string)"/> method with an explicit type.
        /// </summary>
        [Fact]
        public void PopulateType()
        {
            CommandLine.Arguments.Populate(GetType());

            Assert.NotEqual(string.Empty, ParentProcessID);
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
}