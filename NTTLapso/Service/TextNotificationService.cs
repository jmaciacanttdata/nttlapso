using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Models.TextNotification;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class TextNotificationService
    {
        private TextNotificationRepository _repo = new TextNotificationRepository();
        public TextNotificationService() { }

        public async Task<List<TextNotificationData>> List(IdTextNotificationRequest request)
        {
            return await _repo.List(request);
        }

        public async Task Create(TextNotificationRequest request)
        {
            await _repo.Create(request);
        }

        public async Task Edit(IdTextNotificationRequest request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }
}
