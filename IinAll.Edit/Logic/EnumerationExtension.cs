using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace IinAll.Edit.Logic
{
   public class EnumerationExtension : MarkupExtension
   {
      private Type m_EnumType;

      public EnumerationExtension (Type enumType)
      {
         if (enumType == null)
            throw new ArgumentNullException ("enumType");

         EnumType = enumType;
      }

      public Type EnumType
      {
         get { return m_EnumType; }
         private set
         {
            if (m_EnumType == value)
               return;

            var enumType = Nullable.GetUnderlyingType (value) ?? value;

            if (enumType.IsEnum == false)
               throw new ArgumentException ("Type must be an Enum.");

            m_EnumType = value;
         }
      }

      public override object ProvideValue (IServiceProvider serviceProvider)
      {
         var enumValues = Enum.GetValues (EnumType);

         return (from object enumValue in enumValues
                 select new EnumerationMember
                 {
                    Value = (int)enumValue,
                    Description = GetDescription (enumValue)
                 }).ToArray ();
      }

      private string GetDescription (object enumValue)
      {
         var descriptionAttribute = EnumType
            .GetField (enumValue.ToString ())
            .GetCustomAttributes (typeof (DescriptionAttribute), false)
            .FirstOrDefault () as DescriptionAttribute;


         return descriptionAttribute != null
            ? descriptionAttribute.Description
            : enumValue.ToString ().Replace ("_", " ");
      }

      public class EnumerationMember
      {
         public string Description { get; set; }
         public int Value { get; set; }
      }
   }
}