using System;
using System.Security.Cryptography;

namespace Slab.Elasticsearch
{
    public class UniqueIdGenerator
    {
        public string GenerateUniqueId()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var rndBytes = new byte[8];
                rng.GetBytes(rndBytes);
                return BitConverter.ToString(rndBytes).Replace("-", "");
            }
        }
    }
}
