
using DataServices.Entities;
using DataServices.Repositories.Accounts;
using DataServices.Repositories.ChatRoom;
using DataServices.Repositories.ChatRoomUserMapping;
using DataServices.Repositories.Images;
using DataServices.Repositories.ImageSets;
using DataServices.Repositories.MessageRepositorys;
using DataServices.Repositories.Tokens;
using DataServices.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
        public IAccountRepository Accounts { get;  }
        public IUserRepository Users { get;  }
        public ITokenRepostitory Tokens { get;  }
        public IImgSetRepository ImgSets { get;  }
        public IImagesRepository Images { get;  }
        public IChatRoomRepository ChatRoom { get; }
        public IRooUserMappingRepository RoomMapping { get; }
        public IMessageRepository messageRepository { get; }
    }
    public class UnitOfWork: IUnitOfWork
    {
        private readonly RoomChattingContext _db;

        public IAccountRepository Accounts { get; private set; }
        public IUserRepository Users { get; private set; }
        public ITokenRepostitory Tokens { get; private set; }
        public IImgSetRepository ImgSets { get; private set; }
        public IImagesRepository Images { get; private set; }
        public IChatRoomRepository ChatRoom { get; private set; }
        public IRooUserMappingRepository RoomMapping { get; private set; }
        public IMessageRepository messageRepository { get; private set; }
        public UnitOfWork(RoomChattingContext db)
        {
            _db = db;
            Accounts = new AccountRepository(_db);
            Users = new UserRepository(_db);
            Tokens = new TokenRepository(_db);
            ImgSets = new ImgSetRepository(_db);
            Images = new ImagesRepository(_db);
            ChatRoom = new ChatRoomRepository(_db);
            RoomMapping = new RoomUserMappingRepository(_db);
            messageRepository = new MessageRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
