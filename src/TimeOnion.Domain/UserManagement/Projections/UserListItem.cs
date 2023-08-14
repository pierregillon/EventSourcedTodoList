using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement.Projections;

public record UserListItem(UserId UserId, EmailAddress EmailAddress, PasswordHash PasswordHash);
