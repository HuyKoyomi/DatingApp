using API.DTOs;
using API.Entities;
using API.Extensions;
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
}