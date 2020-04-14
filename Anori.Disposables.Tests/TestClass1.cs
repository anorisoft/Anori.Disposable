using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests1
{
    using Anori.Disposables;

    [TestFixture]
    public class SingleSetTests
    {
        [Test]
        public void SingleSet_NewSingleSet()
        {
            var singleSet = new SingleSet();
            Assert.False(singleSet.Value, "Single set is set!");
            Assert.False(singleSet.Set(), "Single set is set!");
            Assert.True(singleSet.Value, "Single not set is set!");
            Assert.True(singleSet.Set(), "Single not set is set!");
            Assert.True(singleSet.Value, "Single not set is set!");
            Assert.True(singleSet.Set(), "Single not set is set!");
        }
    }
}