using DataService.Repositories.Repository;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.Repositories.Images
{
    public class ImagesRepository : Repository<Image>, IImagesRepository
    {
        private readonly RoomChattingContext _context;
        public ImagesRepository(RoomChattingContext context):base(context)
        {
            _context = context;
        }

        public async void Udpate(Image image)
        {
            _context.Update(image);
        }
    }
}
