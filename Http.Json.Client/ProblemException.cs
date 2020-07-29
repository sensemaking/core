using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Sensemaking.Http.Json.Client
{
    public class ProblemException : Exception
    {
        internal ProblemException(HttpStatusCode status, IEnumerable<(string, string)> headers, Problem problem) :
            base($"A problem has occured while making an http request:{Environment.NewLine}" +
                 $"\tStatus: {status}{Environment.NewLine}" +
                 $"\tProblem: {problem.Title}{Environment.NewLine}" +
                 string.Join($"{Environment.NewLine}\t", problem.Errors) + Environment.NewLine)
        {
            Status = status;
            Headers = headers.ToArray();
            Problem = problem;
        }

        public HttpStatusCode Status { get; }
        public Problem Problem { get; }
        public (string Name, string Value)[] Headers { get; }
        public bool HasProblem() => Problem != Problem.Empty;
    }
}