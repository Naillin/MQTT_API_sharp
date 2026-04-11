using System.Security.Claims;
using MQTT_API_sharp.Core.Models;

namespace MQTT_API_sharp.Core.Interfaces;

public interface IAuthService
{
    public Task<IList<Claim>> LoginAsync(LoginDto loginModel);
}