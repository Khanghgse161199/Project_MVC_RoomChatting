using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Tokens
{
    public interface ITokenRepostitory: IRepository<Token>
    {
        void Udpate(Token token);
    }
}
