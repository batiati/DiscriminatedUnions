using System;

namespace System
{
	public struct OneOf<T1, T2> : IEquatable<OneOf<T1, T2>>
	{
		#region Fields

		private readonly object value;

		#endregion Fields

		#region Properties

		public object Value => value;

		#endregion Properties

		#region Constructors
		public OneOf(T1 value)
		{
			this.value = value;
		}

		public OneOf(T2 value)
		{
			this.value = value;
		}

		#endregion Constructors

		#region Methods

		public bool TryGet(out T1 value)
		{
			bool success;
			(value, success) = this.value is T1 _value ? (_value, true) : (default(T1), false);

			return success;
		}

		public bool TryGet(out T2 value)
		{
			bool success;
			(value, success) = this.value is T2 _value ? (_value, true) : (default(T2), false);

			return success;
		}

		public bool Equals(OneOf<T1, T2> other)
		{
			return object.Equals(value, other.value);
		}

		public bool Equals(T1 other)
		{
			return object.Equals(value, other);
		}

		public bool Equals(T2 other)
		{
			return object.Equals(value, other);
		}

		public override bool Equals(object obj)
		{
			return object.Equals(value, obj);
		}

		public override int GetHashCode()
		{
			return value?.GetHashCode() ?? 0;
		}

		public static implicit operator T1(OneOf<T1, T2> union)
		{
			return (T1)union.value;
		}

		public static implicit operator OneOf<T1, T2>(T1 value)
		{
			return new OneOf<T1, T2>(value);
		}

		public static implicit operator T2(OneOf<T1, T2> union)
		{
			return (T2)union.value;
		}

		public static implicit operator OneOf<T1, T2>(T2 value)
		{
			return new OneOf<T1, T2>(value);
		}

		public static bool operator ==(OneOf<T1, T2> a, OneOf<T1, T2> b)
		{
			return object.Equals(a.value, b.value);
		}

		public static bool operator !=(OneOf<T1, T2> a, OneOf<T1, T2> b)
		{
			return !object.Equals(a.value, b.value);
		}

		#endregion Methods
	}
}
