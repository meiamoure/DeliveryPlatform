using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using DeliveryPlatform.Application.Auth.Dtos;

namespace DeliveryPlatform.Application.Auth.Commands.Login;
public sealed record LoginCommand(
    string Login,
    string Password
) : IRequest<LoginResponseDto>;
