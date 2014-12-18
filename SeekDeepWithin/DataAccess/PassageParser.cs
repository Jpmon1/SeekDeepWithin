using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SeekDeepWithin.DataAccess
{
   public class PassageParser
   {
      private readonly Collection <string> m_PassageList = new Collection <string> ();
      private string m_LastBook = "";

      /// <summary>
      ///    Gets or Sets the list of parsed passages.
      /// </summary>
      public List <string> PassageList { get; set; }

      /// <summary>
      ///    Gets or Sets the default bible.
      /// </summary>
      public string DefaultBible { get; set; }

      /**
       * Parses and renders the given passage into HTML.
       * @param string passageToParse Passage To Parse.
       * @param boolean inTable True if data is in render in a table.
       * @param boolean isMobile If true, links will point to mobile site.
       * @return string The outputted HTML from the parse operation.
       */
      /*public void Render (string passageToParse = "", bool inTable = true,
              bool isMobile = false)
      {
         host = (isMobile) ? "http://m.seekdeepwithin.com/" : "http://seekdeepwithin.com/";
         if (passageToParse != "") {
            this->m_PassageList = array ();
            this->parsePassages (passageToParse);
         }
         passageList = this->getPassageList ();
         db = new SdwDatabase();
         foreach (passageList as bookTitle => versionArray) {
            bookId = db->getBookId (bookTitle);
            encodeBook = urlencode (bookTitle);
            foreach (versionArray as versionName => subBookArray) {
               versionId = db->getVersionId (versionName);
               bookVersionId = db->getBookVersionId (bookId, versionId);
               encodeVersion = urlencode (versionName);
               bookData = db->getBookData (bookVersionId, true);
               html = "<div class=\"panel panel-default\" style=\"margin-left:20px;margin-right:20px;\"><div class=\"panel-heading\"><h3 class=\"panel-title\"><strong>";
               html .= isset (bookData ['version_title']) ? bookData ['version_title'] : bookData ['title'];
               html .= "</strong></h3></div>";
               foreach (subBookArray as subbookTitle => chapterArray) {
                  subbookId = db->getSubbookId (subbookTitle);
                  versionSubbookId = db->getVersionSubbookId (bookVersionId,
                          subbookId);
                  encodeSubbook = urlencode (subbookTitle);
                  foreach (chapterArray as chapter => passageArray) {
                     title = "";
                     subbookChapterId = db->getSubbookChapterIdFromOrder (versionSubbookId,
                             chapter, chapterName);
                     link = host . "?area=read&b=encodeBook";
                     if (versionName != "default") {
                        link .= "&v=encodeVersion";
                     }
                     if (subbookTitle != "default") {
                        link .= "&s=encodeSubbook";
                        title = "subbookTitle ";
                     }
                     if (chapterName != "default") {
                        link .= "&c=chapterName";
                     }
                     title .= chapterName;
                     passages = db->getPassage (subbookChapterId, passageArray);
                     html .= "<table class=\"table\">";
                     html .= "<tr><td colspan=\"2\"><a class=\"list-group-item\" href=\"link\" title=\"Open title\">";
                     html .= "<span class=\"badge\"><span class=\"glyphicon glyphicon-share-alt\"></span></span>";
                     html .= "<strong>title</strong></a></td></tr>";
                     foreach (passages as passage) {
                        pLink = link . "&p=" . passage['passage_number'];
                        html .= "<tr><td><a href=\"pLink\" class=\"btn btn-default\">";
                        html .= passage['passage_number'];
                        html .= "</a></td><td width=\"100%\">";
                        html .= passage['passage'];
                        html .= "</td></tr>";
                     }
                     html .= "</table>";
                  }
               }
               html .= "</div>";
            }
         }
         return html;
      }*/

      /**
       * Parses the given passages.
       * @param string par The passages to parse.
       */

      public void Parse (string par)
      {
         string book = "";
         string chapter = "";
         string verse = "";
         bool bBook = true;
         bool bVerse = false;
         bool bChapter = false;
         bool handled = false;
         int length = par.Length;
         par = par.Replace (" ", ""); // preg_replace ("/\s/u", "", par);

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
      }

      /**
       * Adds all passages for the given list.
       * @param string book The book.
       * @param string chapter The chapter, or chapters.
       * @param string passage The verse, or verses.
       */

      private void AddPassages (string book, string chapter, string passage)
      {
         if (book == "" && chapter == "")
            return;
         if (book == "")
         {
            if (m_LastBook == "") // Unable to determine the book...
               return;
            book = m_LastBook;
         }

         Dictionary <string, string> bookData = GetBook (book);
         if (bookData ["subbook"] == "")
            return;

         if (bookData ["haschapters"] == "false" && passage == "")
         {
            passage = chapter;
            chapter = "";
         }

         if (passage.IndexOf ("-", StringComparison.Ordinal) != -1)
         {
            string[] passageSplit = passage.Split ('-');
            int start = Convert.ToInt32 (passageSplit [0]);
            int end = Convert.ToInt32 (passageSplit [1]);
            for (int a = start; a <= end; a++)
            {
               AddPassage (bookData ["book"], bookData ["version"],
                  bookData ["subbook"], chapter, a.ToString (CultureInfo.InvariantCulture));
            }
         }
         else if (chapter.IndexOf ("-", StringComparison.Ordinal) != -1)
         {
            string[] chapterSplit = chapter.Split ('-');
            int start = Convert.ToInt32 (chapterSplit [0]);
            int end = Convert.ToInt32 (chapterSplit [1]);
            for (int a = start; a <= end; a++)
            {
               if (passage != "" && a == end)
               {
                  for (int c = 1; c <= Convert.ToInt32 (passage); c++)
                  {
                     AddPassage (bookData ["book"], bookData ["version"],
                        bookData ["subbook"], a.ToString (CultureInfo.InvariantCulture),
                        c.ToString (CultureInfo.InvariantCulture));
                  }
               }
               else
               {
                  AddPassage (bookData ["book"], bookData ["version"],
                     bookData ["subbook"], a.ToString (CultureInfo.InvariantCulture), "all");
               }
            }
         }
         else
         {
            AddPassage (bookData ["book"], bookData ["version"],
               bookData ["subbook"], chapter, passage != "" ? passage : "all");
         }
      }

      /**
       * Adds the given passage ot the passage list.
       * @param type book The book to add.
       * @param type subBook The sub book to add.
       * @param type chapter The chapter to add.
       * @param type passage The passage to add.
       */

      private void AddPassage (string book, string version, string subBook, string chapter, string passage)
      {
         if (book != "" && subBook != "" && chapter != "")
         {
            m_LastBook = subBook == "default" ? book : subBook;
            //this.m_PassageList [book] [version] [subBook] [chapter] [] = passage;
         }
      }

      /// <summary>
      ///    Gets the fully qualified name of the given book.
      /// </summary>
      /// <param name="book">The abbreviation of the book.</param>
      /// <returns>Data about the book.</returns>
      public Dictionary <string, string> GetBook (string book)
      {
         bool hasChapters = true;
         string b = book.Replace (" ", "").ToLower (); //strtolower (str_replace (" ", '', book));
         string subBook = book;
         string rtnBook = "Bible";
         string version = DefaultBible;

         if (b == "")
            subBook = b;
         else if (b == "genesis" || b == "gen" || b == "ge" || b == "gn")
            subBook = "Genesis";
         else if (b == "exodus" || b == "exo" || b == "ex" || b == "exod")
            subBook = "Exodus";
         else if (b == "leviticus" || b == "lev" || b == "le" || b == "lv")
            subBook = "Leviticus";
         else if (b == "numbers" || b == "num" || b == "nu" || b == "nm" || b == "nb")
            subBook = "Numbers";
         else if (b == "deuteronomy" || b == "deut" || b == "dt" || b == "deu" || b == "de")
            subBook = "Deuteronomy";
         else if (b == "joshua" || b == "josh" || b == "jos" || b == "jsh")
            subBook = "Joshua";
         else if (b == "judges" || b == "judg" || b == "jdg" || b == "jg" || b == "jdgs")
            subBook = "Judges";
         else if (b == "ruth" || b == "rth" || b == "ru" || b == "rut")
            subBook = "Ruth";
         else if (b == "1samuel" || b == "1sam" || b == "1sa" || b == "1s" || b == "1sm" || b == "isam" || b == "isamuel" ||
                  b == "1stsamuel" || b == "firstsamuel")
            subBook = "1 Samuel";
         else if (b == "2samuel" || b == "2sam" || b == "2sa" || b == "2s" || b == "iisa" || b == "2sm" || b == "iisam" ||
                  b == "iisamuel" || b == "2ndsamuel" || b == "secondsamuel")
            subBook = "2 Samuel";
         else if (b == "1kings" || b == "1kgs" || b == "1kg" || b == "1ki" || b == "1k" || b == "ikgs" || b == "iki" ||
                  b == "ikings" || b == "1stkgs" || b == "1stkings" || b == "firstkings" || b == "firstkgs" ||
                  b == "1kin")
            subBook = "1 Kings";
         else if (b == "2kings" || b == "2kgs" || b == "2kg" || b == "2ki" || b == "2k" || b == "iikgs" ||
                  b == "iiki" || b == "iikings" || b == "2ndkgs" || b == "2ndkings" || b == "secondkings" ||
                  b == "secondkgs" || b == "2kin")
            subBook = "2 Kings";
         else if (b == "1chronicles" || b == "1chron" || b == "1ch" || b == "ich" || b == "1chr" ||
                  b == "ichr" || b == "ichron" || b == "ichronicles" || b == "1stchronicles" ||
                  b == "firstchronicles")
            subBook = "1 Chronicles";
         else if (b == "2chronicles" || b == "2chron" || b == "2ch" || b == "iich" || b == "iichr" ||
                  b == "2chr" || b == "iichron" || b == "iichronicles" || b == "2ndchronicles" ||
                  b == "secondchronicles")
            subBook = "2 Chronicles";
         else if (b == "ezra" || b == "ezr")
            subBook = "Ezra";
         else if (b == "nehemiah" || b == "neh" || b == "ne")
            subBook = "Nehemiah";
         else if (b == "esther" || b == "esth" || b == "es")
            subBook = "Esther";
         else if (b == "job" || b == "jb")
            subBook = "Job";
         else if (b == "psalm" || b == "pslm" || b == "ps" || b == "psalms" || b == "psa" || b == "psm" ||
                  b == "pss")
            subBook = "Psalms";
         else if (b == "proverbs" || b == "prov" || b == "pr" || b == "prv" || b == "pro")
            subBook = "Proverbs";
         else if (b == "ecclesiastes" || b == "eccles" || b == "ec" || b == "qoh" || b == "qoheleth" ||
                  b == "ecc" || b == "eccl")
            subBook = "Ecclesiastes";
         else if (b == "songofsolomon" || b == "song" || b == "so" || b == "canticleofcanticles" ||
                  b == "canticles" || b == "songofsongs" || b == "sos")
            subBook = "Song of Solomon";
         else if (b == "isaiah" || b == "isa" || b == "is")
            subBook = "Isaiah";
         else if (b == "jeremiah" || b == "jer" || b == "je" || b == "jr")
            subBook = "Jeremiah";
         else if (b == "lamentations" || b == "lam" || b == "la")
            subBook = "Lamentations";
         else if (b == "ezekiel" || b == "ezek" || b == "eze" || b == "ezk")
            subBook = "Ezekiel";
         else if (b == "daniel" || b == "dan" || b == "da" || b == "dn")
            subBook = "Daniel";
         else if (b == "hosea" || b == "hos" || b == "ho")
            subBook = "Hosea";
         else if (b == "joel" || b == "joe" || b == "jl")
            subBook = "Joel";
         else if (b == "amos" || b == "am" || b == "amo")
            subBook = "Amos";
         else if (b == "obadiah" || b == "obad" || b == "ob" || b == "obd" || b == "oba")
            subBook = "Obadiah";
         else if (b == "jonah" || b == "jnh" || b == "jon")
            subBook = "Jonah";
         else if (b == "micah" || b == "mic")
            subBook = "Micah";
         else if (b == "nahum" || b == "nah" || b == "na")
            subBook = "Nahum";
         else if (b == "habakkuk" || b == "hab")
            subBook = "Habakkuk";
         else if (b == "zephaniah" || b == "zeph" || b == "zep" || b == "zp")
            subBook = "Zephaniah";
         else if (b == "haggai" || b == "hag" || b == "hg")
            subBook = "Haggai";
         else if (b == "zechariah" || b == "zech" || b == "zec" || b == "zc")
            subBook = "Zechariah";
         else if (b == "malachi" || b == "mal" || b == "ml")
            subBook = "Malachi";
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
         else if (b == "matthew" || b == "matt" ||
                  b == "mt" || b == "mat")
            subBook = "Matthew";
         else if (b == "mark" || b == "mrk" ||
                  b == "mk" || b == "mr")
            subBook = "Mark";
         else if (b == "luke" || b == "luk" ||
                  b == "lk" || b == "lu")
            subBook = "Luke";
         else if (b == "john" ||
                  b == "jn" || b == "jhn" ||
                  b == "joh")
            subBook = "John";
         else if (b == "acts" ||
                  b == "ac" ||
                  b == "act")
            subBook = "Acts";
         else if (b == "romans" ||
                  b == "rom" ||
                  b == "ro" ||
                  b == "rm")
            subBook = "Romans";
         else if (b == "1corinthians" ||
                  b == "1cor" ||
                  b == "1co" ||
                  b == "ico" ||
                  b == "icor" ||
                  b == "icorinthians" ||
                  b == "1stcorinthians" ||
                  b == "firstcorinthians")
         {
            subBook ="1 Corinthians";
         }
         else if (b == "2corinthians" ||
                  b == "2cor" ||
                  b == "2co" ||
                  b == "iico" ||
                  b == "iicor" ||
                  b == "iicorinthians" ||
                  b == "2ndcorinthians" ||
                  b == "secondcorinthians")
         {
            subBook = "2 Corinthians";
         }
         else if (b ==
                  "galatians" ||
                  b ==
                  "gal" ||
                  b ==
                  "ga")
         {
            subBook =
               "Galatians";
         }
         else if (b ==
                  "ephesians" ||
                  b ==
                  "ephes" ||
                  b ==
                  "eph")
         {
            subBook
               =
               "Ephesians";
         }
         else if (
            b ==
            "philippians" ||
            b ==
            "phil" ||
            b ==
            "php" ||
            b ==
            "phili" ||
            b ==
            "philip")
            subBook = "Philippians";
         else if (b == "colossians" || b == "col")
            subBook = "Colossians";
         else if (b == "1thessalonians" ||
                  b ==
                  "1thess" ||
                  b ==
                  "1th" ||
                  b ==
                  "ith" ||
                  b ==
                  "ithes" ||
                  b ==
                  "1thes" ||
                  b ==
                  "ithess" ||
                  b ==
                  "ithessalonians" ||
                  b ==
                  "1stthessalonians" ||
                  b ==
                  "firstthessalonians")
         {
            subBook
               =
               "1 Thessalonians";
         }
         else if
            (
            b ==
            "2thessalonians" ||
            b ==
            "2thess" ||
            b ==
            "2th" ||
            b ==
            "iith" ||
            b ==
            "iithes" ||
            b ==
            "2thes" ||
            b ==
            "iithess" ||
            b ==
            "iithessalonians" ||
            b ==
            "2ndthessalonians" ||
            b ==
            "secondthessalonians")
         {
            subBook
               =
               "2 Thessalonians";
         }
         else if
            (
            b ==
            "1timothy" ||
            b ==
            "1tim" ||
            b ==
            "1ti" ||
            b ==
            "iti" ||
            b ==
            "itim" ||
            b ==
            "itimothy" ||
            b ==
            "1sttimothy" ||
            b ==
            "firsttimothy")
         {
            subBook
               =
               "1 Timothy";
         }
         else if
            (
            b ==
            "2timothy" ||
            b ==
            "2tim" ||
            b ==
            "2ti" ||
            b ==
            "iiti" ||
            b ==
            "iitim" ||
            b ==
            "iitimothy" ||
            b ==
            "2ndtimothy" ||
            b ==
            "secondtimothy")
         {
            subBook
               =
               "2 Timothy";
         }
         else if
            (
            b ==
            "titus" ||
            b ==
            "tit")
         {
            subBook
               =
               "Titus";
         }
         else if
            (
            b ==
            "philemon" ||
            b ==
            "philem" ||
            b ==
            "phm")
         {
            subBook
               =
               "Philemon";
         }
         else if
            (
            b ==
            "hebrews" ||
            b ==
            "heb")
         {
            subBook
               =
               "Hebrews";
         }
         else if
            (
            b ==
            "james" ||
            b ==
            "jas" ||
            b ==
            "jm" ||
            b ==
            "jam")
         {
            subBook
               =
               "James";
         }
         else if
            (
            b ==
            "1peter" ||
            b ==
            "1pet" ||
            b ==
            "1pe" ||
            b ==
            "ipe" ||
            b ==
            "ipet" ||
            b ==
            "ipt" ||
            b ==
            "1pt" ||
            b ==
            "ipeter" ||
            b ==
            "1stpeter" ||
            b ==
            "firstpeter")
         {
            subBook
               =
               "1 Peter";
         }
         else if
            (
            b ==
            "2peter" ||
            b ==
            "2pet" ||
            b ==
            "2pe" ||
            b ==
            "iipe" ||
            b ==
            "iipet" ||
            b ==
            "iipt" ||
            b ==
            "2pt" ||
            b ==
            "iipeter" ||
            b ==
            "2ndpeter" ||
            b ==
            "secondpeter")
         {
            subBook
               =
               "2 Peter";
         }
         else if
            (
            b ==
            "1john" ||
            b ==
            "1jn" ||
            b ==
            "ijn" ||
            b ==
            "ijo" ||
            b ==
            "1jo" ||
            b ==
            "ijoh" ||
            b ==
            "1joh" ||
            b ==
            "ijhn" ||
            b ==
            "1jhn" ||
            b ==
            "ijohn" ||
            b ==
            "1stjohn" ||
            b ==
            "firstjohn")
         {
            subBook
               =
               "1 John";
         }
         else if
            (
            b ==
            "2john" ||
            b ==
            "2jn" ||
            b ==
            "iijn" ||
            b ==
            "iijo" ||
            b ==
            "2jo" ||
            b ==
            "iijoh" ||
            b ==
            "2joh" ||
            b ==
            "iijhn" ||
            b ==
            "2jhn" ||
            b ==
            "iijohn" ||
            b ==
            "2ndjohn" ||
            b ==
            "secondjohn")
         {
            subBook
               =
               "2 John";
         }
         else if
            (
            b ==
            "3john" ||
            b ==
            "3jn" ||
            b ==
            "iiijn" ||
            b ==
            "iiijo" ||
            b ==
            "3jo" ||
            b ==
            "iiijoh" ||
            b ==
            "3joh" ||
            b ==
            "iiijhn" ||
            b ==
            "3jhn" ||
            b ==
            "iiijohn" ||
            b ==
            "3rdjohn" ||
            b ==
            "thirdjohn")
         {
            subBook
               =
               "3 John";
         }
         else if
            (
            b ==
            "jude" ||
            b ==
            "jud")
         {
            subBook
               =
               "Jude";
         }
         else if
            (
            b ==
            "revelation" ||
            b ==
            "rev" ||
            b ==
            "re" ||
            b ==
            "therevelation")
         {
            subBook
               =
               "Revelation";
         }
         else if
            (
            b ==
            "aquariangospelofjesusthechrist" ||
            b ==
            "agjc")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Aquarian Gospel of Jesus the Christ";
         }
         else if
            (
            b ==
            "gospelofbuddha" ||
            b ==
            "gob" ||
            b ==
            "gospelofbuddha,the")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Gospel of Buddha, The";
         }
         else if
            (
            b ==
            "bhagavadgita" ||
            b ==
            "bg")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Bhagavad Gita";
         }
         else if
            (
            b ==
            "tao" ||
            b ==
            "taoteching")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Tao Te Ching";
         }
         else if
            (
            b ==
            "thelifeofsaintissa" ||
            b ==
            "lifeofsaintissa" ||
            b ==
            "lsi" ||
            b ==
            "lsibsm")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "The Life of Saint Issa";
         }
         else if
            (
            b ==
            "thegospelofpeter" ||
            b ==
            "gospelofpeter" ||
            b ==
            "gop")
         {
            hasChapters
               =
               false;
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Gospel of Peter";
         }
         else if
            (
            b ==
            "q" ||
            b ==
            "qur" ||
            b ==
            "quran" ||
            b ==
            "koran" ||
            b ==
            "qur'an")
         {
            subBook
               =
               "default";
            version
               =
               "default";
            rtnBook
               =
               "Quran";
         }

         return new Dictionary <string, string>
         {
            {"book", rtnBook},
            {"version", version},
            {"subbook", subBook},
            {"haschapters", hasChapters ? "true" : "false"}
         };
      }
   }
}