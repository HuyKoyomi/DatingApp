using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class MessagesController : BaseApiController
{

    private readonly IMessageRepository _messRepo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    public MessagesController(IMessageRepository messRepo, IUserRepository userRepo, IMapper mapper)
    {
        _messRepo = messRepo;
        _userRepo = userRepo;
        _mapper = mapper;

    }

    [HttpPost()]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUserName();

        if (username == createMessageDto.RecipientUsername)
        {
            return BadRequest("You cannot message yourself");
        }

        var sender = await _userRepo.GetUserByUsernameAsync(username);
        var recipient = await _userRepo.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if (sender == null || recipient == null) return BadRequest("Cannot send message at this time");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.Username,
            RecipientUsername = recipient.Username,
            Content = createMessageDto.Content,
        };

        _messRepo.AddMessage(message);

        if (await _messRepo.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
        return BadRequest("Failed to create message");
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUserName();
        var message = await _messRepo.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(message);

        return message;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
    {
        var currentUsername = User.GetUserName();
        return Ok(await _messRepo.GetMessageThread(currentUsername, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUserName();

        var message = await _messRepo.GetMessage(id);

        if (message == null) return BadRequest("Cannot delete this message");

        if (message.SenderUsername != username && message.RecipientUsername != username)
        {
            return Forbid();
        }
        if (message.SenderUsername == username) message.SenderDeleted = true;

        if (message.RecipientUsername == username) message.RecipientDeleted = true;

        if (message is
            {
                SenderDeleted: true,
                RecipientDeleted: true
            })
        {
            _messRepo.DeleteMessage(message);
        }

        if (await _messRepo.SaveAllAsync()) return Ok();

        return BadRequest("Problem deleting the messages");

    }

}