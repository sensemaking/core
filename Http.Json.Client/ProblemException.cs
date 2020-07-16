using System;

namespace Sensemaking.Http.Json.Client
{
    public class ProblemException : Exception
    {
        public ProblemException(HttpStatus httpStatus, Problem problem) : base("A problem has occured while making an http request.")
        {
            HttpStatus = httpStatus;
            Problem = problem;
        }

        public HttpStatus HttpStatus { get; }
        public Problem Problem { get; }
        public bool HasProblem() => Problem != Problem.Empty;
    }
}