﻿using Faze.Examples.Gallery.API.Data;
using Faze.Examples.Gallery.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faze.Examples.Gallery.API.Controllers
{
    [ApiController]
    public class VisualisationsController : ControllerBase
    {
        private readonly ILogger<VisualisationsController> logger;
        private readonly IVisualisationRepository repository;

        public VisualisationsController(ILogger<VisualisationsController> logger, IVisualisationRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet("visualisations/")]
        public Task<IEnumerable<Visualisation>> GetAll()
        {
            return repository.GetAll();
        }

        [HttpGet("visualisations/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await repository.Get(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("visualisations/")]
        public async Task<IActionResult> Post(Visualisation vis)
        {
            var result = await repository.Create(vis);

            return Ok(result);
        }
    }
}
