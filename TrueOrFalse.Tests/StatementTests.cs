using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Models;

namespace TrueOrFalse.Tests
{
    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void EqualStatementShouldBeEqual()
        {
            Statement s1 = new Statement("abc", true);
            Statement s2 = new Statement("abc", true);

            Assert.AreEqual(s1, s2);
        }

        [Test]
        public void UnequalStatementsByFlagShouldNotBeEqual()
        {
            Statement s1 = new Statement("abc", true);
            Statement s2 = new Statement("abc", false);

            Assert.AreNotEqual(s1, s2);
        }

        [Test]
        public void UnequalStatementsByTextShouldNotBeEqual()
        {
            Statement s1 = new Statement("abcd", true);
            Statement s2 = new Statement("abc", true);

            Assert.AreNotEqual(s1, s2);
        }
    }
}
