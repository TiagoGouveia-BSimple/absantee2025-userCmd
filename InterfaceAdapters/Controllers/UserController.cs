using Application.DTO;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
namespace InterfaceAdapters.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    List<string> _errorMessages = new List<string>();
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // Post: api/users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUsers(UserDTO userDTO)
    {
        {
            var userDTOResult = await _userService.Add(userDTO);
            return Ok(userDTOResult);
        }
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
