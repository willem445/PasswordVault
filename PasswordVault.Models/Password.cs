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

    public class Password
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


        //public static explicit operator Password(DataGridViewRow dr)
        //{
        //    Password p = new Password();
        //    p.Application = dr.Cells[0].Value.ToString();
        //    p.Username = dr.Cells[1].Value.ToString();
        //    p.Email = dr.Cells[2].Value.ToString();
        //    p.Description = dr.Cells[3].Value.ToString();
        //    p.Website = dr.Cells[4].Value.ToString();
        //    return p;
        //}

    } // Password CLASS
}
