using System;
using System.Security.Cryptography;
using System.Text;

namespace FileCompare2._0
{
    public static class StringExtension
    {
        public static string GetMD5Checksum(this string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(str)));
        }

        public static string FirstOf(this string str, char Of)
        {
            if (str == null) return str;
            if (str.Length == 0) return str;
            if (str.IndexOf(Of) == -1) return string.Empty;
            str = str.Substring(0, str.LastIndexOf(Of));
            return str;
        }

        public static string FirstOf(this string str, string Of)
        {
            if (str == null || Of == null) return str;
            if (str.Length == 0 || Of.Length == 0) return str;
            if (str.IndexOf(Of) == -1) return string.Empty;
            str = str.Substring(0, str.LastIndexOf(Of));
            return str;
        }

        public static string FirstOf(this string str, char Of, int index)
        {
            if (str == null) return str;
            if (str.Length == 0) return str;
            if (str.IndexOf(Of) == -1) return string.Empty;
            if (index == 0 || index == 1 || index == 88)
            {
                if (index == 88)
                    return str.FirstOf(Of, str.QuantityOf(Of));

                return str.Substring(0, str.IndexOf(Of));
            }

            int Quantity = 0;
            for (int i = 0; i < str.Length + 1; i++)
            {
                if (str[i] == Of)
                {
                    Quantity++;
                    if (Quantity == index)
                        return str.Substring(0, i + 1);
                }
            }

            return str;
        }

        public static string LastOf(this string str, string Of)
        {
            if (str == null || Of == null) return str;
            if (str.Length == 0 || Of.Length == 0) return str;
            if (str.IndexOf(Of) == -1) return string.Empty;
            return str.Substring(str.IndexOf(Of) + Of.Length);
        }

        public static string LastOf(this string str, char Of)
        {
            if (str == null) return str;
            if (str.Length == 0) return str;
            if (str.IndexOf(Of) == -1) return string.Empty;
            return str.Substring(str.LastIndexOf(Of) + 1);
        }

        public static int QuantityOf(this string str, char Elm)
        {
            int Quantity = 0;
            foreach (char chr in str)
                if (chr == Elm)
                    Quantity++;

            return Quantity;
        }

        public static int QuantityOf(this string str, string Elm)
        {
            int Quantity = 0;
            while (Elm.IndexOf(str) != -1)
            {
                Quantity++;
                Elm = Elm.Substring(Elm.IndexOf(str) + str.Length);
            }

            return Quantity;
        }

        public static string KnTextCorrect(this string str)
        {
            if (str != null && str.Length > 0)
            {
                string copy = str;
                str = str.Trim().ToLower();

                if (str.Length > 0)
                {
                    str = str.Replace("(", "");
                    str = str.Replace(")", "");
                    str = str.Replace("   ", "");
                    str = str.Replace("  ", "");
                    str = str.Replace(" ", "");
                    str = str.Replace("\t", "");
                }
                //for (int i = 0; i < str.Length; i++) if (str[i] == ' ' && i + 1 < str.Length) str = str.Substring(0, i + 1) + Char.ToUpper(str[i + 1]) + str.Substring(i + 2, str.Length - i - 2);
            }

            return str;
        }

        public static string TextCorrect(this string str)
        {
            if (str != null && str.Length > 0)
            {
                str = str.Trim();
                str = str.ToLower();

                if (str.Length > 0)
                {
                    str = Char.ToUpper(str[0]) + str.Substring(1);
                    str = str.Replace("+", " ");
                    str = str.Replace("_", " ");
                    str = str.Replace("-", " ");
                    str = str.Replace(".", " ");
                    str = str.Replace(",", "");
                    str = str.Replace("'", "");
                    str = str.Replace("\\", "");
                    str = str.Replace("/", "");
                    str = str.Replace("   ", " ");
                    str = str.Replace("  ", " ");
                }
                for (int i = 0; i < str.Length; i++) if (str[i] == ' ' && i + 1 < str.Length) str = str.Substring(0, i + 1) + Char.ToUpper(str[i + 1]) + str.Substring(i + 2, str.Length - i - 2);
            }
            return str;
        }
    }

    class TextEdit
    {
        //internal List<StrInfo> TextDivision(List<string> strList)
        //{
        //    List<StrInfo> strInfo = new List<StrInfo>();
        //    if (strList.Count > 0)
        //    {
        //        string[] TextList = new string[strList.Count];
        //        int i = 0;

        //        foreach (string elem in strList)
        //            TextList[i] = strList[i++].KnTextCorrect();

        //        strInfo = TextDivision(TextList);
        //    }

        //    return strInfo;
        //}

        //internal static List<StrInfo> TextDivision(string[] TextList)
        //{
        //    List<StrInfo> strInfoList = new List<StrInfo>();

        //    foreach (string elm in TextList)
        //    {
        //        StrInfo strInfo = new StrInfo() { Text = elm };

        //        string intElm = string.Empty, strElm = string.Empty, result = string.Empty;
        //        int nOfElem = 0;
        //        bool fl = false, oldfl = false;
        //        List<string> txtList = new List<string>();
        //        List<string> nmbList = new List<string>();

        //        for (int i = 0; i < strInfo.Text.Length; i++)
        //        {

        //            if (Convert.ToInt32(strInfo.Text[i]) >= 48 && Convert.ToInt32(strInfo.Text[i]) <= 57)
        //            {
        //                intElm += strInfo.Text[i];
        //                fl = true;
        //            }
        //            else
        //            {
        //                strElm += strInfo.Text[i];
        //                fl = false;
        //            }

        //            if (i != 0 && fl != oldfl)
        //            {
        //                nOfElem++;
        //                if (oldfl)
        //                {
        //                    result = result + intElm + " ";
        //                    nmbList.Add(intElm);
        //                    intElm = string.Empty;
        //                }
        //                else
        //                {
        //                    result = result + strElm + " ";
        //                    txtList.Add(strElm);
        //                    strElm = string.Empty;
        //                }
        //            }

        //            if (i == strInfo.Text.Length - 1)
        //            {

        //                if (fl)
        //                {
        //                    result = result + intElm;
        //                    nmbList.Add(intElm);
        //                    nOfElem++;
        //                }
        //                else
        //                {
        //                    result = result + strElm;
        //                    txtList.Add(strElm);
        //                    nOfElem++;
        //                }
        //            }
        //            oldfl = fl;
        //        }

        //        strInfo.TextList = txtList;
        //        strInfo.NumberList = nmbList;
        //        strInfo.DivisionText = result;
        //        strInfo.NOfElem = nOfElem;
        //        Console.WriteLine(strInfo.Text + " => " + strInfo.DivisionText);
        //        strInfoList.Add(strInfo);
        //    }

        //    return strInfoList;
        //}

        public string TextBetween(string Txt, string Fr, string To)
        {
            string Str = "";
            if (Txt.Length != 0 && Fr.Length != 0 && To.Length != 0)
            {
                if (Txt.IndexOf(Fr) != -1)
                {
                    Str = Txt.Substring(Txt.IndexOf(Fr) + Fr.Length);
                    if (Str.IndexOf(To) != -1) Str = Str.Substring(0, Str.IndexOf(To));
                }
            }
            return Str;
        }
    }
}
