﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AdvertApi.Models;
using AdvertApi.Services;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdvertApi.Controllers
{
	[ApiController]
	[Route("adverts/v1")]
	public class Advert : ControllerBase
    {
	    private IAdvertStorageService _advertStorageService;

	    public Advert(IAdvertStorageService advertStorageService)
	    {
		    _advertStorageService = advertStorageService;
	    }
		
		[HttpPost]
		[Route("Create")]
		[ProducesResponseType(400)]
		[ProducesResponseType(201, Type = typeof(CreateAdvertResponse))]
	    public async Task<IActionResult> Create(AdvertModel model)
		{
			string recordId;
			try
		    {
			    recordId = await _advertStorageService.Add(model);

		    }
			catch(KeyNotFoundException)
			{
				return new NotFoundResult();
			}
		    catch (Exception e)
		    {
			    return StatusCode(500, e.Message);
			    throw;
			    
		    }
			return StatusCode(201, new CreateAdvertResponse { Id = recordId});

		}

		[HttpPut]
		[Route("Confirm")]
		[ProducesResponseType(404)]
		[ProducesResponseType(200)]
		public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
	    {
		    try
		    {
			    await _advertStorageService.Confirm(model);
		    }
		    catch (KeyNotFoundException e)
		    {
			    return new NotFoundResult();
			    
		    }
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			    
		    }
			return new OkResult();
	    }
    }
}
