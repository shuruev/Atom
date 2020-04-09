using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Atom.Util
{
    /// <summary>
    /// Lightweight wrapper over XDocument with easy-to-use methods for working with namespaces, element lookups, to- and from- XML conversions, etc.
    /// </summary>
    public class XDoc : XDocument
    {
        private readonly XmlNamespaceManager _ns;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        private XDoc(XDocument doc)
            : base(doc)
        {
            _ns = new XmlNamespaceManager(new NameTable());
        }

        /// <summary>
        /// Initializes XML document from specified string.
        /// </summary>
        public static XDoc FromXml(string xml)
        {
            var doc = Parse(xml, LoadOptions.PreserveWhitespace);
            return new XDoc(doc);
        }

        /// <summary>
        /// Creates new XML document with specified declaration.
        /// </summary>
        public static XDoc New(XDocDeclaration declaration = XDocDeclaration.None)
        {
            XDeclaration xd;
            switch (declaration)
            {
                case XDocDeclaration.None:
                    xd = null;
                    break;
                case XDocDeclaration.UTF8:
                    xd = new XDeclaration("1.0", "utf-8", null);
                    break;
                case XDocDeclaration.UTF16:
                    xd = new XDeclaration("1.0", "utf-16", null);
                    break;
                case XDocDeclaration.UTF32:
                    xd = new XDeclaration("1.0", "utf-32", null);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown declaration type '{declaration}'");
            }

            var doc = new XDocument(xd);
            return new XDoc(doc);
        }

        /// <summary>
        /// Converts XML document to its string representation.
        /// </summary>
        public string ToXml(XDocFormatting formatting = XDocFormatting.None)
        {
            // preserving original encoding if was specified, or use UTF-8 by default
            var encoding = GetEncoding();

            var doc = (XDocument)this;
            var settings = new XmlWriterSettings
            {
                Encoding = encoding,
                OmitXmlDeclaration = Declaration == null
            };

            if (formatting != XDocFormatting.None)
            {
                var xml = ToXml();
                doc = Parse(xml);
            }

            if (formatting == XDocFormatting.Indented)
            {
                settings.Indent = true;
            }

            using (var ms = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(ms, settings))
                {
                    doc.WriteTo(xw);
                }

                return encoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Recognizes XML encoding from declaration, or uses UTF-8 by default.
        /// </summary>
        private Encoding GetEncoding()
        {
            switch (Declaration?.Encoding)
            {
                case "utf-16": return new UnicodeEncoding(false, false);
                case "utf-32": return new UTF32Encoding(false, false);
                default: return new UTF8Encoding(false);
            }
        }

        /// <summary>
        /// Adds new XML namespace to be used with this document.
        /// </summary>
        public XDoc AddNamespace(string prefix, XNamespace ns)
        {
            _ns.AddNamespace(prefix, ns.NamespaceName);
            return this;
        }

        /// <summary>
        /// Builds new name using specified namespace.
        /// </summary>
        public XName Name(string prefix, string name)
        {
            XNamespace ns = _ns.LookupNamespace(prefix);
            return ns + name;
        }

        /// <summary>
        /// Executes XPath query using underlying namespace manager.
        /// </summary>
        public XElement Get(string xpath)
        {
            return this.XPathSelectElement(xpath, _ns);
        }

        /// <summary>
        /// Executes XPath query using underlying namespace manager.
        /// </summary>
        public IEnumerable<XElement> Select(string xpath)
        {
            return this.XPathSelectElements(xpath, _ns);
        }
    }

    /// <summary>
    /// Defines how result XML should be formatted.
    /// </summary>
    public enum XDocFormatting
    {
        /// <summary>
        /// No special formatting applied.
        /// </summary>
        None,

        /// <summary>
        /// XML will be auto-formatted and indented.
        /// </summary>
        Indented,

        /// <summary>
        /// All extra whitespaces will be removed.
        /// </summary>
        Minified,
    }

    /// <summary>
    /// Defines possible XML declaration to use.
    /// </summary>
    public enum XDocDeclaration
    {
        /// <summary>
        /// Do not use XML declaration.
        /// </summary>
        None,

        /// <summary>
        /// Use declaration with UTF-8 encoding.
        /// </summary>
        UTF8,

        /// <summary>
        /// Use declaration with UTF-16 encoding.
        /// </summary>
        UTF16,

        /// <summary>
        /// Use declaration with UTF-32 encoding.
        /// </summary>
        UTF32
    }
}
