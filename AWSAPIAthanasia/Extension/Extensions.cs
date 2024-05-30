using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiAthanasia.Extension
{
    public static class Extensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
        public static string Limpiar(this string cadena)
        {

            string limpio = cadena.Normalize(NormalizationForm.FormD).ToLower().Trim();
            limpio = limpio.Replace("á", "a")
                  .Replace("é", "e")
                  .Replace("í", "i")
                  .Replace("ó", "o")
                  .Replace("ú", "u")
                  .Replace("ñ", "n")
                  .Replace("ü", "u")
                  .Replace("ä", "a")
                  .Replace("ö", "o")
                  .Replace("ü", "u")
                  .Replace("ß", "ss")
                  .Replace(" ", "").ToUpper();
            limpio = Regex.Replace(limpio, @"[^A-Z0-9\s]", "");
            return limpio;
        }


    }
}
