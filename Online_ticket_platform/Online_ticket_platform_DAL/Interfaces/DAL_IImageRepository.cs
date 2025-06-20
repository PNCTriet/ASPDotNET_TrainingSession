using System;
using System.Collections.Generic;
using Online_ticket_platform_Model;

namespace Online_ticket_platform_DAL.Interfaces
{
    public interface DAL_IImageRepository
    {
        List<MOD_Image> GetAllImages();
        MOD_Image GetImageById(int id);
        int InsertImage(MOD_Image image);
        bool UpdateImage(MOD_Image image);
        bool DeleteImage(int id);
        List<MOD_Image> GetImagesByUploader(int uploadedBy);
    }
} 