using System;

namespace Sensemaking.Http.Json.Client
{
    public class ProblemException : Exception
    {
        public ProblemException(HttpStatus status, Problem problem) : base("A problem has occured while making an http request.")
        {
            Status = status;
            Problem = problem;
        }

        public HttpStatus Status { get; }
        public Problem Problem { get; }
        public bool HasProblem() => Problem != Problem.Empty;
    }
}