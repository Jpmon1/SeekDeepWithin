/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Serialization helper methods
   /// </summary>
   public static class Serialization
   {
      /// <summary>
      /// XML serializes an object into a file. Defaults to utf-16 encoding.
      /// </summary>
      /// <param name="item">Object to be serialized.</param>
      /// <param name="fileName">File name where to output serialization.</param>
      public static void Serialize<T> (this T item, string fileName) where T : class
      {
         item.Serialize (fileName, Encoding.Unicode);
      }

      /// <summary>
      /// XML serializes an object into a file.
      /// </summary>
      /// <param name="item">Object to be serialized.</param>
      /// <param name="fileName">File name where to output serialization.</param>
      /// <param name="encoding">Text file encoding.</param>
      public static void Serialize<T> (this T item, string fileName, Encoding encoding) where T : class
      {
         using (var fileWriter = new StreamWriter (fileName, false, encoding))
            item.Serialize (fileWriter);
      }

      /// <summary>
      /// Serializes the given item to XML.
      /// </summary>
      /// <typeparam name="T">Type of item to serialize.</typeparam>
      /// <param name="item">Item to serialize.</param>
      /// <param name="textWriter">TextWriter to write XML to.</param>
      private static void Serialize<T> (this T item, TextWriter textWriter) where T : class
      {
         using (var writer = new XmlNoStartDocWriter (textWriter))
         {
            var type = item.GetType ();
            var nameSpaces = new XmlSerializerNamespaces ();
            nameSpaces.Add ("", "");
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;
            var serializer = new XmlSerializer (type);
            serializer.Serialize (writer, item, nameSpaces);
         }
      }

      /// <summary>
      /// XML serializes an object into a string.
      /// </summary>
      /// <param name="item">Object to be serialized.</param>
      /// <returns>The XML representation of the objcet</returns>
      public static string Serialize<T> (this T item) where T : class
      {
         string xmlString;
         using (var stringWriter = new StringWriter (CultureInfo.InvariantCulture))
         {
            item.Serialize (stringWriter);
            xmlString = stringWriter.ToString ();
         }
         return xmlString;
      }

      /// <summary>
      /// Deserialized the file into an object.
      /// </summary>
      /// <param name="xmlString">File name.</param>
      /// <typeparam name="T">Type to be deserialized.</typeparam>
      /// <returns>Deserialized object, new object if no file.</returns>
      public static T DeserializeFromString <T> (string xmlString) where T : class
      {
         if (!string.IsNullOrEmpty (xmlString))
         {
            object item;
            using (var reader = new StringReader (xmlString))
            {
               var serializer = new XmlSerializer (typeof (T));
               item = serializer.Deserialize (reader);
            }
            return (T)item;
         }
         return (T)Activator.CreateInstance (typeof (T));
      }

      /// <summary>
      /// Deserialized the file into an object.
      /// </summary>
      /// <param name="fileName">File name.</param>
      /// <typeparam name="T">Type to be deserialized.</typeparam>
      /// <returns>Deserialized object, new object if no file.</returns>
      public static T Deserialize <T> (string fileName) where T : class
      {
         if (File.Exists (fileName))
         {
            object item;
            using (var reader = new StreamReader (fileName))
            {
               var serializer = new XmlSerializer (typeof (T));
               item = serializer.Deserialize (reader);
            }
            return (T) item;
         }
         return (T)Activator.CreateInstance (typeof (T));
      }


      /// <summary>
      /// Class produces serialization to exclude header and formatting.
      /// </summary>
      private class XmlNoStartDocWriter : XmlTextWriter
      {
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="w"></param>
         public XmlNoStartDocWriter (TextWriter w) : base (w) { }

         /// <summary>
         /// Suppress the writing of header data
         /// </summary>
         public override void WriteStartDocument () { }
      }
   }
}
