using Application.DTO;
using Application.IService;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
namespace InterfaceAdapters.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    List<string> _errorMessages = new List<string>();
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Post: api/users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUsers(CreateUserDTO userDTO)
    {
        var userDTOResult = await _userService.Add(userDTO);
        var result = new UserDTO
        {
            Id = userDTOResult.Id,
            Names = userDTOResult.Names,
            Surnames = userDTOResult.Surnames,
            Email = userDTOResult.Email,
            Period = userDTOResult.Period
        };
        
        return Ok(userDTOResult);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<UserDTO>> UpdateUser(UpdateUserDTO updaterUserDTO)
    {
        var result = await _userService.UpdateUser(new UserDTO
        {
            Id = updaterUserDTO.Id,
            Names = updaterUserDTO.Names,
            Surnames = updaterUserDTO.Surnames,
            Email = updaterUserDTO.Email,
            Period = updaterUserDTO.Period
        });

        return Ok(result);
    }

    // Patch: api/users/id/activation
    [HttpPatch("{id}/updateactivation")]
    public async Task<ActionResult<UserDTO>> UpdateActivation(Guid id, [FromBody] ActivationDTO activationPeriodDTO)
    {
        if (!await _userService.Exists(id))
            return NotFound();

        var result = await _userService.UpdateActivation(id, activationPeriodDTO);
        return Ok(result);
    }
}
