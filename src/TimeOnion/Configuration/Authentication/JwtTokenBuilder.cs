using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.UserManagement;

namespace TimeOnion.Configuration.Authentication;

public class JwtTokenBuilder
{
    private readonly IClock _clock;
    private readonly JwtTokenOptions _options;

    public JwtTokenBuilder(IOptions<JwtTokenOptions> options, IClock clock)
    {
        _clock = clock;
        _options = options.Value;
    }

    public JwtToken Build(UserId userId, string email)
    {
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.Value.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, email)
        };
        var token = GetToken(authClaims);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtToken(tokenValue, token.ValidTo);
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));

        return new JwtSecurityToken(
            _options.ValidIssuer,
            _options.ValidAudience,
            expires: _clock.Now().Add(_options.Validity),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}