using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SeekDeepWithin.Controllers
{
   public class Hashids
   {
      public const string DEFAULT_ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
      public const string DEFAULT_SEPS = "cfhistuCFHISTU";

      private const double SEP_DIV = 3.5;
      private const double GUARD_DIV = 12.0;

      private string m_Alphabet;
      private readonly string m_Salt;
      private string m_Seps;
      private string m_Guards;
      private readonly int m_MinHashLength;

      private Regex m_GuardsRegex;
      private Regex m_SepsRegex;
      private static readonly Regex s_HexValidator = new Regex ("^[0-9a-fA-F]+$", RegexOptions.Compiled);
      private static readonly Regex s_HexSplitter = new Regex (@"[\w\W]{1,12}", RegexOptions.Compiled);

      /// <summary>
      /// Instantiates a new Hashids with the default setup.
      /// </summary>
      public Hashids () : this (string.Empty) { }

      /// <summary>
      /// Instantiates a new Hashids en/de-coder.
      /// </summary>
      /// <param name="salt"></param>
      /// <param name="minHashLength"></param>
      /// <param name="alphabet"></param>
      /// <param name="seps"></param>
      public Hashids (string salt = "", int minHashLength = 0, string alphabet = DEFAULT_ALPHABET, string seps = DEFAULT_SEPS)
      {
         if (string.IsNullOrWhiteSpace (alphabet))
            throw new ArgumentNullException ("alphabet");

         this.m_Salt = salt;
         this.m_Alphabet = string.Join (string.Empty, alphabet.Distinct ());
         this.m_Seps = seps;
         this.m_MinHashLength = minHashLength;

         if (this.m_Alphabet.Length < 16)
            throw new ArgumentException ("alphabet must contain atleast 4 unique characters.", "alphabet");

         this.SetupSeps ();
         this.SetupGuards ();
      }

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers to encode</param>
      /// <returns>the hashed string</returns>
      public virtual string Encode (params int [] numbers)
      {
         return this.GenerateHashFrom (numbers.Select (n => (long) n).ToArray ());
      }

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers to encode</param>
      /// <returns>the hashed string</returns>
      public virtual string Encode (IEnumerable<int> numbers)
      {
         return this.Encode (numbers.ToArray ());
      }

      /// <summary>
      /// Decodes the provided hash into
      /// </summary>
      /// <param name="hash">the hash</param>
      /// <exception cref="T:System.OverflowException">if the decoded number overflows integer</exception>
      /// <returns>the numbers</returns>
      public virtual int [] Decode (string hash)
      {
         return this.GetNumbersFrom (hash).Select (n => (int) n).ToArray ();
      }

      /// <summary>
      /// Encodes the provided hex string to a hashids hash.
      /// </summary>
      /// <param name="hex"></param>
      /// <returns></returns>
      public virtual string EncodeHex (string hex)
      {
         if (!s_HexValidator.IsMatch (hex))
            return string.Empty;

         var numbers = new List<long> ();
         var matches = s_HexSplitter.Matches (hex);

         foreach (Match match in matches) {
            var number = Convert.ToInt64 (string.Concat ("1", match.Value), 16);
            numbers.Add (number);
         }

         return this.EncodeLong (numbers.ToArray ());
      }

      /// <summary>
      /// Decodes the provided hash into a hex-string
      /// </summary>
      /// <param name="hash"></param>
      /// <returns></returns>
      public virtual string DecodeHex (string hash)
      {
         var ret = new StringBuilder ();
         var numbers = this.DecodeLong (hash);

         foreach (var number in numbers)
            ret.Append (string.Format ("{0:X}", number).Substring (1));

         return ret.ToString ();
      }

      /// <summary>
      /// Decodes the provided hashed string into an array of longs 
      /// </summary>
      /// <param name="hash">the hashed string</param>
      /// <returns>the numbers</returns>
      public long [] DecodeLong (string hash)
      {
         return this.GetNumbersFrom (hash);
      }

      /// <summary>
      /// Encodes the provided longs to a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      public string EncodeLong (params long [] numbers)
      {
         return this.GenerateHashFrom (numbers);
      }

      /// <summary>
      /// Encodes the provided longs to a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      public string EncodeLong (IEnumerable<long> numbers)
      {
         return this.EncodeLong (numbers.ToArray ());
      }

      /// <summary>
      /// Encodes the provided numbers into a string.
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hash</returns>
      [Obsolete ("Use 'Encode' instead. The method was renamed to better explain what it actually does.")]
      public virtual string Encrypt (params int [] numbers)
      {
         return Encode (numbers);
      }

      /// <summary>
      /// Encrypts the provided hex string to a hashids hash.
      /// </summary>
      /// <param name="hex"></param>
      /// <returns></returns>
      [Obsolete ("Use 'EncodeHex' instead. The method was renamed to better explain what it actually does.")]
      public virtual string EncryptHex (string hex)
      {
         return EncodeHex (hex);
      }

      /// <summary>
      /// Decodes the provided numbers into a array of numbers
      /// </summary>
      /// <param name="hash">hash</param>
      /// <returns>array of numbers.</returns>
      [Obsolete ("Use 'Decode' instead. Method was renamed to better explain what it actually does.")]
      public virtual int [] Decrypt (string hash)
      {
         return Decode (hash);
      }

      /// <summary>
      /// Decodes the provided hash to a hex-string
      /// </summary>
      /// <param name="hash"></param>
      /// <returns></returns>
      [Obsolete ("Use 'DecodeHex' instead. The method was renamed to better explain what it actually does.")]
      public virtual string DecryptHex (string hash)
      {
         return DecodeHex (hash);
      }

      /// <summary>
      /// 
      /// </summary>
      private void SetupSeps ()
      {
         // seps should contain only characters present in alphabet; 
         m_Seps = new String (m_Seps.Intersect (m_Alphabet.ToArray ()).ToArray ());

         // alphabet should not contain seps.
         m_Alphabet = new String (m_Alphabet.Except (m_Seps.ToArray ()).ToArray ());

         m_Seps = ConsistentShuffle (m_Seps, m_Salt);

         if (m_Seps.Length == 0 || (m_Alphabet.Length / m_Seps.Length) > SEP_DIV) {
            var sepsLength = (int) Math.Ceiling (m_Alphabet.Length / SEP_DIV);
            if (sepsLength == 1)
               sepsLength = 2;

            if (sepsLength > m_Seps.Length) {
               var diff = sepsLength - m_Seps.Length;
               m_Seps += m_Alphabet.Substring (0, diff);
               m_Alphabet = m_Alphabet.Substring (diff);
            } else m_Seps = m_Seps.Substring (0, sepsLength);
         }

         m_SepsRegex = new Regex (string.Concat ("[", m_Seps, "]"), RegexOptions.Compiled);
         m_Alphabet = ConsistentShuffle (m_Alphabet, m_Salt);
      }

      /// <summary>
      /// 
      /// </summary>
      private void SetupGuards ()
      {
         var guardCount = (int) Math.Ceiling (m_Alphabet.Length / GUARD_DIV);

         if (m_Alphabet.Length < 3) {
            m_Guards = m_Seps.Substring (0, guardCount);
            m_Seps = m_Seps.Substring (guardCount);
         } else {
            m_Guards = m_Alphabet.Substring (0, guardCount);
            m_Alphabet = m_Alphabet.Substring (guardCount);
         }

         m_GuardsRegex = new Regex (string.Concat ("[", m_Guards, "]"), RegexOptions.Compiled);
      }

      /// <summary>
      /// Internal function that does the work of creating the hash
      /// </summary>
      /// <param name="numbers"></param>
      /// <returns></returns>
      private string GenerateHashFrom (long [] numbers)
      {
         if (numbers == null || numbers.Length == 0)
            return string.Empty;

         if (this.Order)
            numbers = numbers.OrderBy (n => n).ToArray();
         var ret = new StringBuilder ();
         var alphabet = this.m_Alphabet;

         long numbersHashInt = 0;
         for (var i = 0; i < numbers.Length; i++)
            numbersHashInt += (int) (numbers [i] % (i + 100));

         var lottery = alphabet [(int) (numbersHashInt % alphabet.Length)];
         ret.Append (lottery.ToString (CultureInfo.InvariantCulture));

         for (var i = 0; i < numbers.Length; i++) {
            var number = numbers [i];
            var buffer = lottery + this.m_Salt + alphabet;

            alphabet = ConsistentShuffle (alphabet, buffer.Substring (0, alphabet.Length));
            var last = Hash (number, alphabet);

            ret.Append (last);

            if (i + 1 < numbers.Length) {
               number %= (last [0] + i);
               var sepsIndex = ((int) number % this.m_Seps.Length);

               ret.Append (this.m_Seps [sepsIndex]);
            }
         }

         if (ret.Length < this.m_MinHashLength) {
            var guardIndex = ((int) (numbersHashInt + ret [0]) % this.m_Guards.Length);
            var guard = this.m_Guards [guardIndex];

            ret.Insert (0, guard);

            if (ret.Length < this.m_MinHashLength) {
               guardIndex = ((int) (numbersHashInt + ret [2]) % this.m_Guards.Length);
               guard = this.m_Guards [guardIndex];

               ret.Append (guard);
            }
         }

         var halfLength = alphabet.Length / 2;
         while (ret.Length < this.m_MinHashLength) {
            alphabet = ConsistentShuffle (alphabet, alphabet);
            ret.Insert (0, alphabet.Substring (halfLength));
            ret.Append (alphabet.Substring (0, halfLength));

            var excess = ret.Length - this.m_MinHashLength;
            if (excess > 0) {
               ret.Remove (0, excess / 2);
               ret.Remove (this.m_MinHashLength, ret.Length - this.m_MinHashLength);
            }
         }

         return ret.ToString ();
      }

      public bool Order { get; set; }

      private static string Hash (long input, string alphabet)
      {
         var hash = new StringBuilder ();

         do {
            hash.Insert (0, alphabet [(int) (input % alphabet.Length)]);
            input = (input / alphabet.Length);
         } while (input > 0);

         return hash.ToString ();
      }

      private static long Unhash (string input, string alphabet)
      {
         long number = 0;

         for (var i = 0; i < input.Length; i++) {
            var pos = alphabet.IndexOf (input [i]);
            number += (long) (pos * Math.Pow (alphabet.Length, input.Length - i - 1));
         }

         return number;
      }

      private long [] GetNumbersFrom (string hash)
      {
         if (string.IsNullOrWhiteSpace (hash))
            return new long [0];

         var alphabet = string.Copy (this.m_Alphabet);
         var ret = new List<long> ();
         int i = 0;

         var hashBreakdown = m_GuardsRegex.Replace (hash, " ");
         var hashArray = hashBreakdown.Split (new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

         if (hashArray.Length == 3 || hashArray.Length == 2)
            i = 1;

         hashBreakdown = hashArray [i];
         if (hashBreakdown [0] != default (char)) {
            var lottery = hashBreakdown [0];
            hashBreakdown = hashBreakdown.Substring (1);

            hashBreakdown = m_SepsRegex.Replace (hashBreakdown, " ");
            hashArray = hashBreakdown.Split (new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var subHash in hashArray) {
               var buffer = lottery + this.m_Salt + alphabet;
               alphabet = ConsistentShuffle (alphabet, buffer.Substring (0, alphabet.Length));
               ret.Add (Unhash (subHash, alphabet));
            }

            if (EncodeLong (ret.ToArray ()) != hash)
               ret.Clear ();
         }

         return ret.ToArray ();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="alphabet"></param>
      /// <param name="salt"></param>
      /// <returns></returns>
      private string ConsistentShuffle (string alphabet, string salt)
      {
         if (string.IsNullOrWhiteSpace (salt))
            return alphabet;

         int v, p, n, j;
         v = p = n = j = 0;

         for (var i = alphabet.Length - 1; i > 0; i--, v++) {
            v %= salt.Length;
            p += n = salt [v];
            j = (n + v + p) % i;

            var temp = alphabet [j];
            alphabet = alphabet.Substring (0, j) + alphabet [i] + alphabet.Substring (j + 1);
            alphabet = alphabet.Substring (0, i) + temp + alphabet.Substring (i + 1);
         }

         return alphabet;
      }
   }
}