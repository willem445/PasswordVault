﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordVault;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Tests
{
    [TestClass()]
    public class CsvDatabaseTests
    {
        private const string BASE_PATH = @"..\..\..\PasswordVault\CSV\";
        private const string USERS_CSV_PATH = BASE_PATH + @"usersUT.csv";
        private const string PASSWORDS_CSV_PATH = BASE_PATH + @"passwordsUT.csv";

        [TestMethod()]
        public void AddUserTest()
        {
            CsvDatabaseFactory csvDatabaseFactory = new CsvDatabaseFactory();
            IDatabase db = csvDatabaseFactory.Get();
            ((CsvDatabase)db).UsersCsvPathOverride = USERS_CSV_PATH;
            ((CsvDatabase)db).PasswordCsvPathOverride = PASSWORDS_CSV_PATH;
            List<Password> result = db.GetUserPasswords("willem445");
            Assert.AreEqual(20, result.Count);           
        }
    }
}

/*=================================================================================================
DESCRIPTION
*================================================================================================*/
/* 
 ------------------------------------------------------------------------------------------------*/