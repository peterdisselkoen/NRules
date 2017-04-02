﻿using System.Linq;
using NRules.IntegrationTests.TestAssets;
using NUnit.Framework;

namespace NRules.IntegrationTests
{
    [TestFixture]
    public class OneFactRetractingRuleTest : BaseRuleTestFixture
    {
        [Test]
        public void Fire_OneMatchingFact_FiresOnceAndRetractsFact()
        {
            //Arrange
            var fact = new FactType {TestProperty = "Valid Value 1"};
            Session.Insert(fact);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
            Assert.AreEqual(0, Session.Query<FactType>().Count());
        }
        
        protected override void SetUpRules()
        {
            SetUpRule<TestRule>();
        }

        public class FactType
        {
            public string TestProperty { get; set; }
            public int TestCount { get; set; }

            public void IncrementCount()
            {
                TestCount++;
            }
        }

        public class TestRule : BaseRule
        {
            public override void Define()
            {
                FactType fact = null;

                When()
                    .Match<FactType>(() => fact, f => f.TestProperty.StartsWith("Valid"));
                Then()
                    .Do(ctx => ctx.Retract(fact))
                    .Do(ctx => Action(ctx));
            }
        }
    }
}