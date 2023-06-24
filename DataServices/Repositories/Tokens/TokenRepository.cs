using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Tokens
{
    internal class TokenRepository : Repository<Token>, ITokenRepostitory
    {
        private readonly RoomChattingContext _context;
        public TokenRepository(RoomChattingContext context):base(context)
        {
            _context = context;
        }

        public void Udpate(Token token)
        {
            _context.Update(token);
        }
    }
}
