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
using System.Reflection;
using System.Xml.Serialization;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Methods used to copy/clone an object.
   /// </summary>
   public static class CopyObjects
   {
      #region Copy Properties.
      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="source">The source object. Object to be cloned</param>
      /// <param name="destination">The destination object. Object to be copied into</param>
      public static void CopyPropertiesTo<T> (this T source, T destination) where T : class
      {
         CopyProperties (source, destination, false);
      }

      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="source">The source object. Object to be cloned</param>
      /// <param name="destination">The destination object. Object to be copied into</param>
      /// <param name="includeIgnoredProperties">Include ignore properties.</param>
      public static void CopyPropertiesTo<T> (this T source, T destination, bool includeIgnoredProperties) where T : class
      {
         CopyProperties (source, destination, includeIgnoredProperties);
      }

      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="destination">The destination object. Object to be copied into</param>
      /// <param name="source">The source object. Object to be cloned</param>
      public static void CopyPropertiesFrom<T> (this T destination, T source) where T : class
      {
         CopyProperties (source, destination, false);
      }

      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="destination">The destination object. Object to be copied into</param>
      /// <param name="source">The source object. Object to be cloned</param>
      /// <param name="includeIgnoredProperties">To include ignored properties.</param>
      public static void CopyPropertiesFrom<T> (this T destination, T source, bool includeIgnoredProperties) where T : class
      {
         CopyProperties (source, destination, includeIgnoredProperties);
      }

      /// <summary>
      /// Copies the data from the properties of the source 
      /// object to the properties of the destination object.
      /// Destination must exist.
      /// </summary>
      /// <param name="source">Object from which properties need to read from.</param>
      /// <param name="destination">Object which properties need to be written</param>
      /// <param name="includeIgnoredProperties">True to include ignored properties.</param>
      /// <exception cref="System.ArgumentException">Thrown when the two given objects are not of the same type.</exception>
      /// <exception cref="System.ArgumentNullException">Thrown when the source or destination are null</exception>
      /// <exception cref="System.NotImplementedException">
      /// Arrays and Collections have not been implemented yet.
      /// </exception>
      private static void CopyProperties (object source, object destination, bool includeIgnoredProperties)
      {
         if (source == null)
            throw new ArgumentNullException ("source");

         if (destination == null)
            throw new ArgumentNullException ("destination");

         CopyObject (source, ref destination, includeIgnoredProperties);
      }


      private static void CopyObject (object source, ref object destination, bool includeIgnoredProperties)
      {
         var sourceType = source.GetType ();

         // Value types
         if (sourceType.IsValueType)
         {
            destination = source;
            return;
         }

         // strings
         if (sourceType == typeof (string))
         {
            destination = source;
            return;
         }

         // Arrays
         if (sourceType.IsArray)
         {
            var srcArray = (Array)source;
            var dstArray = (Array)destination;
            if (srcArray.Length != dstArray.Length)
               throw new InvalidOperationException ("Can not copy arrays with different length!");

            for (var i = 0; i < srcArray.Length; i++)
            {
               var srcArrayObject = srcArray.GetValue (i);
               var destArrayObject = dstArray.GetValue (i);
               if (srcArrayObject != null)
               {
                  if (destArrayObject == null)
                     dstArray.SetValue (srcArrayObject.CloneProperties (), i);
                  else
                     CopyObject (srcArrayObject, ref destArrayObject, includeIgnoredProperties);
               }
               else
                  dstArray.SetValue (null, i);
            }
            return;
         }

         // Generics
         if (sourceType.IsGenericClass ())
         {
            // get the object and copy/clone it and then invoke the Add method with the copied/cloned object as argument.
            var countProperty = sourceType.GetProperty ("Count");
            var itemProperty = sourceType.GetProperty ("Item");
            var addMethod = sourceType.GetMethod ("Add");
            var removeMethod = sourceType.GetMethod ("RemoveAt");
            if (addMethod != null) // no read only collection
            {
               // verify collection length
               var srcLength = (int)countProperty.GetValue (source, null);
               var dstLength = (int)countProperty.GetValue (destination, null);
               if (dstLength > srcLength)
               {
                  if (removeMethod == null)
                     throw new ArgumentException ("Source size should be equal or greater than the size of destination");

                  for (var i = dstLength - 1; i >= srcLength; i--)
                     removeMethod.Invoke (destination, new object[] { i });
               }

               for (var i = 0; i < srcLength; i++)
               {
                  var sourcePropertyValue = itemProperty.GetValue (source, new object[] { i });
                  if (i < dstLength)
                  {
                     if (sourcePropertyValue != null)
                     {
                        var destinationPropertyValue = itemProperty.GetValue (destination, new object[] { i });
                        if (destinationPropertyValue != null)
                           CopyObject (sourcePropertyValue, ref destinationPropertyValue, includeIgnoredProperties);
                        else
                           destinationPropertyValue = sourcePropertyValue.CloneProperties (includeIgnoredProperties);
                        itemProperty.SetValue (destination, destinationPropertyValue, new object[] { i });
                     }
                     else
                        itemProperty.SetValue (destination, null, new object[] { i });
                  }
                  else
                  {
                     object destinationPropertyValue = null;
                     if (sourcePropertyValue != null)
                        destinationPropertyValue = sourcePropertyValue.CloneProperties (includeIgnoredProperties);
                     addMethod.Invoke (destination, new[] { destinationPropertyValue });
                  }
               }
            }
            return;
         }

         // Class
         if (sourceType.IsClass)
         {
            var properties = sourceType.GetProperties (BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
               if (!includeIgnoredProperties)
               {
                  var ignore = property.GetCustomAttributes (typeof (XmlIgnoreAttribute), false);
                  if (ignore.Length > 0)
                     continue;
               }

               if (property.CanRead)
               {
                  object srcPropertyValue = null;
                  var isItemString = false;
                  try
                  {
                     srcPropertyValue = property.GetValue (source, null);
                  }
                  catch (TargetParameterCountException)
                  {
                     isItemString = true;
                  }
                  if (isItemString)
                     continue;

                  var dstPropertyValue = property.GetValue (destination, null);
                  CopyProperty (destination, property, srcPropertyValue, dstPropertyValue, includeIgnoredProperties);
               }
            }
         }
      }

      private static void CopyProperty (object parentDestination, PropertyInfo property, object source, object destination, bool includeIgnoredProperties)
      {
         var propertyType = property.PropertyType;

         // Value Type
         if (propertyType.IsValueType)
         {
            if (property.IsPropertyWritable ())
               property.SetValue (parentDestination, source, null);
            return;
         }

         // String
         if (propertyType == typeof (string))
         {
            if (property.IsPropertyWritable ())
               property.SetValue (parentDestination, source, null);
            return;
         }

         // Array
         if (propertyType.IsArray)
         {
            if (source == null)
               property.SetValue (parentDestination, null, null);
            else
            {
               if (destination == null)
                  property.SetValue (parentDestination, source.CloneProperties (includeIgnoredProperties), null);
               else
                  CopyObject (source, ref destination, includeIgnoredProperties);
            }
            return;
         }

         // Class
         if (propertyType.IsClass)
         {
            if (source == null)
            {
               if (property.IsPropertyWritable ())
                  property.SetValue (parentDestination, null, null);
            }
            else
            {
               if (destination == null)
               {
                  if (property.IsPropertyWritable ())
                     property.SetValue (parentDestination, source.CloneProperties (includeIgnoredProperties), null);
               }
               else
               {
                  CopyObject (source, ref destination, includeIgnoredProperties);
                  if (property.IsPropertyWritable ())
                     property.SetValue (parentDestination, destination, null);
               }
            }
         }
      }
      #endregion

      #region Clone Properties.
      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="obj">The source object. Object to be cloned</param>
      /// <returns>A new object of the same type of the source object and with equal public properties values</returns>
      public static T CloneProperties<T> (this T obj) where T : class
      {
         if (obj == null)
            return null;
         return (T)CloneObject (obj, null, false);
      }

      /// <summary>
      /// Clone all the properties of an object.
      /// </summary>
      /// <typeparam name="T">Type of the source object</typeparam>
      /// <param name="obj">The source object. Object to be cloned</param>
      /// <param name="includeIgnoredProperties">Include ignored properties.</param>
      /// <returns>A new object of the same type of the source object and with equal public properties values</returns>
      public static T CloneProperties<T> (this T obj, bool includeIgnoredProperties) where T : class
      {
         if (obj == null)
            return null;
         return (T)CloneObject (obj, null, includeIgnoredProperties);
      }

      /// <summary>
      /// Recursive method to navigate and clone all the properties of a given object
      /// </summary>
      /// <param name="obj">Object to clone</param>
      /// <param name="readOnlyObject">Only used with Collections. Destination when the original collection is read only.</param>
      /// <param name="includeIgnoredProperties">Includes ignored properties.</param>
      /// <returns>The cloned object</returns>
      private static object CloneObject (object obj, object readOnlyObject, bool includeIgnoredProperties)
      {
         if (obj == null)
            return null;

         var type = obj.GetType ();
         if (type.IsValueType || type == typeof (string))
         {
            return obj;
         }
         if (type.IsArray)
         {
            if (type.FullName != null)
            {
               var elementType = Type.GetType (type.FullName.Replace ("[]", string.Empty) + ", " + type.Assembly.FullName, true);
               var array = obj as Array;
               if (array != null)
               {
                  var copied = Array.CreateInstance (elementType, array.Length);
                  for (var i = 0; i < array.Length; i++)
                     copied.SetValue (CloneObject (array.GetValue (i), null, includeIgnoredProperties), i);
                  return Convert.ChangeType (copied, obj.GetType (), CultureInfo.InvariantCulture);
               }
            }
         }
         if (type.IsGenericClass ())
         {
            var toret = readOnlyObject ?? Activator.CreateInstance (type);
            toret.CopyPropertiesFrom (obj, includeIgnoredProperties);
            return toret;
         }
         if (type.IsClass)
         {
            var toret = Activator.CreateInstance (obj.GetType ());
            toret.CopyPropertiesFrom (obj, includeIgnoredProperties);
            return toret;
         }
         throw new ArgumentException ("Unknown type");
      }
      #endregion
      /// <summary>
      ///  Helper static method to detect if a class is generic or not.
      /// </summary>
      /// <param name="type">Type to be checked.</param>
      /// <returns>True if generic.</returns>
      public static bool IsGenericClass (this Type type)
      {
         var isGeneric = false;
         while (type != null)
         {
            if (type.IsGenericType)
            {
               isGeneric = true;
               break;
            }
            type = type.BaseType;
         }
         return isGeneric;
      }

      /// <summary>
      /// Gets the generic base type.
      /// </summary>
      /// <param name="type">Type from whre to find the generic base type.</param>
      /// <returns>The generic base type.</returns>
      public static Type GetGenericBaseType (this Type type)
      {
         Type genericBaseType = null;
         while (type != null)
         {
            if (type.IsGenericType)
            {
               var dataTypes = type.GetGenericArguments ();
               genericBaseType = dataTypes[0];
               break;
            }
            type = type.BaseType;
         }
         return genericBaseType;
      }

      /// <summary>
      /// Finds if the property has a public Set.
      /// </summary>
      /// <param name="property">The property</param>
      /// <returns>True if the properti has a public set.</returns>
      public static bool IsPropertyWritable (this PropertyInfo property)
      {
         var ok = property.CanWrite;
         if (ok)
            ok = (property.GetSetMethod () != null);
         return ok;
      }
   }
}
