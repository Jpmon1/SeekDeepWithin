using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.DataAccess
{
   public class PassageParser
   {
      private string m_LastBook = "";
      private readonly ISdwDatabase m_Db;
      private readonly StringBuilder m_Log = new StringBuilder();
      private readonly List<PassageViewModel> m_PassageList = new List<PassageViewModel> ();

      /// <summary>
      /// Initializes a new passage parser.
      /// </summary>
      public PassageParser (ISdwDatabase db)
      {
         this.m_Db = db;
      }
      
      /// <summary>
      /// Gets or Sets the list of parsed passages.
      /// </summary>
      public List<PassageViewModel> PassageList { get { return this.m_PassageList; } }

      public string Log { get; private set; }

      /// <summary>
      /// Parses the given passages.
      /// </summary>
      /// <param name="par">Passeges to parse.</param>
      public void Parse (string par)
      {
         this.m_Log.Clear ();
         string book = "";
         string chapter = "";
         string verse = "";
         bool bBook = true;
         bool bVerse = false;
         bool bChapter = false;
         bool handled = false;
         par = par.Replace (" ", "");
         int length = par.Length;

         int charIndex = 0;
         while (Char.IsDigit (par [charIndex]))
         {
            book += par [charIndex];
            charIndex++;
         }

         for (; charIndex < length; charIndex++)
         {
            char currentChar = par [charIndex];
            if (currentChar == ':')
            {
               bBook = false;
               bChapter = false;
               bVerse = true;
            }
            else if (currentChar == ',')
            {
               if (bBook)
               {
                  if ((charIndex + 1) < length)
                  {
                     if (!Char.IsDigit (par [charIndex + 1]))
                     {
                        book += currentChar;
                        handled = true;
                     }
                  }
               }
               if (!handled)
               {
                  AddPassages (book, chapter, verse);
                  if (verse == "") chapter = "";
                  verse = "";
               }
            }
            else if (currentChar == ';')
            {
               AddPassages (book, chapter, verse);
               book = "";
               chapter = "";
               verse = "";
               bBook = true;
               bChapter = false;
               bVerse = false;
            }
            else if (currentChar == '.')
            {
               if (bBook)
               {
                  bBook = false;
                  bChapter = true;
                  bVerse = false;
               }
               else
               {
                  bBook = false;
                  bChapter = false;
                  bVerse = true;
               }
            }
            else if (currentChar == '-')
            {
               if (bChapter)
                  chapter += currentChar;
               if (bVerse)
                  verse += currentChar;
            }
            else if (Char.IsDigit (currentChar))
            {
               if (bBook)
               {
                  if ((charIndex + 1) < length && Char.IsLetter (par [charIndex + 1]) /* && book != "" */)
                     book += currentChar;
                  else
                  {
                     bBook = false;
                     bChapter = true;
                     chapter += currentChar;
                  }
               }
               else if (bChapter)
                  chapter += currentChar;
               else
                  verse += currentChar;
            }
            else if (bBook)
               book += currentChar;
         }
         AddPassages (book, chapter, verse);
         this.Log = this.m_Log.ToString ();
      }

      /**
       * Adds all passages for the given list.
       * @param string book The book.
       * @param string chapter The chapter, or chapters.
       * @param string passage The verse, or verses.
       */
      private void AddPassages (string book, string chapter, string passage)
      {
         this.m_Log.AppendFormat ("<li>Adding {0}|{1}|{2}</li>", book, chapter, passage);
         if (book == "" && chapter == "")
            return;
         if (book == "")
         {
            if (m_LastBook == "") // Unable to determine the book...
               return;
            book = m_LastBook;
         }

         this.m_LastBook = book;
         var passages = new List <int> ();
         var chapters = new List<int> ();
         if (chapter.Contains ("-"))
         {
            string[] chapterSplit = chapter.Split ('-');
            int start = Convert.ToInt32 (chapterSplit[0]);
            int end = Convert.ToInt32 (chapterSplit[1]);
            for (int a = start; a <= end; a++)
            {
               chapters.Add (a);
               if (!string.IsNullOrWhiteSpace (passage) && a == end)
               {
                  for (int c = 1; c <= Convert.ToInt32 (passage); c++)
                     passages.Add (c);
               }
            }
         }
         else
         {
            if (!string.IsNullOrWhiteSpace(chapter))
               chapters.Add (Convert.ToInt32 (chapter));

            if (passage.Contains ("-"))
            {
               string[] passageSplit = passage.Split ('-');
               int start = Convert.ToInt32 (passageSplit[0]);
               int end = Convert.ToInt32 (passageSplit[1]);
               for (int a = start; a <= end; a++)
                  passages.Add (a);
            }
            else if (!string.IsNullOrEmpty (passage))
               passages.Add (Convert.ToInt32 (passage));
         }

         var abbreviations = this.m_Db.Abbreviations.Get (a => a.Text == book).ToList();
         this.m_Log.AppendFormat ("<li>Found {0} abbreviation(s)</li>", abbreviations.Count);
         foreach (var abbreviation in abbreviations)
         {
            var version = abbreviation.SubBook.Versions.FirstOrDefault (v => v.Version.IsDefault) ??
                          abbreviation.SubBook.Versions.FirstOrDefault ();
            if (version != null)
            {
               var chaps = new List <SubBookChapter> ();
               if (chapters.Count == 0)
                  chaps.Add (version.Chapters.FirstOrDefault ());
               else if (chapters.Count == 1)
                  chaps.Add (version.Chapters.FirstOrDefault (c => c.Order == chapters [0]));
               else
               {
                  chaps.AddRange ((from subBookChapter in version.Chapters
                     where chapters.Contains (subBookChapter.Order)
                     select subBookChapter));
               }

               for (int a = 0; a < chaps.Count; a++)
               {
                  if (passages.Count == 0)
                     this.PassageList.AddRange (chaps[a].Passages.Select(pe => new PassageViewModel(pe)));
                  else
                  {
                     if (a + 1 == chaps.Count)
                     {
                        this.PassageList.AddRange ((from passageEntry in chaps[a].Passages
                                                    where passages.Contains (passageEntry.Number)
                                                    select new PassageViewModel (passageEntry)));
                     }
                     else
                        this.PassageList.AddRange (chaps[a].Passages.Select (pe => new PassageViewModel (pe)));
                  }
               }
            }
         }
      }

      /// <summary>
      /// Builds html output for the parsed passages.
      /// </summary>
      /// <param name="url"></param>
      /// <returns>The html output of the parse.</returns>
      public string BuildHtmlOutput (Uri url)
      {
         var renderer = new SdwRenderer { DoFooters = false };
         var html = new StringBuilder ();
         html.AppendLine("<div class=\"panel passageParseTable\">");
         var lastChapter = -1;
         var host = url == null ? string.Empty : url.AbsoluteUri.Replace (url.AbsolutePath, "");
         foreach (var passage in this.PassageList)
         {
            passage.Renderer = renderer;
            if (passage.ChapterId != lastChapter)
            {
               html.AppendLine ("<div class=\"row\">");
               html.AppendLine ("<div class=\"small-12 columns\">");
               html.AppendFormat ("<a href=\"{0}/Chapter/Read/{1}\">", host, passage.ChapterId);
               html.AppendLine (passage.GetTitleNoVerse ());
               html.AppendLine ("</a>");
               html.AppendLine ("</div>");
               html.AppendLine ("</div>");
               lastChapter = passage.ChapterId;
            }
            html.AppendLine ("<div class=\"row\">");
            html.AppendLine ("<div class=\"small-2 medium-1 large-1 columns\"><blockquote>");
            html.AppendFormat ("<a href=\"{0}/Passage?entryId={1}\">", host, passage.EntryId);
            html.AppendLine (passage.Number.ToString (CultureInfo.InvariantCulture));
            html.AppendLine ("</a>");
            html.AppendLine ("</blockquote></div>");
            html.AppendLine ("<div class=\"small-10 medium-11 large-11 columns\">");
            html.AppendLine (passage.Render(url));
            html.AppendLine ("</div>");
            html.AppendLine ("</div>");
         }
         html.AppendLine ("</div>");
         return html.ToString();
      }
      /*
      /// <summary>
      /// Gets the fully qualified name of the given book.
      /// </summary>
      /// <param name="book">The abbreviation of the book.</param>
      /// <returns>Data about the book.</returns>
      private Dictionary <string, string> GetBook (string book)
      {
         bool hasChapters = true;
         string b = book.Replace (" ", "").ToLower ();
         string subBook = book;
         string rtnBook = "Bible";
         string version = string.Empty;

         //var abbreviations = this.m_Db.Abbreviations.Get (a => a.Text == book);

         if (b == "")
            subBook = b;
         else if (b == "tobit" || b == "tob" || b == "tb")
         {
            version = "Douay-Rheims Bible";
            subBook = "Tobit";
         }
         else if (b == "judith" || b == "jdth" || b == "jdt" || b == "jth")
         {
            version = "Douay-Rheims Bible";
            subBook = "Judith";
         }
         else if (b == "additionstoesther" || b == "addesth" || b == "addes" ||
                  b == "restofesther" || b == "therestofesther" || b == "aes")
            subBook = "Additions to Esther";
         else if (b == "wisdomofsolomon" || b == "wisdofsol" || b == "wis" || b == "ws" ||
                  b == "wisdom")
         {
            version = "Douay-Rheims Bible";
            subBook = "Wisdom of Solomon";
         }
         else if (b == "sirach" || b == "sir" || b == "ecclesiasticus" || b == "ecclus")
            subBook = "Sirach";
         else if (b == "baruch" || b == "bar")
         {
            version = "Douay-Rheims Bible";
            subBook = "Baruch";
         }
         else if (b == "letterofjeremiah" || b == "letjer" || b == "lje" ||
                  b == "ltrjer")
            subBook = "Letter of Jeremiah";
         else if (b == "songofthreeyouths" || b == "songofthree" || b == "songthr" ||
                  b == "thesongofthreeyouths" || b == "praz" ||
                  b == "prayerofazariah" || b == "azariah" ||
                  b == "thesongofthethreeholychildren" || b == "thesongofthreejews" ||
                  b == "songofthethreeholychildren" || b == "songofthr" ||
                  b == "songofthreechildren" || b == "songofthreejews")
            subBook = "Song of Three Youths";
         else if (b == "susanna" || b == "sus")
            subBook = "Susanna";
         else if (b == "belandthedragon" || b == "bel")
            subBook = "Bel and the Dragon";
         else if (b == "1maccabees" || b == "1macc" || b == "1mac" || b == "1m" ||
                  b == "ima" || b == "1ma" || b == "imac" || b == "imacc" ||
                  b == "imaccabees" || b == "1stmaccabees" ||
                  b == "firstmaccabees")
         {
            version = "Douay-Rheims Bible";
            subBook = "1 Maccabees";
         }
         else if (b == "2maccabees" || b == "2macc" || b == "2mac" ||
                  b == "2m" || b == "iima" || b == "2ma" || b == "iimac" ||
                  b == "iimacc" || b == "iimaccabees" || b == "2ndmaccabees" ||
                  b == "secondmaccabees")
         {
            version = "Douay-Rheims Bible";
            subBook = "2 Maccabees";
         }
         else if (b == "1esdras" || b == "1esdr" || b == "1esd" ||
                  b == "ies" || b == "1es" || b == "iesd" || b == "iesdr" ||
                  b == "iesdras" || b == "1stesdras" || b == "firstesdras")
            subBook = "1 Esdras";
         else if (b == "prayerofmanasseh" || b == "profman" ||
                  b == "prman" || b == "pma" || b == "prayerofmanasses")
            subBook = "Prayer of Manasseh";
         else if (b == "additionalpsalm" || b == "addpsalm" ||
                  b == "addps")
            subBook = "Additional Psalm";
         else if (b == "3maccabees" || b == "3macc" ||
                  b == "3mac" || b == "iiima" || b == "3ma" ||
                  b == "iiimac" || b == "iiimacc" ||
                  b == "iiimaccabees" || b == "3rdmaccabees" ||
                  b == "thirdmaccabees")
            subBook = "3 Maccabees";
         else if (b == "2esdras" || b == "2esdr" ||
                  b == "2esd" || b == "iies" || b == "2es" ||
                  b == "iiesd" || b == "iiesdr" ||
                  b == "iiesdras" || b == "2ndesdras" ||
                  b == "secondesdras")
            subBook = "2 Esdras";
         else if (b == "4maccabees" || b == "4macc" ||
                  b == "4mac" || b == "ivma" || b == "4ma" ||
                  b == "ivmac" || b == "ivmacc" ||
                  b == "ivmaccabees" ||
                  b == "iiiimaccabees" ||
                  b == "4thmaccabees" ||
                  b == "fourthmaccabees")
            subBook = "4 Maccabees";
         else if (b == "ode")
            subBook = "Ode";
         else if (b == "psalmsofsolomon" ||
                  b == "pssolomon" || b == "pssol" ||
                  b == "psalmssolomon")
            subBook = "Psalms of Solomon";
         else if (b == "epistletothelaodiceans" ||
                  b == "laodiceans" || b == "laod" ||
                  b == "eplaod" ||
                  b == "epistlaodiceans" ||
                  b == "epistlelaodiceans" ||
                  b == "epistletolaodiceans")
         {
            subBook = "Epistle to the Laodiceans";
         }

         return new Dictionary <string, string>
         {
            {"book", rtnBook},
            {"version", version},
            {"subbook", subBook},
            {"haschapters", hasChapters ? "true" : "false"}
         };
      }*/
   }
}