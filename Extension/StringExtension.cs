namespace BinarySearch.Extension;

public static class StringExtension
{
    public static byte[] ToHexArray(this string hex)
    {
        hex = hex.Replace(" ", "");
        var bytes = new byte[hex.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }

        return bytes;
    }
}