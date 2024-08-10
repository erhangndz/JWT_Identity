namespace JwtIdentity.WebUI.Models
{
    public class TokenResponse
    {

       
            public object actor { get; set; }
            public string[] audiences { get; set; }
            public Claim[] claims { get; set; }
            public string encodedHeader { get; set; }
            public string encodedPayload { get; set; }
            public Header header { get; set; }
            public string id { get; set; }
            public string issuer { get; set; }
            public Payload payload { get; set; }
            public object innerToken { get; set; }
            public object rawAuthenticationTag { get; set; }
            public object rawCiphertext { get; set; }
            public object rawData { get; set; }
            public object rawEncryptedKey { get; set; }
            public object rawInitializationVector { get; set; }
            public object rawHeader { get; set; }
            public object rawPayload { get; set; }
            public string rawSignature { get; set; }
            public object securityKey { get; set; }
            public string signatureAlgorithm { get; set; }
            public Signingcredentials signingCredentials { get; set; }
            public object encryptingCredentials { get; set; }
            public object signingKey { get; set; }
            public string subject { get; set; }
            public DateTime validFrom { get; set; }
            public DateTime validTo { get; set; }
            public DateTime issuedAt { get; set; }
       

        public class Header
        {
            public string alg { get; set; }
            public string typ { get; set; }
        }

        public class Payload
        {
            public string sub { get; set; }
            public string jti { get; set; }
            public string email { get; set; }
            public string uid { get; set; }
            public string[] roles { get; set; }
            public int exp { get; set; }
            public string iss { get; set; }
            public string aud { get; set; }
        }

        public class Signingcredentials
        {
            public string algorithm { get; set; }
            public object digest { get; set; }
            public object cryptoProviderFactory { get; set; }
            public Key key { get; set; }
            public object kid { get; set; }
        }

        public class Key
        {
            public int keySize { get; set; }
        }

        public class Claim
        {
            public string issuer { get; set; }
            public string originalIssuer { get; set; }
            public Properties properties { get; set; }
            public object subject { get; set; }
            public string type { get; set; }
            public string value { get; set; }
            public string valueType { get; set; }
        }

        public class Properties
        {
        }

    }
}
