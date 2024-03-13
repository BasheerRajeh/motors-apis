using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.Services.Common;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class MessageController : ApiControllerBase
    {
        private readonly MessageService _service;
        private readonly EmailService _emailService;


        public MessageController(EmailService emailService, MessageService service)
        {
            _emailService = emailService;
            _service = service;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> sendEmail(MessageVm messageVm)
        {
            var msg = await _service.Save(messageVm);
            _emailService.SendEmail(messageVm.Body);
            return Ok(msg);
        }
    }
}
