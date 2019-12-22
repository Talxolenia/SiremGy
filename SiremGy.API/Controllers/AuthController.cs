﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SiremGy.BLL.Interfaces.Common;
using SiremGy.BLL.Interfaces.Exceptions;
using SiremGy.BLL.Interfaces.Token;
using SiremGy.Models.Users;

namespace SiremGy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(IUsersService usersService, ITokenService tokenService, IConfiguration configuration)
        {
            _usersService = usersService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost(nameof(RegisterUser))]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            var result = await _usersService.RegisterUser(registerModel);
            IActionResult actionResult = CreatedAtRoute("", result);

            return actionResult;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _usersService.Login(loginModel);

            var key = _configuration.GetSection("AppSettings:Token").Value;
            string generatedToken = _tokenService.GenerateAuthenticationToken(result.Value, key);

            IActionResult actionResult = Ok(new { token = generatedToken });

            return actionResult;
        }
    }
}
