using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DTOs;
using Domain.Entities;

namespace ApplicationService.Extensions
{
    public static class UserDtoExtension
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto(user);
        }

        public static ICollection<UserDto> ToUserDtos(this ICollection<User> users)
        {
            return users.Select(u => new UserDto(u)).ToList();
        }
    }
}
