// -----------------------------------------------------------------------
// <copyright file="TestClass.cs" company="bfa solutions ltd">
// Copyright (c) bfa solutions ltd. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NUnit.Tests1
{
    using System;
    using System.Windows;

    using NUnit.Framework;

    using WeakEvent;

    [TestFixture]
    public class TestClass
    {
        private void AddEvent(WeakReference<Class1> weak)
        {
            if (weak.TryGetTarget(out var c))
            {
                c.Event1 += this.COnEvent1;
            }
        }

        private void AddWeakEvent(WeakReference<Class1> weak)
        {
            if (weak.TryGetTarget(out var c))
            {
                WeakEventManager<Class1, EventArgs>.AddHandler(c, "Event1", this.COnEvent1);
            }
        }

        private void COnEvent1(object sender, EventArgs e)
        {
        }

        private WeakReference<Class1> NewWeakClass1()
        {
            return new WeakReference<Class1>(new Class1());
        }

        private WeakReference<Class2> NewWeakClass2()
        {
            return new WeakReference<Class2>(new Class2());
        }

        private static void AddEvent(WeakReference<Class1> weak1, WeakReference<Class2> weak2)
        {
            if (weak1.TryGetTarget(out var c1))
            {
                if (weak2.TryGetTarget(out var c2))
                {
                    c1.Event1 += c2.OnEvent1;
                }
            }
        }

        private static void AddEvent(Class1 c1, WeakReference<Class2> weak2)
        {
            if (weak2.TryGetTarget(out var c2))
            {
                c1.Event1 += c2.OnEvent1;
            }
        }

        private static void RemoveEvent(Class1 c1, WeakReference<Class2> weak2)
        {
            if (weak2.TryGetTarget(out var c2))
            {
                c1.Event1 -= c2.OnEvent1;
            }
        }

        private static Func<bool> AddWeakEvent(Class1 c1, WeakReference<Class2> weak2)
        {
            if (weak2.TryGetTarget(out var c2))
            {
                WeakEventManager<Class1, EventArgs>.AddHandler(c1, "Event1", c2.OnEvent1);

                return () =>
                    {
                        if (weak2.TryGetTarget(out var c))
                        {
                            WeakEventManager<Class1, EventArgs>.RemoveHandler(c1, "Event1", c.OnEvent1);
                            return true;
                        }

                        return false;
                    };
            }
            return () => false;
        }

        private static Action AddWeakEventBadRemove(Class1 c1, WeakReference<Class2> weak2)
        {
            if (weak2.TryGetTarget(out var c2))
            {
                WeakEventManager<Class1, EventArgs>.AddHandler(c1, "Event1", c2.OnEvent1);

                return () => { WeakEventManager<Class1, EventArgs>.RemoveHandler(c1, "Event1", c2.OnEvent1); };
            }

            return () => { };
        }

        private static void AddEvent(WeakReference<Class1> weak1, Class2 c2)
        {
            if (weak1.TryGetTarget(out var c1))
            {
                c1.Event1 += c2.OnEvent1;
            }
        }

        [Test]
        public void NewClass1AndEventTest()
        {
            var weak = this.NewWeakClass1();
            this.AddEvent(weak);
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            Assert.False(weak.TryGetTarget(out var c2));
        }

        [Test]
        public void NewClass1AndWeakClass2AndAddEventAndAddWeakEventAndRemoveTest()
        {
            var c1 = new Class1();
            Func<bool> remove;
            var weak2 = this.NewWeakClass2();
            {
                AddEvent(c1, weak2);
                remove = AddWeakEvent(c1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.True(IsAlive(weak2));
                Assert.True(remove());
            }

            RemoveEvent(c1, weak2);

            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.False(IsAlive(weak2));
                Assert.False(remove());
            }
        }

        private static bool IsAlive(WeakReference<Class2> weak2)
        {
            return weak2.TryGetTarget(out var c2);
        }

        [Test]
        public void NewClass1AndWeakClass2AndAddWeakEventAndBadRemoveTest()
        {
            var c1 = new Class1();
            var weak2 = this.NewWeakClass2();
            {
                var remove = AddWeakEventBadRemove(c1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.True(weak2.TryGetTarget(out var c2));
            }
        }

        [Test]
        public void NewClass1AndWeakClass2AndAddWeakEventAndRemoveTest()
        {
            var c1 = new Class1();
            Func<bool> remove;
            var weak2 = this.NewWeakClass2();
            {
                remove = AddWeakEvent(c1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.False(weak2.TryGetTarget(out var c2));
                Assert.False(remove());
            }
        }

        [Test]
        public void NewClass1AndWeakClass2AndEventTest()
        {
            var c1 = new Class1();
            var weak2 = this.NewWeakClass2();
            {
                AddEvent(c1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.True(weak2.TryGetTarget(out var c2));
            }
        }

        [Test]
        public void NewClass1AndWeakClass2AndWeakEventTest()
        {
            var c1 = new Class1();
            var weak2 = this.NewWeakClass2();
            {
                AddWeakEvent(c1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.False(weak2.TryGetTarget(out var c2));
            }
        }

        [Test]
        public void NewClass1AndWeakEventTest()
        {
            var weak = this.NewWeakClass1();
            this.AddWeakEvent(weak);
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            Assert.False(weak.TryGetTarget(out var c2));
        }

        [Test]
        public void NewClass1Test()
        {
            var weak = this.NewWeakClass1();
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            Assert.False(weak.TryGetTarget(out var c));
        }

        [Test]
        public void NewWeakClass1AndClass2AndEventTest()
        {
            var weak1 = this.NewWeakClass1();
            var c2 = new Class2();
            {
                AddEvent(weak1, c2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.False(weak1.TryGetTarget(out var c1));
            }
        }

        [Test]
        public void NewWeakClass1AndClass2Test()
        {
            var weak1 = this.NewWeakClass1();
            var weak2 = this.NewWeakClass1();
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            Assert.False(weak1.TryGetTarget(out var c1));
            Assert.False(weak2.TryGetTarget(out var c2));
        }

        [Test]
        public void NewWeakClass1AndWeakClass2AndEventTest()
        {
            var weak1 = this.NewWeakClass1();
            var weak2 = this.NewWeakClass2();
            {
                AddEvent(weak1, weak2);
            }
            GC.Collect(GC.MaxGeneration);
            GC.WaitForFullGCComplete();
            {
                Assert.False(weak1.TryGetTarget(out var c1));

                Assert.False(weak2.TryGetTarget(out var c2));
            }
        }
    }
}