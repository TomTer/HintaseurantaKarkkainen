using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace HintaseurantaKarkkainen
{
    class Libs
    {

        public static readonly string MainSite = "https://www.karkkainen.com/verkkokauppa/";
        public static readonly string PictureUrlPart = "https://www.karkkainen.com/tuotekuva/ISO/";

        /**
         * Funktio palauttaa UNIX-timestampin
         */
        public static long GetUnixTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        /**
         * Regexia varten oma funktio. Palauttaa kaikki Matchit
         */
        public static MatchCollection GetMatches(string strInput, string regexStr)
        {
            var regexObject = new Regex(regexStr);
            return regexObject.Matches(strInput);
        }

    }
}
