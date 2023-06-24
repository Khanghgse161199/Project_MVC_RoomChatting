using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.ImageSets
{
    public class ImgSetRepository : Repository<ImgSet>, IImgSetRepository
    {
        private readonly RoomChattingContext _context;
        public ImgSetRepository (RoomChattingContext context) : base (context)
        {
            _context = context;
        }
        public void Udpate(ImgSet imgSet)
        {
           _context.Update(imgSet);
        }
    }
}
