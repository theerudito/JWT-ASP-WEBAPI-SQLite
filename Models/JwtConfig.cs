using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT_ASP_WEBAPI.Models
{
  public class JwtConfig
  {
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public int Expires { get; set; }
  }
}