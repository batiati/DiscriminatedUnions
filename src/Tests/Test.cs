using System;
using NUnit.Framework;

namespace Tests
{
	class User
	{
		public int ID { get; set; }

		public string Name { get; set; }
	}

	public class Tests
	{
		[Test]
		public void IsValueTypeTest()
		{
			var value = new OneOf<int, string>(100);
			Assert.IsTrue(value.TryGet(out int i32));
			Assert.AreEqual(i32, 100);
		}

		[Test]
		public void IsNotValueTypeTest()
		{
			var value = new OneOf<int, long>(1000);
			Assert.IsFalse(value.TryGet(out long i64));
			Assert.AreNotEqual(i64, 1000);
		}

		[Test]
		public void ValueTypeEqualsTest()
		{
			var value1 = new OneOf<int, string>(100);
			var value2 = new OneOf<int, string>(100);

			Assert.AreEqual(value1, value2);
			Assert.AreEqual(value1, 100);
			Assert.AreEqual(value2, 100);
		}

		[Test]
		public void ValueTypeCastTest()
		{
			int i32 = 100;
			OneOf<int, string> value1 = i32;
			int i32_b = value1;

			Assert.AreEqual(value1, i32);
			Assert.AreEqual(value1, i32_b);
			Assert.AreEqual(i32, i32_b);
		}

		[Test]
		public void IsRefTypeTest()
		{
			var value = new OneOf<int, string>("hello");
			Assert.IsTrue(value.TryGet(out string hello));
			Assert.AreEqual(hello, "hello");
		}

		[Test]
		public void IsNotRefTypeTest()
		{
			var value = new OneOf<Exception, string>(new Exception("hello"));
			Assert.IsFalse(value.TryGet(out string hello));
			Assert.AreNotEqual(hello, "hello");
		}

		[Test]
		public void RefTypeEqualsTest()
		{
			var value1 = new OneOf<string, Exception>("hello");
			var value2 = new OneOf<string, Exception>("hello");

			Assert.AreEqual(value1, value2);
			Assert.AreEqual(value1, "hello");
			Assert.AreEqual(value2, "hello");
		}

		[Test]
		public void RefTypeCastTest()
		{
			string str = "hello";
			OneOf<int, string> value1 = str;
			string str_b = value1;

			Assert.AreEqual(value1, str);
			Assert.AreEqual(value1, str_b);
			Assert.AreEqual(str, str_b);
		}

		[Test]
		public void IsBothByInheritanceTest()
		{
			var value = new OneOf<object, string>("hello");
			
			Assert.IsTrue(value.TryGet(out string hello));
			Assert.AreEqual(hello, "hello");

			Assert.IsTrue(value.TryGet(out object obj));
			Assert.AreEqual(obj, "hello");
		}

		[Test]
		public void ReturnValueTest()
		{
			OneOf<int, string> divideBy(int a, int b)
			{
				if (b == 0) return "Cannot divide by zero";
				return a / b;
			}

			var ret1 = divideBy(10, 0);
			Assert.IsTrue(ret1.TryGet(out string _));
			Assert.AreEqual(ret1, "Cannot divide by zero");

			var ret2 = divideBy(10, 2);
			Assert.IsTrue(ret2.TryGet(out int _));
			Assert.AreEqual(ret2, 5);
		}

		[Test]
		public void ArgumentTest()
		{
			OneOf<int, string> sum(OneOf<int, string> a, OneOf<int, string> b)
			{
				if (a.TryGet(out int aI32))
				{
					if (b.TryGet(out int bI32))
					{
						return aI32 + bI32;
					}
					else if (b.TryGet(out string bStr))
					{
						return string.Concat(aI32.ToString(), bStr);
					}
				}
				else if (a.TryGet(out string aStr))
				{
					if (b.TryGet(out int bI32))
					{
						return string.Concat(aStr, bI32.ToString());
					}
					else if (b.TryGet(out string bStr))
					{
						return string.Concat(aStr, bStr);
					}
				}

				throw new NotImplementedException();
			}

			var ret1 = sum(10, 10);
			Assert.AreEqual(ret1, 20);

			var ret2 = sum("10", 10);
			Assert.AreEqual(ret2, "1010");

			var ret3 = sum(10, "10");
			Assert.AreEqual(ret3, "1010");

			var ret4 = sum("10", "10");
			Assert.AreEqual(ret4, "1010");
		}

		[Test]
		public void NullValueTest()
		{
			var value = new OneOf<string, User>((string)null);
			Assert.IsFalse(value.TryGet(out string str));
			Assert.IsFalse(value.TryGet(out User user));
		}
	}
}