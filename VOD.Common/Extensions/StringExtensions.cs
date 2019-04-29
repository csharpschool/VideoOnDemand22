namespace VOD.Common.Extensions
{
    public static class StringExtensions
    {
        #region String Checking
        public static bool IsNullOrEmptyOrWhiteSpace(this string[] values)
        {
            foreach (var val in values)
                if (string.IsNullOrEmpty(val) ||
                    string.IsNullOrWhiteSpace(val))
                    return true;

            return false;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) ||
                   string.IsNullOrWhiteSpace(value);
        }
        #endregion

        #region String Manipulation
        public static string Truncate(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Length <= length) return value;

            return $"{value.Substring(0, length)} ...";
        }
        #endregion
    }
}
