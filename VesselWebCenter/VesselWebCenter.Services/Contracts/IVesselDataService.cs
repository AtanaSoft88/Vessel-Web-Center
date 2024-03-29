﻿using Microsoft.AspNetCore.Mvc.Rendering;
using VesselWebCenter.Data.Models;
using VesselWebCenter.Services.ViewModels;

namespace VesselWebCenter.Services.Contracts
{
    public interface IVesselDataService
    {
        IQueryable<VesselsViewModel> GetAll();        
        Task<SingleVesselViewModel> GetChoosenVessel(int id);
        Task<IEnumerable<VesselsHomeViewModel>> AllEmptyVesselsAsHomePage();


    }
}
