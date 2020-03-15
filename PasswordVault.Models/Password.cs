﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace PasswordVault.Models
{
    public enum PasswordFilterOption
    {
        Application = 0,
        Description = 1,
        Website = 2,
    }

    public class Password : IEquatable<Password>
    {
        [Browsable(false)]
        public Int64 UniqueID { get; set; }

        [Browsable(true)]
        public string Application { get; set; }

        [Browsable(true)]
        public string Username { get; set; }

        [Browsable(true)]
        public string Email { get; set; }

        [Browsable(true)]
        public string Description { get; set; }

        [Browsable(true)]
        public string Website { get; set; }

        [Browsable(false)]
        public string Passphrase { get; set; }

        public Password()
        {

        }

        public Password(string application, string username, string email, string description, string website, string passphrase)
        {
            Passphrase = passphrase;
            Application = application;
            Username = username;
            Email = email;
            Description = description;
            Website = website;
        }

        public Password(Int64 uniqueID, string application, string username, string email, string description, string website, string passphrase)
        {
            UniqueID = uniqueID;
            Passphrase = passphrase;
            Application = application;
            Username = username;
            Email = email;
            Description = description;
            Website = website;
        }

        public virtual string GetPasswordString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0},{1},{2},{3},{4},{5}", Application, Username, Email, Description, Website, Passphrase);
        }

        public bool Equals(Password other)
        {
            bool result = false;

            if (other == null)
            {
                return false;
            }

            if (other.Username == this.Username &&
                other.Application == this.Application &&
                other.Email == this.Email &&
                other.Website == this.Website &&
                other.Passphrase == this.Passphrase &&
                other.Description == this.Description)
            {
                result = true;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            if (!(obj is Password))
                return false;

            Password mys = (Password)obj;

            if (mys.Username == this.Username &&
                mys.Application == this.Application &&
                mys.Email == this.Email &&
                mys.Website == this.Website &&
                mys.Passphrase == this.Passphrase &&
                mys.Description == this.Description)
            {
                result = true;
            }

            return result;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            // Suitable nullity checks etc, of course :)
            hash = hash * 23 + Username.GetHashCode();
            hash = hash * 23 + Application.GetHashCode();
            hash = hash * 23 + Email.GetHashCode();
            hash = hash * 23 + Website.GetHashCode();
            hash = hash * 23 + Passphrase.GetHashCode();
            hash = hash * 23 + Description.GetHashCode();
            return hash;
        }
    } // Password CLASS
}
