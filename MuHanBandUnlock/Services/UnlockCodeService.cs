using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace MuHanBandUnlock.Services
{
    public static class UnlockCodeService
    {
        public static string NormalizeMac(string mac)
        {
            if (string.IsNullOrEmpty(mac)) return string.Empty;
            var sb = new StringBuilder(mac.Length);
            foreach (var ch in mac)
            {
                if (ch == '：' || ch == ':' || ch == '-' || ch == ' ' || ch == '.') continue;
                sb.Append(char.ToUpperInvariant(ch));
            }
            return sb.ToString();
        }

        public static string NormalizeSn(string sn)
        {
            if (string.IsNullOrEmpty(sn)) return string.Empty;
            return sn.Trim().ToUpperInvariant();
        }

        public static string CalculateUnlockCode(string mac, string sn, bool newAlgorithm)
        {
            var m = NormalizeMac(mac);
            var s = NormalizeSn(sn);

            if (m.Length == 0 && s.Length == 0)
                throw new ArgumentException("MAC 和 SN 都不能为空");

            var input = newAlgorithm ? s + m + "XIAOMI" : m + s + "XIAOMI";

            var provider = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            var buffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var hashed = provider.HashData(buffer);

            byte[] raw;
            CryptographicBuffer.CopyToByteArray(hashed, out raw);

            var code = new StringBuilder(10);
            for (int i = 0; i < 10 && i < raw.Length; i++)
            {
                code.Append((raw[i] % 0xA).ToString());
            }
            return code.ToString();
        }
    }
}
