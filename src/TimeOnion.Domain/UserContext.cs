using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain;

public record UserContext(UserId UserId, EmailAddress EmailAddress);