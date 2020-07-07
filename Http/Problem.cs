namespace Sensemaking.Http
{
    public readonly struct Problem
    {
        public Problem(string title, params string[] errors)
        {
            Title = title;
            Errors = errors;
        }

        public string Title { get; }
        public string[] Errors { get; }
    }
}