namespace TimeOnion.Configuration.Authentication;

public record JwtToken(string Token, DateTime ValidTo);