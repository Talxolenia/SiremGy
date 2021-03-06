﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiremGy.BLL.Interfaces.Users;
using SiremGy.BLL.Models.Users;
using SiremGy.DTO.Users;

namespace SiremGy.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _usersService.GetUsers();
            var users = UserDTO.CastFromUserModelAgregate(result.Value);

            return Ok(users);
        }

        [HttpGet("{id}", Name = nameof(GetUsers))]
        public async Task<IActionResult> GetUsers(int id)
        {
            var result = await _usersService.GetUser(id);
            var users = new UserDTO(result.Value);

            return Ok(users);
        }

        // POST: api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
