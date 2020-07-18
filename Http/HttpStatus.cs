using System;
using System.Net;

namespace Sensemaking.Http
{
    public readonly struct HttpStatus
    {
        public HttpStatus(HttpStatusCode code, string reason)
        {
            Code = code;
            this.reason = reason;
        }

        public HttpStatusCode Code { get; }

        private readonly string reason;
        public string Reason => reason ?? string.Empty;

        #region Equality

        public static bool operator ==(HttpStatus @this, HttpStatus that)
        {
            return @this.Equals(that);
        }

        public static bool operator !=(HttpStatus @this, HttpStatus that)
        {
            return !@this.Equals(that);
        }

        public override bool Equals(object? that)
        {
            return that is HttpStatus status && Equals(status);
        }

        public bool Equals(HttpStatus that)
        {
            return this.Code == that.Code && this.Reason == that.Reason;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, Reason);
        }

        #endregion
    }
}