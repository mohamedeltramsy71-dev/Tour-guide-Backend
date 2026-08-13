using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.Payment;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("initiate")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
    {
        var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _paymentService.InitiatePaymentAsync(request, touristId);
        return Ok(result);
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook([FromBody] PaymobWebhookDto webhook)
    {
        await _paymentService.HandleWebhookAsync(webhook);
        return Ok();
    }

    [HttpGet("{bookingId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentStatus(int bookingId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _paymentService.GetPaymentStatusAsync(bookingId, userId);
        return Ok(result);
    }
}