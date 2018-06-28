using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Models;

namespace TicTacToe.Services
{
    public class GameSessionService : IGameSessionService
    {
        private static ConcurrentBag<GameSessionModel> _sessions;
        private IUserService _userService;

        static GameSessionService()
        {
            _sessions = new ConcurrentBag<GameSessionModel>();
        }

        public GameSessionService(IUserService userService)
        {
            _userService = userService;
        }

        public Task<GameSessionModel> GetGameSession(Guid gamesSessionId)
        {
            return Task.Run(() => _sessions.FirstOrDefault(x => x.Id == gamesSessionId));
        }

        public async Task<GameSessionModel> CreateGameSession(Guid invitationId, string invitedByEmail, string invitedPlayerEmail)
        {
            var invitedBy = await _userService.GetUserByEmail(invitedByEmail);
            var invitedPlayer = await _userService.GetUserByEmail(invitedPlayerEmail);

            GameSessionModel session = new GameSessionModel
            {
                User1 = invitedBy,
                User2 = invitedPlayer,
                Id = invitationId,
                ActiveUser = invitedBy
            };

            _sessions.Add(session);
            return session;
        }
    }
}
