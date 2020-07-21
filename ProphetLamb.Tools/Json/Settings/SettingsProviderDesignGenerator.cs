using System;
using System.Reflection;
using System.Text;

namespace ProphetLamb.Tools.Json.Settings
{
    public class SettingsProviderDesignGenerator
    {
    }

    internal class SettingsProviderDesignGeneratorHelper
    {
        private readonly string _newLine,
                                _namespace,
                                _accessibility,
                                _className,
                                _pogoName,
                                _settingsProviderClassNamespace = "ProphetLamb.Tools.Json.Settings.SettingsProvider",
                                _settingsProviderName = "settingsProvider",
                                _fileName;
        private int _indentation;
        private string _indent = String.Empty;

        public static void GenerateDesignerSource<T_POGO>(ISettingsProvider<T_POGO>? provider, StringBuilder? syntaxBuilder)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));
            if (syntaxBuilder is null)
                throw new ArgumentNullException(nameof(syntaxBuilder));
            Type pogoType = typeof(T_POGO);
            var helper = new SettingsProviderDesignGeneratorHelper(pogoType, provider.FileName, false);
            helper.GenerateHeading(syntaxBuilder);
            foreach(var node in pogoType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (node.PropertyType.IsValueType)
                    helper.GenerateValueTypeNode(syntaxBuilder, node);
                else
                    helper.GenerateNode(syntaxBuilder, node);
            }
            helper.GenerateFooter(syntaxBuilder);
        }

        private SettingsProviderDesignGeneratorHelper(Type? pogoType, string? fileName, bool publicAccessibility)
        {
            if (pogoType is null)
                throw new ArgumentNullException(nameof(pogoType));
            _newLine = Environment.NewLine;
            _namespace = pogoType.Namespace??throw new ArgumentException("Namespace cannot be null.", nameof(pogoType));
            _accessibility = publicAccessibility ? "public" : "internal";
            _className = "SettingsProvider_" + pogoType.Name + "_Designer";
            _pogoName = pogoType.Name;
            _fileName = fileName??throw new ArgumentNullException("Filename cannot be null.", nameof(fileName));
        }

        private int Indentation
        {
            get => _indentation;
            set
            {
                _indentation = value;
                _indent = value == 0 ? String.Empty : new String(' ', 4 * value);
            }
        }

        private void GenerateHeading(StringBuilder syntaxBuilder)
        {
            // using System;
            //
            // namespace [NAMESPACE]
            // {
            syntaxBuilder
                .Append("using System;").Append(_newLine)
                .Append(_newLine)
                .Append(_indent).Append("namespace ").Append(_namespace).Append(_newLine)
                .Append(_indent).Append('{').Append(_newLine);
            Indentation++;
            //     public|internal class SettingsProvider_[POGONAME]_Designer
            //     {
            syntaxBuilder
                .Append(_indent).Append(_accessibility).Append(" class ").Append(_className).Append(_newLine)
                .Append(_indent).Append('{').Append(_newLine);
            Indentation++;
            //         private ProphetLamb.Tools.Json.Settings.SettingsProvider<[POGONAME]> settingsProvider;
            //
            //         public SettingsProvider_[POGONAME]_Designer()
            //         {
            syntaxBuilder
                .Append(_indent).Append("private ").Append(_settingsProviderClassNamespace).Append('<').Append(_pogoName).Append("> ").Append(_settingsProviderName).Append(';').Append(_newLine)
                .Append(_newLine)
                .Append(_indent).Append("public ").Append(_className).Append("()").Append(_newLine)
                .Append(_indent).Append('{').Append(_newLine);
            Indentation++;
            //             settingsProvider = ProphetLamb.Tools.Json.Settings.SettingsProvider<[POGONAME]>.Create(@"[FILENAME]");
            syntaxBuilder
                .Append(_indent).Append(_settingsProviderName).Append(" = ").Append(_settingsProviderClassNamespace).Append('<').Append(_pogoName).Append('>').Append(".Create(@\"").Append(_fileName).Append("\");").Append(_newLine);
            Indentation--;
            //         }
            syntaxBuilder
                .Append(_indent).Append('}').Append(_newLine);
        }

        private void GenerateFooter(StringBuilder syntaxBuilder)
        {
            Indentation--;
                // }
            syntaxBuilder
                .Append(_indent).Append('}').Append(_newLine);
            Indentation--;
            // }
            syntaxBuilder
                .Append(_indent).Append('}').Append(_newLine);
        }

        private void GenerateValueTypeNode(StringBuilder syntaxBuilder, PropertyInfo nodePropertyInfo)
        {
            Type type = nodePropertyInfo.PropertyType;
            string typeName = type.FullName ?? throw new InvalidOperationException("the fullname of the type is undefinded."),
                   name = nodePropertyInfo.Name;
            syntaxBuilder.Append(_newLine);
            // public [TYPE] [NAME]
            // {
            syntaxBuilder
                .Append(_indent).Append("public ").Append(typeName).Append(" ").Append(name).Append(_newLine)
                .Append(_indent).Append('{').Append(_newLine);
            Indentation++;
            //     get => settingsProvider.GetValue<[TYPE]>(@"[NAME]");
            syntaxBuilder
                .Append(_indent).Append("get => ").Append(_settingsProviderName).Append(".GetValue<").Append(typeName).Append(">(@\"").Append(name).Append("\");").Append(_newLine);
            if (nodePropertyInfo.CanWrite)
            {
                //     set => settingsProvider.SetValue(@"[NAME]", value);
                syntaxBuilder
                    .Append(_indent).Append("set => ").Append(_settingsProviderName).Append(".SetValue(@\"").Append(name).Append("\", value);").Append(_newLine);
            }
            Indentation--;
            // }
            syntaxBuilder
                .Append(_indent).Append('}').Append(_newLine);
        }

        private void GenerateNode(StringBuilder syntaxBuilder, PropertyInfo nodePropertyInfo)
        {
            Type type = nodePropertyInfo.PropertyType;
            string typeName = type.FullName ?? throw new InvalidOperationException("the fullname of the type is undefinded."),
                   name = nodePropertyInfo.Name;
            syntaxBuilder.Append(_newLine);
            // public object? [NAME]
            // {
            syntaxBuilder
                .Append(_indent).Append("public ").Append(typeName).Append(" ").Append(name).Append(_newLine)
                .Append(_indent).Append('{').Append(_newLine);
            Indentation++;
            //     get => settingsProvider.GetValue(@"[NAME]");
            syntaxBuilder
                .Append(_indent).Append("get => ").Append(_settingsProviderName).Append(".GetValue(@\"").Append(name).Append("\");").Append(_newLine);
            if (nodePropertyInfo.CanWrite)
            {
                //     set => settingsProvider.SetValue(@"[NAME]", value);
                syntaxBuilder
                    .Append(_indent).Append("set => ").Append(_settingsProviderName).Append(".SetValue(@\"").Append(name).Append("\", value);").Append(_newLine);
            }
            Indentation--;
            // }
            syntaxBuilder
                .Append(_indent).Append('}').Append(_newLine);
        }
    }
}