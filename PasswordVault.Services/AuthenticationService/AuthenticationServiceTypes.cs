using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;

namespace PasswordVault.Services
{
    public enum AuthenticateResult
    {
        PasswordIncorrect,
        UsernameDoesNotExist,
        Successful,
        Failed
    }

    public struct AuthenticateReturn : IEquatable<AuthenticateReturn>
    {
        public AuthenticateReturn(AuthenticateResult result, User user)
        {
            Result = result;
            User = user;
        }

        public AuthenticateResult Result { get; }
        public User User { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is AuthenticateReturn))
                return false;

            AuthenticateReturn mys = (AuthenticateReturn)obj;

            if (mys.Result != Result)
                return false;

            if (mys.User != User)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + Result.GetHashCode();
            hash = hash * 23 + User.GetHashCode();
            return hash;
        }

        public static bool operator ==(AuthenticateReturn left, AuthenticateReturn right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AuthenticateReturn left, AuthenticateReturn right)
        {
            return !(left == right);
        }

        public bool Equals(AuthenticateReturn other)
        {
            AuthenticateReturn mys = (AuthenticateReturn)other;

            if (mys.Result != Result)
                return false;

            if (mys.User != User)
                return false;

            return true;
        }
    }
}
