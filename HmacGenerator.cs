using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace task3;

public class HmacGenerator
{
    public string Hmac { get; set; } 
    public string RandomKey { get; set; }
    private byte[] Key { get; set; } = new byte[32];
    
    public HmacGenerator(string computerMove)
    {
        RandomKey = GenerateRandomKey();
        Hmac = GenerateHmac(computerMove);
    }

    private string GenerateHmac(string computerMove)
    {
        IDigest sha3Digest = new Sha3Digest(256);
        HMac hmac = new HMac(sha3Digest);
        byte[] data = Encoding.UTF8.GetBytes(computerMove);
        hmac.Init(new KeyParameter(Key));
        hmac.BlockUpdate(data, 0, data.Length);
        byte[] hash = new byte[sha3Digest.GetDigestSize()];
        hmac.DoFinal(hash, 0);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    private string GenerateRandomKey()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(Key);
        return BitConverter.ToString(Key).Replace("-", string.Empty);
    }
}
