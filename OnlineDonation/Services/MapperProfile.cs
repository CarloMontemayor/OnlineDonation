using AutoMapper;
using OnlineDonation.Data.Entity;
using OnlineDonation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineDonation.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Mapping properties from Customer to CustomerModel  
            CreateMap<Donation, DonationViewModel>();
            CreateMap<DonationViewModel, Donation>();
            // ForMember is used incase if any field doesn't match  
        }  
    }
}
