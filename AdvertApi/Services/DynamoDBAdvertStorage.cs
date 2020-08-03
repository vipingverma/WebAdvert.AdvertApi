﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;
using AutoMapper;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace AdvertApi.Services
{
    public class DynamoDBAdvertStorage : IAdvertStorageService
    {
	    private IMapper _mapper;

	    public DynamoDBAdvertStorage(IMapper mapper)
	    {
		    _mapper = mapper;
	    }
		public async Task<string> Add(AdvertModel model)
		{
			var dbModel = _mapper.Map<AdvertDbModel>(model);

			dbModel.Id = new Guid().ToString();
			dbModel.CreationDateTime = DateTime.UtcNow;
			dbModel.Status = AdvertStatus.Pending;

			using (var client = new AmazonDynamoDBClient())
			{
				using (var context = new DynamoDBContext(client))
				{
					await context.SaveAsync(dbModel);
				}
			}
			return dbModel.Id;
		}

	    public async Task Confirm(ConfirmAdvertModel model)
	    {
		    using (var client = new AmazonDynamoDBClient())
		    {
			    using (var context = new DynamoDBContext(client))
			    {
				    var record = await context.LoadAsync<AdvertDbModel>(model.Id);
				    if (record == null)
				    {
					    throw new KeyNotFoundException($"A Record wth the ID={model.Id} doesn't exist");
				    }
				    if (model.Status == AdvertStatus.Active)
				    {
					    record.Status = AdvertStatus.Active;
					    await context.SaveAsync(record);
				    }
				    else
				    {
					    await context.DeleteAsync(record);
				    }
			    }
		    }
	    }
    }
}
