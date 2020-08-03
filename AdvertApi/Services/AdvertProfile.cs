using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace AdvertApi.Services
{
	public class AdvertProfile : Profile
	{
		public AdvertProfile()
		{
			CreateMap<AdvertDbModel, AdvertDbModel>();
		}
	}


}
