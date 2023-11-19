using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace RSA
{
    public struct PublicKey
    {
        public BigInteger N { get; set; }
        public BigInteger pubKey { get; set; }

        public PublicKey(BigInteger N, BigInteger pk)
        {
            this.N = N;
            pubKey = pk;
        }
    }
}