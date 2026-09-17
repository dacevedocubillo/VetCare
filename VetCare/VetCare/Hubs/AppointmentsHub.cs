using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace VetCare.Hubs;
[Authorize]
public class AppointmentsHub : Hub;
