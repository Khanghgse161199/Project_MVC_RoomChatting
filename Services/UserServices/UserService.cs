using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Profile;

namespace Services.UserServices
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(string accID, string fullName, DateTime birthDay, string imgUrl);
        Task<ProfileViewModel> GetProfileAsync(string AccId);
    }

    public class UserService:IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ProfileViewModel> GetProfileAsync(string AccId)
        {
            var currentAcc = await _uow.Accounts.FirstOfDefaultAsync(p => p.Id == AccId && p.IsActive, "Users");
            if (currentAcc != null)
            {
                var currentUser = await _uow.Users.FirstOfDefaultAsync(p => p.AccId == currentAcc.Id);
                var images = await _uow.Images.FirstOfDefaultAsync(p => p.ImgSetId == currentUser.ImgSetId && p.IsActive);
                if (images != null)
                {
                    var newProfile = new ProfileViewModel()
                    {
                        AccId = AccId,
                        birthday = currentUser.BirthDay,
                        Email = currentAcc.Email,
                        Fullname = currentUser.Fullname,
                        ImgUrl = images.ImgUrl
                    };

                    return newProfile;
                }
                else return null;
            }
            else return null;
        }
        public async Task<bool> CreateUserAsync(string accID, string fullName, DateTime birthDay, string imgUrl)
        {
            ImgSet newImgSet = new ImgSet()
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,    
            };

            await _uow.ImgSets.AddAsync(newImgSet);

            Image newImage = new Image()
            {
                Id = Guid.NewGuid().ToString(),
                ImgSetId = newImgSet.Id,
                ImgUrl = imgUrl,
                IsActive = true,
            };

            await _uow.Images.AddAsync(newImage);

            User newUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Fullname = fullName,
                BirthDay = birthDay,
                ImgSetId = newImgSet.Id,
                AccId = accID,
            };

            await _uow.Users.AddAsync(newUser);
            await _uow.SaveAsync();
            return true;
        }
    }
}
