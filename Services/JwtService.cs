namespace CPSC319BackEnd.Helper
{
    using System.Text;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;

    public class JwtService
    {
        private string secureKey = "1234567890 a very long word";



        public string Generate(int id)
        {
            var symmetricSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var cred = new SigningCredentials(symmetricSecKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(cred);
            DateTime centuryBegin = new DateTime(1970, 1, 1);
            var exp = new TimeSpan(DateTime.Now.AddYears(1).Ticks - centuryBegin.Ticks).TotalSeconds;
            var now = new TimeSpan(DateTime.Now.Ticks - centuryBegin.Ticks).TotalSeconds;

            var payload = new JwtPayload(issuer: id.ToString(), audience: null, claims: null, notBefore: null, 
                expires: DateTime.UtcNow.AddYears(10));
            var st = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(st);
        }
        public JwtSecurityToken Verify(String jwt)
        {
            var st = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            st.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);
            return (JwtSecurityToken) validatedToken;
        }
    }

}

