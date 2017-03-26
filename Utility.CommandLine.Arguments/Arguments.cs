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
      █  Provides static methods used to retrieve the command line arguments with which the application was started.
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
    ///     Provides static methods used to retrieve the command line arguments with which the application was started.
    /// </summary>
    public static class Arguments
    {
        #region Private Fields

        /// <summary>
        ///     The regular expression with which to parse the command line string.
        /// </summary>
        private const string ArgumentRegEx = "(?:[-]{1,2}|\\/)([\\w-]+)[=|:| ]?(\\w\\S*|\\\".*\\\"|\\\'.*\\\')?";

        private const string GroupRegEx = "^-[^-]+";

        #endregion Private Fields

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
        public static Dictionary<string, string> Parse(string commandLineString = "")
        {
            commandLineString = commandLineString.Equals(string.Empty) ? Environment.CommandLine : commandLineString;

            return GetArgumentDictionary(commandLineString);
        }

        /// <summary>
        ///     Populates the properties in the invoking class marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> and sets the value to the corresponding value specified in
        ///     the list of command line arguments, if present.
        /// </summary>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        public static void Populate(string commandLineString = "")
        {
            commandLineString = commandLineString.Equals(string.Empty) ? Environment.CommandLine : commandLineString;

            Populate(new StackFrame(1).GetMethod().DeclaringType, commandLineString);
        }

        /// <summary>
        ///     Populates the properties in the invoking class marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> and sets the value to the corresponding value specified in
        ///     the list of command line arguments, if present.
        /// </summary>
        /// <param name="type">
        ///     The Type for which the static properties matching the list of command line arguments are to be populated.
        /// </param>
        public static void Populate(Type type)
        {
            Populate(type, Environment.CommandLine);
        }

        /// <summary>
        ///     Populates the properties in the invoking class marked with the
        ///     <see cref="ArgumentAttribute"/><see cref="Attribute"/> and sets the value to the corresponding value specified in
        ///     the list of command line arguments, if present.
        /// </summary>
        /// <param name="type">
        ///     The Type for which the static properties matching the list of command line arguments are to be populated.
        /// </param>
        /// <param name="commandLineString">The command line arguments with which the application was started.</param>
        public static void Populate(Type type, string commandLineString)
        {
            // fetch any properties in the specified type marked with the ArgumentAttribute attribute
            Dictionary<string, PropertyInfo> properties = GetArgumentProperties(type);

            // retrieve the command line parameters
            Dictionary<string, string> argumentDictionary = Parse(commandLineString);

            // iterate over the property dictionary
            foreach (string propertyName in properties.Keys)
            {
                // if the argument dictionary contains a matching argument
                if (argumentDictionary.ContainsKey(propertyName))
                {
                    // retrieve the property and type
                    PropertyInfo property = properties[propertyName];
                    Type propertyType = property.PropertyType;

                    // retrieve the value from the argument dictionary
                    string value = argumentDictionary[propertyName];

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

            // iterate over the collection of matches to the parsing regular expression
            foreach (Match match in Regex.Matches(commandLineString, ArgumentRegEx))
            {
                // ensure the match contains three groups; the key/value pair (0), the key (1), and the value (2)
                if (match.Groups.Count == 3)
                {
                    string fullMatch = match.Groups[0].Value;
                    string argument = match.Groups[1].Value;
                    string value = match.Groups[2].Value.Trim('"', '\'');

                    // check to see if the argument uses a single dash. if so, split the argument name into a char array and add
                    // each to the dictionary. if a value is specified, it belongs to the final character.
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
            }

            return argumentDictionary;
        }

        private static void ExclusiveAdd(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
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

            // iterate over the private static properties in the specified type
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

        #endregion Private Methods
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
        /// <param name="name">The name of the argument, as appears in the list of the command line arguments.</param>
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
}