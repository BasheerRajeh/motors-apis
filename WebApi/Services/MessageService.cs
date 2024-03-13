using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.ViewModels;

namespace WebApi.Services
{
    public class MessageService
    {
        private readonly UnitOfWork _uow;
        private readonly ContactSubmissionService _contactService;

        public MessageService(UnitOfWork uow, ContactSubmissionService contactService)
        {
            _uow = uow;
            _contactService = contactService;
        }

        public async Task<Message> Save(MessageVm message)
        {

            var existingUser = message.Email != null ? await _uow.ContactSubmissions.FindOne(c => c.Email == message.Email) : null;

            var existing = message.Id.HasValue && message.Id > 0 ? await _uow.Message.GetDetails(message.Id.Value) : null;


            if(existingUser == null)
            {
                existingUser = await _contactService.Save(new ContactSubmissionVm
                {
                    Email = message.Email,
                    Name = message.FullName,
                    Active = true,
                    CreatedDate = DateTime.UtcNow,
                });
            }

            var msgNew = Map(message, existing ?? new Message());

            if (existing is null)
            {
                msgNew.CreatedDate = msgNew.UpdatedDate;
                msgNew.ContactSubmissionId = existingUser.Id;
                msgNew = await _uow.Message.Add(msgNew);
            }

            await _uow.CompleteAsync();

            return msgNew;
        }

        private Message Map(MessageVm messageVm, Message message)
        {
            message.Body = messageVm.Body;
            message.UpdatedDate = DateTime.UtcNow;
            message.ContactSubmissionId = messageVm.ContactSubmissionId;
            return message;
        }

        private MessageVm MapVm(MessageVm messageVm, Message message)
        {
            messageVm.Body = message.Body;
            messageVm.UpdatedDate = message.UpdatedDate;
            messageVm.ContactSubmissionId = message.ContactSubmissionId;
            return messageVm;
        }


        internal async Task<MessageVm> GetMessage(int msgId)
        {
            var message = await _uow.Message.GetDetails(msgId);

            return message is null
                ? throw new Exception($"Message with Id:{msgId} not found")
                : MapVm(new MessageVm(), message);


        }
    }
}
