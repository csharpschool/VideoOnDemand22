using System;

namespace VOD.Common.DTOModels
{
    public class TokenDTO
    {
        public string Token { get; set; } = "";
        public DateTime TokenExpires { get; set; } = default;
        public bool TokenHasExpired
        {
            get
            {
                return TokenExpires == default ? true : !(TokenExpires.Subtract(DateTime.UtcNow).Minutes > 0);
            }
        }

        public TokenDTO(string token, DateTime expires)
        {
            Token = token;
            TokenExpires = expires;
        }

        public TokenDTO()
        {
        }
    }
}
