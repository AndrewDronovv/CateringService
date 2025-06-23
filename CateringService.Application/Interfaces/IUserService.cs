using CateringService.Application.DataTransferObjects.Requests;
using CateringService.Application.DataTransferObjects.Responses;

namespace CateringService.Application.Abstractions;

public interface IUserService
{
    Task<UserViewModel> CreateUserAsync(AddUserRequest request);
}