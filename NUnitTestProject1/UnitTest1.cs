using MinimalCache;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class T1
    {
        public string G { get; private set; }
        public T1(string g)
        {
            G = g;
        }
    }

    public class Tests
    {

        Cache Cache { get; set; }
        [SetUp]
        public void Setup()
        {
            Cache = new Cache();
        }

        [Test]
        public void TestCreateInstance()
        {
            var k = Guid.NewGuid().ToString();
            var t = Cache.CreateInstance<T1>(k, k);
            Assert.That(t, Is.InstanceOf(t.GetType()));
        }

        [Test]
        public void TestAdd()
        {
            var list = new List<T1>();
            for(int i =0; i < 100000; i++)
            {
                var k = Guid.NewGuid().ToString();
                list.Add(Cache.CreateInstance<T1>(k,k));
            }            
        }

        [Test]
        public void TestRemove()
        {
            var list = new List<T1>();
            for (int i = 0; i < 100000; i++)
            {
                var k = Guid.NewGuid().ToString();
                list.Add(Cache.CreateInstance<T1>(k,k));
            }
            foreach (var item in list)
            {
                Cache.Remove<T1>(item.G);
            }
            for (int i = 0; i < 10; i++)
            {
                Assert.Throws<InvalidOperationException>(() => Cache.Remove<T1>(list[i].G));
            }            
        }

        [Test]
        public void TestGet()
        {
            var list = new List<T1>();
            var list2 = new List<T2>();
            for (int i = 0; i < 100000; i++)
            {
                var k = Guid.NewGuid().ToString();
                var k2 = Guid.NewGuid().ToString();
                list.Add(Cache.CreateInstance<T1>(k,k));
                list2.Add(Cache.CreateInstance<T2>(k2,k2));                
            }            

            for (int i = 0; i < 100000; i++)
            {
                var item1 = Cache.Get<T1>(list[i].G);
                Assert.IsInstanceOf(typeof(T1), item1);
            }
            for (int i = 0; i < 100000; i++)
            {
                var item2 = Cache.Get<T2>(list2[i].G);
                Assert.IsInstanceOf(typeof(T2), item2);
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }

    internal class T2 : T1
    {
        public T2(string G) : base(G)
        {

        }
    }
}