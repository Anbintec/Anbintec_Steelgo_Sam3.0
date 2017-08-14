using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SecurityManager.TokenHandler
{
    public class SecurityConstants
    {
        public static readonly byte[] KeyForHmacSha256 = new byte[64];

        public static readonly string TokenIssuer = string.Empty;

        public static readonly string TokenAudience = string.Empty;

        public static readonly double TokenLifetimeMinutes = 30;

        static SecurityConstants()
        {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            cryptoProvider.GetNonZeroBytes(KeyForHmacSha256);   //Secure enough? Will change on every call. Has to be made a constant.

            TokenIssuer = "issuer"; //What should be a good value here? web api url?

            TokenAudience = "";  //What should be a good value here?
        }
    }
}