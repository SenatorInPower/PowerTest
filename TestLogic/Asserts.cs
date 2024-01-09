using System;
namespace PowerTest
{
    public class Asserts
    {
        public static void IsNotNull(object obj, string message = "")
        {
            if (obj == null)
                throw new AssertionException("Assert.IsNotNull failed. " + message);
        }

        public static void IsNull(object obj, string message = "")
        {
            if (obj != null)
                throw new AssertionException("Assert.IsNull failed. " + message);
        }

        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
                throw new AssertionException("Assert.IsTrue failed. " + message);
        }

        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
                throw new AssertionException("Assert.IsFalse failed. " + message);
        }

        public static void AreEqual(object actual, object expected, string message = "")
        {
            if (!Equals(actual, expected))
                throw new AssertionException($"Assert.AreEqual failed. Expected: {expected}, Actual: {actual}. {message}");
        }

        public static void AreNotEqual(object actual, object expected, string message = "")
        {
            if (Equals(actual, expected))
                throw new AssertionException($"Assert.AreNotEqual failed. Expected not to be: {expected}, but was: {actual}. {message}");
        }

        public static void IsInstanceOfType(object obj, Type expectedType, string message = "")
        {
            if (obj == null || !expectedType.IsInstanceOfType(obj))
                throw new AssertionException($"Assert.IsInstanceOfType failed. Expected type: {expectedType}, Actual type: {obj?.GetType()}. {message}");
        }

        public static void IsNotInstanceOfType(object obj, Type wrongType, string message = "")
        {
            if (obj != null && wrongType.IsInstanceOfType(obj))
                throw new AssertionException($"Assert.IsNotInstanceOfType failed. Wrong type: {wrongType}, Actual type: {obj.GetType()}. {message}");
        }

        public static void Throws<TException>(Action action, string message = "") where TException : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (TException)
            {
                return;
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Assert.Throws failed. Expected exception: {typeof(TException)}, Actual exception: {ex.GetType()}. {message}");
            }
            throw new AssertionException($"Assert.Throws failed. Expected exception: {typeof(TException)}, but no exception was thrown. {message}");
        }

        public static void DoesNotThrow(Action action, string message = "")
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Assert.DoesNotThrow failed. Unexpected exception: {ex.GetType()}. {message}");
            }
        }

        public static void GreaterThan<T>(T actual, T expected, string message = "") where T : IComparable
        {
            if (actual.CompareTo(expected) <= 0)
                throw new AssertionException($"Assert.GreaterThan failed. Expected: {expected} to be greater than Actual: {actual}. {message}");
        }


        public static void GreaterThanOrEqual<T>(T actual, T expected, string message = "") where T : IComparable
        {
            if (actual.CompareTo(expected) < 0)
                throw new AssertionException($"Assert.GreaterThanOrEqual failed. Expected: {expected} to be greater than or equal to Actual: {actual}. {message}");
        }


        public static void LessThan<T>(T actual, T expected, string message = "") where T : IComparable
        {
            if (actual.CompareTo(expected) >= 0)
                throw new AssertionException($"Assert.LessThan failed. Expected: {expected} to be less than Actual: {actual}. {message}");
        }


        public static void LessThanOrEqual<T>(T actual, T expected, string message = "") where T : IComparable
        {
            if (actual.CompareTo(expected) > 0)
                throw new AssertionException($"Assert.LessThanOrEqual failed. Expected: {expected} to be less than or equal to Actual: {actual}. {message}");
        }


        public static void InRange<T>(T value, T min, T max, string message = "") where T : IComparable
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
                throw new AssertionException($"Assert.InRange failed. Value: {value} is not in range {min}-{max}. {message}");
        }

        // ... Другие методы по мере необходимости
    }

    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }

}