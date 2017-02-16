using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vlast.Util.Global
{
    /// <summary>
    /// Helper para trabalhar com textos e strings.
    /// </summary>
    public class TextUtils
    {
        static DateTime EPOCH_DATE = new DateTime(1970, 01, 01);

        /// <summary>
        /// Remove os acentos e substitui pelos caracteres sem acento.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveAccents(string text)
        {
            return new string(text
                                .Normalize(NormalizationForm.FormD)
                                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                                .ToArray());
        }


        /// <summary>
        /// Verifica se existe acentos na string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool StringContainsAccents(string text)
        {
            foreach (char c in text)
            {
                string s = c.ToString().Normalize(NormalizationForm.FormD);
                if ((s.Length > 1) && char.IsLetter(s[0]) &&
                   s.Skip(1).All(c2 => CharUnicodeInfo.GetUnicodeCategory(c2) == UnicodeCategory.NonSpacingMark))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Verifica se é um email válido
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;

            domainName = idn.GetAscii(domainName);
            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Retorna uma URL para a um texto
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="size">tamanho da url</param>
        /// <returns></returns>
        public static string TextToURL(string phrase, int size = 0)
        {
            string str = RemoveAccents(phrase).ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            if (size > 0)
            {
                return str.Length < size ? str.Substring(0, size) : str;
            }

            return str;
        }


        /// <summary>
        /// Valida o CPF.
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public static Boolean IsValidCpf(string CPF)
        {
            int[] mt1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mt2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string TempCPF;
            string Digito;
            int soma;
            int resto;

            CPF = CPF.Trim();
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (CPF.Length != 11)
                return false;

            TempCPF = CPF.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = resto.ToString();
            TempCPF = TempCPF + Digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(TempCPF[i].ToString()) * mt2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            Digito = Digito + resto.ToString();

            return CPF.EndsWith(Digito);
        }

        /// <summary>
        /// Valida a string base64.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool CheckBase64String(string param)
        {
            if (TextUtils.CheckBase64StringSafe(param) == false)
            {
                return false;
            }
            return true;
        }

        private const char Base64Padding = '=';

        private static readonly HashSet<char> Base64Characters = new HashSet<char>()
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
            'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
        };

        private static bool CheckBase64StringSafe(string param)
        {
            if (param == null)
            {
                // null string is not Base64 something
                return false;
            }

            // replace optional CR and LF characters
            param = param.Replace("\r", String.Empty).Replace("\n", String.Empty);

            if (param.Length == 0 ||
                (param.Length % 4) != 0)
            {
                // Base64 string should not be empty
                // Base64 string length should be multiple of 4
                return false;
            }

            // replace pad chacters
            int lengthNoPadding = param.Length;
            int lengthPadding;

            param = param.TrimEnd(Base64Padding);
            lengthPadding = param.Length;

            if ((lengthNoPadding - lengthPadding) > 2)
            {
                // there should be no more than 2 pad characters
                return false;
            }

            foreach (char c in param)
            {
                if (Base64Characters.Contains(c) == false)
                {
                    // string contains non-Base64 character
                    return false;
                }
            }
            // nothing invalid found
            return true;
        }

        public static string ToGBString(double value)
        {
            return value.ToString(CultureInfo.GetCultureInfo("en-GB"));
        }

        /// <summary>
        /// Pega o número de minutos desde 1970.
        /// /// desde 1970
        /// </summary>
        /// <returns></returns>
        public static long GetTotalMinutes()
        {
            return Convert.ToInt64(DateTime.UtcNow.Subtract(EPOCH_DATE).TotalMinutes);
        }

        /// <summary>
        /// Retorna os minutos da data no formato java desde 1970.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetTotalMinutes(DateTime date)
        {
            return Convert.ToInt64(date.Subtract(EPOCH_DATE).TotalMinutes);
        }

        /// <summary>
        /// Retorna os milisegundos da data.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetTotalMillis(DateTime date)
        {
            return Convert.ToInt64(date.Subtract(EPOCH_DATE).TotalMilliseconds);
        }

        /// <summary>
        /// Calcula a data dos millisegundos utilizando a data 01/01/1970 como base.
        /// </summary>
        /// <returns></returns>
        public static DateTime FromJavaTime(long millis)
        {
            return EPOCH_DATE.ToUniversalTime().AddMilliseconds(millis);
        }

        /// <summary>
        /// Calcula a data dos minutos utilizando a data 01/01/1970 como base.
        /// </summary>
        /// <returns></returns>
        public static DateTime FromJavaTimeMinuts(long minuts)
        {
            return EPOCH_DATE.ToUniversalTime().AddMinutes(minuts);
        }

        /// <summary>
        /// Formato de data para consultas sql.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateTimeToQueryFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Calcula se a data do feed é mais recente que a última data de execução das threads.
        /// </summary>
        /// <param name="feedDate"></param>
        /// <param name="lastExecutionDate"></param>
        /// <returns></returns>
        public static bool CheckFeedDate(string feedDate, DateTime lastFeedDate)
        {
            DateTime pubDate;
            if (DateTime.TryParse(feedDate, out pubDate))
            {
                pubDate = pubDate.ToUniversalTime();
                long pubDateMilli = GetTotalMillis(pubDate);
                long lastExecutionMilli = GetTotalMillis(lastFeedDate);

                if (pubDateMilli > lastExecutionMilli)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Retorna a quantidade por dias,
        /// caso o dia não esteja no array counts retorna 0 para o dia correspondente.
        /// </summary>
        /// <returns></returns>
        public static int[] ValuesOrZero(int[] counts, int[] titles)
        {
            int[] days = GetLabelNumDays();

            List<int> result = new List<int>();
            for (int i = 0; i < days.Count(); i++)
            {
                if (titles.Contains(days[i]))
                {
                    for (int y = 0; y < titles.Count(); y++)
                    {
                        if (titles[y] == days[i])
                        {
                            result.Add(counts[y]);
                        }
                    }
                }
                else
                {
                    result.Add(0);
                }

            }
            return result.ToArray();
        }

        /// <summary>
        /// Retorna a quantidade por dias, somando com a quantidade dos dias anteriores.
        /// </summary>
        /// <returns></returns>
        public static int[] ValuesOrLast(int[] counts, int[] titles, int totalReturn)
        {
            int[] days = GetLabelNumDays();

            List<int> result = new List<int>();
            for (int i = 0; i < days.Count(); i++)
            {
                if (titles.Contains(days[i]))
                {
                    for (int y = 0; y < titles.Count(); y++)
                    {
                        if (titles[y] == days[i])
                        {
                            if (result.Any())
                                result.Add(counts[y] + result.Last());
                            else
                                result.Add(counts[y] + totalReturn);
                        }
                    }
                }
                else
                {
                    if (result.Any())
                        result.Add(result.Last());
                    else
                        result.Add(totalReturn);
                }

            }
            return result.ToArray();
        }

        /// <summary>
        /// Retorno um array com os dias a serem usados nos gráficos.
        /// </summary>
        /// <returns></returns>
        public static int[] GetLabelNumDays()
        {
            int year = DateTime.Now.Year;
            int month = (DateTime.Now.Month - 1) == 0 ? 12 : DateTime.Now.Month - 1;
            int day = DateTime.Now.Day;

            DateTime ultimoDia = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int beforeDay = ultimoDia.Day;

            int resultDay = 30 - day;
            int startDay = resultDay < 0 ? 2 : resultDay == 0 ? 1 : (beforeDay - resultDay + 1);

            List<int> result = new List<int>();
            if (startDay > 2)
            {
                for (int i = startDay; i <= beforeDay; i++)
                {
                    result.Add(i);
                }

                for (int i = 1; i <= day; i++)
                {
                    result.Add(i);
                }
            }
            else
            {
                for (int i = startDay; i <= day; i++)
                {
                    result.Add(i);
                }
            }

            return result.ToArray();
        }

        public static string GetDaysIn()
        {
            string days = "";
            int[] resultDays = GetLabelNumDays();
            for (int i = 0; i < resultDays.Count(); i++)
            {
                days = days + resultDays[i];
                if (i < (resultDays.Count() - 1))
                {
                    days = days + ",";
                }
            }
            return days;
        }

        public static string GetMonthsIn()
        {
            string months = "";

            if (DateTime.Now.Day > 30)
            {
                months = Convert.ToString(DateTime.Now.Month);
            }
            else
            {
                months = Convert.ToString((DateTime.Now.Month - 1)) + "," + Convert.ToString(DateTime.Now.Month);
            }

            return months;
        }

        public static string GetYearsIn()
        {
            string years = "";

            if (DateTime.Now.Month == 1 && DateTime.Now.Day < 30)
            {
                years = Convert.ToString((DateTime.Now.Year - 1)) + "," + Convert.ToString(DateTime.Now.Year);
            }
            else
            {
                years = Convert.ToString(DateTime.Now.Year);
            }

            return years;
        }

        /// <summary>
        /// Valida de a string informada é uma data. Retorno boolean.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool ValidateDate(string date)
        {
            DateTime pubDate;
            if (DateTime.TryParse(date, out pubDate))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Valida se a string informada é uma referência para uma cultura.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ValidateCulture(string culture)
        {
            if (culture.Contains("pt"))
                return "-pt";
            else if (culture.Contains("en"))
                return "-en";
            else if (culture.Contains("es"))
                return "-es";

            return string.Empty;
        }

        /// <summary>
        /// Formata o telefone do brazil retirando o 0 inicial do DDD
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static string FormatBrazilianPhone(string phoneNumber)
        {
            string phoneNumberFormatterd = phoneNumber;

            if (phoneNumberFormatterd != null && phoneNumberFormatterd.StartsWith("550"))
            {
                phoneNumberFormatterd = "55" + phoneNumberFormatterd.Substring(3);
            }

            return phoneNumberFormatterd;
        }

    }
}
