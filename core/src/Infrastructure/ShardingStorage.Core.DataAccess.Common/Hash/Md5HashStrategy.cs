using System.Security.Cryptography;
using System.Text;

namespace ShardingStorage.Core.DataAccess.Common.Hash;

public class Md5HashStrategy : IHashStrategy
{
    public int ComputeHash(string s)
    {
        MD5 md5Hasher = MD5.Create();
        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(s));
        return BitConverter.ToInt32(hashed, 0);
    }
}