using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PasswordVault.Models;

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/

namespace PasswordVault.Services
{
    public struct AddPasswordResult : IEquatable<AddPasswordResult>
    {
        public AddPasswordResult(ValidatePassword result, Int64 uniquePasswordID)
        {
            Result = result;
            UniquePasswordID = uniquePasswordID;
        }

        public ValidatePassword Result { get; }
        public Int64 UniquePasswordID { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is AddPasswordResult))
                return false;

            AddPasswordResult mys = (AddPasswordResult)obj;

            if (mys.Result != Result)
                return false;

            if (mys.UniquePasswordID != UniquePasswordID)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Result.GetHashCode();
                hash = hash * 23 + UniquePasswordID.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(AddPasswordResult left, AddPasswordResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AddPasswordResult left, AddPasswordResult right)
        {
            return !(left == right);
        }

        public bool Equals(AddPasswordResult other)
        {
            AddPasswordResult mys = (AddPasswordResult)other;

            if (mys.Result != Result)
                return false;

            if (mys.UniquePasswordID != UniquePasswordID)
                return false;

            return true;
        }
    }

    public enum DeletePasswordResult
    {
        PasswordDoesNotExist,
        Success,
        Failed
    }
} // PasswordVault.Services.Standard.PasswordService NAMESPACE
