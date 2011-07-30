﻿using System;
using Microsoft.FSharp.Core;
using NUnit.Framework;

namespace FSharp.Core.CS.Tests {
    [TestFixture]
    public class IntegrationTests {
        int doSomething(int userID, int id) {
            // fetch some other entity, do "stuff"
            return userID + id;
        }

        void setError(string e) {}

        const string req_userID = "123";
        const string req_otherID = "999";

        [Test]
        public void Test1_Imperative() {

            int userID;
            var userID_ok = int.TryParse(req_userID, out userID);
            if (!userID_ok) {
                setError("Invalid User ID");
            } else {
                int id;
                var id_ok = int.TryParse(req_otherID, out id);
                if (!id_ok) {
                    setError("Invalid ID");
                } else {
                    Console.WriteLine(doSomething(userID, id));
                }
            }
        }

        [Test]
        public void Test1_option() {
            var userID = FSharpOption.TryParseInt(req_userID);
            if (!userID.HasValue()) {
                setError("Invalid User ID");
            } else {
                var otherID = FSharpOption.TryParseInt(req_otherID);
                if (!otherID.HasValue()) {
                    setError("Invalid ID");
                } else {
                    Console.WriteLine(doSomething(userID.Value, otherID.Value));
                }
            }
        }


        [Test]
        public void Test1_either() {

            var somethingOrError =
                from userID in FSharpOption.TryParseInt(req_userID).ToFSharpChoice("Invalid User ID")
                from id in FSharpOption.TryParseInt(req_otherID).ToFSharpChoice("Invalid ID")
                select doSomething(userID, id);

            somethingOrError.Match(Console.WriteLine, setError);

        }
    }
}