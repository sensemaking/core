using System;
using System.Net;

namespace Sensemaking.Http
{
    public readonly struct HttpStatus
    {
        public HttpStatus(HttpStatusCode code, string reason)
        {
            Code = code;
            Reason = reason;
        }

        public HttpStatusCode Code { get; }
        public string Reason { get; }


        #region Equality

        public static bool operator ==(HttpStatus @this, HttpStatus that)
        {
            return @this.Equals(that);
        }

        public static bool operator !=(HttpStatus @this, HttpStatus that)
        {
            return !@this.Equals(that);
        }

        public override bool Equals(object? obj)
        {
            return obj is HttpStatus other && Equals(other);
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