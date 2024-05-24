using System.Text;

namespace Data.WebClient.Helpers
{
    public static class AuthHelper
    {
        public static string GetBasicAuthToken(string login, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");

            return Convert.ToBase64String(byteArray);
        }
    }
}
