namespace Foundation.Extensions.ExtensionMethods
{
    public static class BooleanExtensions
    {
        public static string ToYesOrNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
