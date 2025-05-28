using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class FairRandomGenerator
    {
        private readonly byte[] _key;
        private readonly byte[] _hmac;
        private readonly int _computerNumber;

        public FairRandomGenerator(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue");

            _key = new byte[32];
            new SecureRandom().NextBytes(_key);

            _computerNumber = GetSecureRandomInRange(minValue, maxValue);

            _hmac = ComputeHmacSha3(_key, BitConverter.GetBytes(_computerNumber));
        }

        public string Hmac => BitConverter.ToString(_hmac).Replace("-", "");
        public string Key => BitConverter.ToString(_key).Replace("-", "");
        public int ComputerNumber => _computerNumber;

        private static byte[] ComputeHmacSha3(byte[] key, byte[] data)
        {
            var digest = new Sha3Digest(256);
            var hmac = new HMac(digest);
            hmac.Init(new KeyParameter(key));
            hmac.BlockUpdate(data, 0, data.Length);

            byte[] result = new byte[hmac.GetMacSize()];
            hmac.DoFinal(result, 0);
            return result;
        }

        public static int GetSecureRandomInRange(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue");

            using var rng = RandomNumberGenerator.Create();
            uint range = (uint)(maxValue - minValue);
            uint max = uint.MaxValue - uint.MaxValue % range;
            uint value;

            do
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                value = BitConverter.ToUInt32(bytes, 0);
            } while (value > max);

            return (int)(minValue + (value % range));
        }

        public static int CalculateFairResult(int computerNumber, int userNumber, int modulo)
        {
            return (computerNumber + userNumber) % modulo;
        }
    }
}
