using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OracleCallTranslator;
using OracleCallTranslator.Entities;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OracleDbCallerTests
{
    public class RequestTranslatorTests
    {
        [Fact]
        public void TestGreaterThan()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.HeightInCm > 200);

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE HeightInCm > 200", result);
        }

        [Fact]
        public void TestLesserThan()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.HeightInCm < 200);

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE HeightInCm < 200", result);
        }

        [Fact]
        public void TestEquals()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.FirstName == "Arvydas");

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE FirstName = 'Arvydas'", result);
        }

        [Fact]
        public void TestAndOperatorWithString()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.FirstName == "Arvydas" && e.LastName == "Sabonis");

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE FirstName = 'Arvydas' AND LastName = 'Sabonis'", result);

        }

        [Fact]
        public void TestAndOperatorWithNumeric()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.HeightInCm < 200 && e.TeamId > 5);

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE HeightInCm < 200 AND TeamId > 5", result);
        }

        [Fact]
        public void TestAndOperatorWithMixed()
        {
            var translator = new ExpressionToOracleTranslator();

            Expression<Func<IQueryable<Player>, IQueryable<Player>>> expression
                = query => query.Where(e => e.FirstName == "Arvydas" && e.HeightInCm > 200 && e.Id > 5);

            var result = translator.Translate(expression);
            Assert.AreEqual("SELECT * FROM Player WHERE FirstName = 'Arvydas' AND HeightInCm > 200 AND Id > 5", result);
        }
    }
}
