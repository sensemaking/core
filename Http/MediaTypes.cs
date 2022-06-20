namespace Sensemaking.Http
{
    public static class MediaType
    {
        public const string Html = "text/html";
        public const string Json = "application/json";
        public const string Siren = "application/vnd.siren+json";
        public const string JsonProblem = "application/problem+json";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
    }

    public static class Charset
    {
        public const string Utf8 = "charset=UTF-8";
        public const string Utf16 = "charset=UTF-16";
    }
}