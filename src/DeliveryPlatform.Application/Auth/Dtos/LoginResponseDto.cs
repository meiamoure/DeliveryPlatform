using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryPlatform.Application.Auth.Dtos;
public sealed record LoginResponseDto(
    string AccessToken,
    string Login,
    string FullName,
    string Role
);
