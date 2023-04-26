using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.TextNotification;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class MastersService
    {
        private MastersRepository _repo = new MastersRepository();
        public MastersService() { }

        public async Task<TextNotificationResponse> SetTextNotification(TextNotificationRequest textNotificationRequest)
        {
            return await _repo.SetTextNotification(textNotificationRequest);
        }
        public async Task<TextNotificationDataResponse> GetTextNotification(TextNotificationRequest textNotificationRequest)
        {
            return await _repo.GetTextNotification(textNotificationRequest);
        }
        public async Task<TextNotificationResponse> UpdateTextNotification(TextNotificationRequest textNotificationRequest)
        {
            return await _repo.UpdateTextNotification(textNotificationRequest);
        }
        public async Task<TextNotificationResponse> DeleteTextNotification(TextNotificationRequest textNotificationRequest)
        {
            return await _repo.DeleteTextNotification(textNotificationRequest);
        }
    }
}
