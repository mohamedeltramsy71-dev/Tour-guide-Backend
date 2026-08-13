using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var conversations = await _chatService.GetConversationsAsync(userId);
        return Ok(conversations);
    }

    [HttpGet("{bookingId}/messages")]
    public async Task<IActionResult> GetMessages(int bookingId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var messages = await _chatService.GetMessagesAsync(bookingId, userId, page, pageSize);
        return Ok(messages);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var count = await _chatService.GetUnreadCountAsync(userId);
        return Ok(new { unreadCount = count });
    }
}