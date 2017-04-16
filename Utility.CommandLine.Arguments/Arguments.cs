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
 ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄▄  ▄▄ ▄▄   ▄▄▄▄ ▄▄     ▄▄     ▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄ ▄ ▄
 █████████████████████████████████████████████████████████████ ███████████████ ██  ██ ██   ████ ██     ██     ████████████████ █ █
      ▄
      █  Provides static methods used to retrieve the command line arguments and operands with which the application was started,
      █  as well as a Type to contain them.
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]

namespace Utility.CommandLine
{
    /// <summary>
    ///     Provides extension method(s) for the Argument namespace.
    /// </summary>
    public static class Extensions
    {
        #region Internal Methods

        /// <summary>
        ///     Adds the specified key to the specified dictionary with the specified value, but only if the specified key is not
        ///     already present in the dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to which they specified key and value are to be added.</param>
        /// <param name="key">The key to add to the dictionary.</param>
        /// <param name="value">The value corresponding to the specified key.</param>
        internal static void ExclusiveAdd(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        #endregion Internal Methods
    }

    /// <summary>
    ///     Indicates that the property is to be used as a target for automatic population of values from command line arguments
    ///     when invoking the <see cref="Arguments.Populate(string)"/> method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ArgumentAttribute : Attribute
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ArgumentAttribute"/> class.
        /// </summary>
        /// <param name="shortName">The short name of the argument, represented as a single character.</param>
        /// <param name="longName">The long name of the argument.</param>
        public ArgumentAttribute(char shortName, string longName)
        {
            ShortName = shortName;
            LongName = longName;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets or sets the long name of the argument.
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        ///     Gets or sets the short name of the argument.
        /// </summary>
        public char ShortName { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    ///     Provides static methods used to retrieve the command line arguments and operands with which the application was
    ///     started, as well as a Type to contain them.
    /// </summary>
    public class Arguments
    {
        #region Private Fields

        /// <summary>
        ///     The regular expression with which to parse the command line string.
        /// </summary>
        private const string ArgumentRegEx = "(?:[-]{1,2}|\\/)([\\w-]+)[=|:| ]?(\\w\\S*|\\\"[^\"]*\\\"|\\\'[^']*\\\')?|([^- ([^'\\\"]+|\"[^\\\"]+\"|\\\'[^']+\\\')";

        /// <summary>
        ///     The regular expression with which to parse argument-value groups.
        /// </summary>
        private const string GroupRegEx = "^-[^-]+";

        /// <summary>
        ///     The regular expression with which to parse strings strictly containing operands.
        /// </summary>
        private const string OperandRegEx = "([^ ([^'\\\"]+|\\\"[^\\\"]+\\\"|\\\'[^']+\\\')";

        /// <summary>
        ///     The regular expression with which to split the command line string explicitly among argument/value pairs and
        ///     operands, and strictly operands.
        /// </summary>
        /// <remarks>
        ///     This regular expression effectively splits a string into two parts; the part before the first "--", and the part
        ///     after. Instances of "--" not surrounded by a word boundary and those enclosed in quotes are ignored.
        /// </remarks>
        private const string StrictOperandSplitRegEx = "(.*?)[^\\\"\\\']\\B-{2}\\B[^\\\"\\\'](.*)";

        #endregion Private Fields

        #region Private Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Arguments"/> class with the specified argument dictionary and operand list.
        /// </summary>
        /// <param name="argumentDictionary">
        ///     The dictionary containing the arguments and values specified in the command line arguments with which the
        ///     application was started.
        /// </param>
        /// <param name="operandList">
        ///     The list containing the operands specified in the command line arguments with which the application was started.
        /// </param>
        private Arguments(Dictionary<string, string> argumentDictionary, List<string> operandList)
        {
            ArgumentDictionary = argumentDictionary;
            OperandList = operandList;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        ///     Gets a dictionary containing the arguments and values specified in the command line arguments with which the
        ///     application was started.
        /// </summary>
        public Dictionary<string, string> ArgumentDictionary { get; private set; }

        /// <summary>
        ///     Gets a list containing the operands specified in the command line arguments with which the application was started.
        /// </summary>
        public List<string> OperandList { get; private set; }

        #endregion Public Properties

        #region Public Indexers

        /// <summary>
        ///     Gets the argument value corresponding to the specified key from the <see cref="ArgumentDictionary"/> property.
        /// </summary>
        /// <param name="index">The key for which the value is to be retrieved.</param>
        /// <returns>The argument value corresponding to the specified key.</returns>
        public string this[string index]
        {
            get
            {
                return ArgumentDictionary[index];
            }
        }

        #endregion Public Indexers

        #region Public Methods

        /// <summary>
        ///     Returns a dictionary containing the values specified in the command line arguments with which the application was
        ///     started, keyed by argument name.
        /// </summary>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        /// <returns>
        ///     The dictionary containing the arguments and values specified in the command line arguments with which the
        ///     application was started.
        /// </returns>
        public static Arguments Parse(string commandLineString = default(string))
        {
            commandLineString = commandLineString == default(string) || commandLineString == string.Empty ? Environment.CommandLine : commandLineString;

            Dictionary<string, string> argumentDictionary;
            List<string> operandList;

            // use the strict operand regular expression to test for/extract the two halves of the string, if the operator is used.
            MatchCollection matches = Regex.Matches(commandLineString, StrictOperandSplitRegEx);

            // if there is a match, the string contains the strict operand delimiter. parse the first and second matches accordingly.
            if (matches.Count > 0 && matches[0].Groups.Count >= 1)
            {
                // the first group of the first match will contain everything in the string prior to the strict operand delimiter,
                // so extract the argument key/value pairs and list of operands from that string.
                argumentDictionary = GetArgumentDictionary(matches[0].Groups[1].Value);
                operandList = GetOperandList(matches[0].Groups[1].Value);

                // the first group of the second match will contain everything in the string after the strict operand delimiter, so
                // extract the operands from that string using the strict method.
                if (matches[0].Groups.Count > 1)
                {
                    List<string> operandListStrict = GetOperandListStrict(matches[0].Groups[2].Value);

                    // join the operand lists.
                    operandList.AddRange(operandListStrict);
                }
            }
            else
            {
                argumentDictionary = GetArgumentDictionary(commandLineString);
                operandList = GetOperandList(commandLineString);
            }

            return new Arguments(argumentDictionary, operandList);
        }

        /// <summary>
        ///     Populates the properties in the invoking class marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> with the values specified in the list of command line
        ///     arguments, if present.
        /// </summary>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        public static void Populate(string commandLineString = default(string))
        {
            Populate(new StackFrame(1).GetMethod().DeclaringType, Parse(commandLineString));
        }

        /// <summary>
        ///     Populates the properties in the specified Type marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> with the values specified in the list of command line
        ///     arguments, if present.
        /// </summary>
        /// <param name="type">
        ///     The Type for which the static properties matching the list of command line arguments are to be populated.
        /// </param>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        public static void Populate(Type type, string commandLineString = default(string))
        {
            Populate(type, Parse(commandLineString));
        }

        /// <summary>
        ///     Populates the properties in the invoking class marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> with the values specified in the specified argument
        ///     dictionary, if present.
        /// </summary>
        /// <param name="argumentDictionary">
        ///     The dictionary containing the argument-value pairs with which the destination properties should be populated
        /// </param>
        public static void Populate(Dictionary<string, string> argumentDictionary)
        {
            Populate(new StackFrame(1).GetMethod().DeclaringType, new Arguments(argumentDictionary, new List<string>()));
        }

        /// <summary>
        ///     Populates the properties in the specified Type marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> with the values specified in the specified argument
        ///     dictionary, if present.
        /// </summary>
        /// <param name="type">
        ///     The Type for which the static properties matching the list of command line arguments are to be populated.
        /// </param>
        /// <param name="arguments">
        ///     The Arguments object containing the dictionary containing the argument-value pairs with which the destination
        ///     properties should be populated and the list of operands.
        /// </param>
        public static void Populate(Type type, Arguments arguments)
        {
            // fetch any properties in the specified type marked with the ArgumentAttribute attribute
            Dictionary<string, PropertyInfo> properties = GetArgumentProperties(type);

            foreach (string propertyName in properties.Keys)
            {
                // if the argument dictionary contains a matching argument
                if (arguments.ArgumentDictionary.ContainsKey(propertyName))
                {
                    // retrieve the property and type
                    PropertyInfo property = properties[propertyName];
                    Type propertyType = property.PropertyType;

                    // retrieve the value from the argument dictionary
                    string value = arguments.ArgumentDictionary[propertyName];

                    object convertedValue;

                    // if the type of the property is bool and the argument value is empty set the property value to true,
                    // indicating the argument is present
                    if (propertyType == typeof(bool) && value == string.Empty)
                    {
                        convertedValue = true;
                    }
                    else
                    {
                        // try to cast the argument value string to the destination type
                        try
                        {
                            convertedValue = Convert.ChangeType(value, propertyType);
                        }
                        catch (Exception ex)
                        {
                            // if the cast fails, throw an exception
                            string message = $"Specified value '{value}' for argument '{propertyName}' (type: {property.PropertyType.Name}).  ";
                            message += "See inner exception for details.";

                            throw new ArgumentException(message, ex);
                        }
                    }

                    // set the target properties' value to the converted value from the argument string
                    property.SetValue(null, convertedValue);
                }
            }

            PropertyInfo operandsProperty = GetOperandsProperty(type);

            // check to ensure the target class has a property marked with the Operands attribute; if not GetOperandsProperty()
            // will return null.
            if (operandsProperty != default(PropertyInfo))
            {
                if (operandsProperty.PropertyType.IsAssignableFrom(typeof(List<string>)))
                {
                    operandsProperty.SetValue(null, arguments.OperandList);
                }
                else
                {
                    operandsProperty.SetValue(null, arguments.OperandList.ToArray());
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Populates and returns a dictionary containing the values specified in the command line arguments with which the
        ///     application was started, keyed by argument name.
        /// </summary>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        /// <returns>
        ///     The dictionary containing the arguments and values specified in the command line arguments with which the
        ///     application was started.
        /// </returns>
        private static Dictionary<string, string> GetArgumentDictionary(string commandLineString)
        {
            Dictionary<string, string> argumentDictionary = new Dictionary<string, string>();

            foreach (Match match in Regex.Matches(commandLineString, ArgumentRegEx))
            {
                // the first match of the regular expression used to parse the string will contain the argument name, if one was matched.
                if (match.Groups[1].Value == default(string) || match.Groups[1].Value == string.Empty)
                {
                    continue;
                }

                string fullMatch = match.Groups[0].Value;
                string argument = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                value = TrimOuterQuotes(value);

                // check to see if the argument uses a single dash. if so, split the argument name into a char array and add each
                // to the dictionary. if a value is specified, it belongs to the final character.
                if (Regex.IsMatch(fullMatch, GroupRegEx))
                {
                    char[] charArray = argument.ToCharArray();

                    // iterate over the characters backwards to more easily assign the value
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        argumentDictionary.ExclusiveAdd(charArray[i].ToString(), i == charArray.Length - 1 ? value : string.Empty);
                    }
                }
                else
                {
                    // add the argument and value to the dictionary if it doesn't already exist.
                    argumentDictionary.ExclusiveAdd(argument, value);
                }
            }

            return argumentDictionary;
        }

        /// <summary>
        ///     Retrieves a dictionary containing properties in the target <see cref="Type"/> marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/>, keyed on the string specified in the 'Name' field of the <see cref="Attribute"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> for which the matching properties are to be retrieved.</param>
        /// <returns>
        ///     A dictionary containing matching properties, keyed on the 'Name' field of the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> used to mark the property.
        /// </returns>
        private static Dictionary<string, PropertyInfo> GetArgumentProperties(Type type)
        {
            Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static))
            {
                // attempt to fetch the ArgumentAttribute of the property
                CustomAttributeData attribute = property.CustomAttributes.Where(a => a.AttributeType.Name == typeof(ArgumentAttribute).Name).FirstOrDefault();

                // if found, extract the Name property and add it to the dictionary
                if (attribute != default(CustomAttributeData))
                {
                    char shortName = (char)attribute.ConstructorArguments[0].Value;
                    string longName = (string)attribute.ConstructorArguments[1].Value;

                    if (!properties.ContainsKey(shortName.ToString()) && !properties.ContainsKey(longName))
                    {
                        properties.Add(shortName.ToString(), property);
                        properties.Add(longName, property);
                    }
                }
            }

            return properties;
        }

        /// <summary>
        ///     Populates and returns a list containing the operands specified in the command line arguments with which the
        ///     application was started.
        /// </summary>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        /// <returns>
        ///     A list containing the operands specified in the command line arguments with which the application was started.
        /// </returns>
        private static List<string> GetOperandList(string commandLineString)
        {
            List<string> operands = new List<string>();

            foreach (Match match in Regex.Matches(commandLineString, ArgumentRegEx))
            {
                // the 3rd match of the regular expression used to parse the string will contain the operand, if one was matched.
                if (match.Groups[3].Value == default(string) || match.Groups[3].Value == string.Empty)
                {
                    continue;
                }

                string fullMatch = match.Groups[0].Value;
                string operand = match.Groups[3].Value;

                operands.Add(TrimOuterQuotes(operand));
            }

            return operands;
        }

        /// <summary>
        ///     Populates and returns a list containing the operands within the specified string grouped by whole words and groups
        ///     of words contained within single or double quotes, treating strings that would otherwise be treated as argument
        ///     keys as operands.
        /// </summary>
        /// <param name="operandListString">The string from which the list of operands is to be parsed.</param>
        /// <returns>
        ///     A list containing the operands within the specified string grouped by whole words and groups of words contained
        ///     within single or double quotes, treating strings that would otherwise be treated as argument keys as operands.
        /// </returns>
        private static List<string> GetOperandListStrict(string operandListString)
        {
            List<string> operands = new List<string>();

            foreach (Match match in Regex.Matches(operandListString, OperandRegEx))
            {
                operands.Add(match.Groups[0].Value);
            }

            return operands;
        }

        /// <summary>
        ///     Retrieves the property in the target <see cref="Type"/> marked with the
        ///     <see cref="OperandsAttribute"/><see cref="Attribute"/>, if one exists.
        /// </summary>
        /// <remarks>
        ///     The target property <see cref="Type"/> of the designated property must be of type string[] or List{string}.
        /// </remarks>
        /// <param name="type">The Type for which the matching property is to be retrieved.</param>
        /// <returns>The matching property, if one exists.</returns>
        /// <exception cref="InvalidCastException">
        ///     Thrown when the Type of the retrieved property is not string[] or List{string}.
        /// </exception>
        private static PropertyInfo GetOperandsProperty(Type type)
        {
            PropertyInfo property = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static)
                .Where(p => p.CustomAttributes
                    .Any(a => a.AttributeType.Name == typeof(OperandsAttribute).Name))
                        .FirstOrDefault();

            if (property != default(PropertyInfo) && property.PropertyType != typeof(string[]) && property.PropertyType != typeof(List<string>))
            {
                throw new InvalidCastException("The target for the Operands attribute must be of string[] or List<string>.");
            }

            return property;
        }

        /// <summary>
        ///     Returns the specified string with outer single or double quotes trimmed, if the string starts and ends with them.
        /// </summary>
        /// <param name="value">The string from which to trim outer single or double quotes.</param>
        /// <returns>The string with outer single or double quotes trimmed.</returns>
        private static string TrimOuterQuotes(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Trim('"');
            }
            else if (value.StartsWith("'") && value.EndsWith("'"))
            {
                value = value.Trim('\'');
            }

            return value;
        }

        #endregion Private Methods
    }

    /// <summary>
    ///     Indicates that the property is to be used as the target for automatic population of command line operands when invoking
    ///     the <see cref="Arguments.Populate(string)"/> method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OperandsAttribute : Attribute
    {
    }
}