using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;

namespace API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/entities")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class EntitiesController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Get a page of entities
        /// </summary>
        /// <param name="request">
        /// A GetEntitiesRequest object which contains fields for paging, searching, filtering, sorting and shaping entity data</param>
        /// <returns>A page of entities</returns>
        /// <response code="200">Success - OK - Returns the requested page of entities</response>
        /// <response code="204">Success - No Content - No entities matched given criteria</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(API.Models.v1.Request.GetEntitiesRequest request)
        {
            if (request == null) return BadRequest();

            var entities = GetEntities(request);

            if (entities == null || !entities.Any()) return NoContent();

            return Ok(entities);
        }

        /// <summary>
        /// Get a entity
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>The requested entity</returns>
        /// <response code="200">Success - OK - Returns the requested entity</response>
        /// <response code="204">Success - No Content - No entity matched the given identifier</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromRoute] Guid id)
        {
            if (id == default(Guid)) return BadRequest();

            var entity = GetEntity(id);

            if (entity == null) return NoContent();

            return Ok(entity);
        }

        #endregion GET

        #region POST

        /// <summary>
        /// Add a entity
        /// </summary>
        /// <param name="request">A CreateEntityRequest object which contains all the necessary data to create a entity</param>
        /// <returns>A URI to the newly created entity in the header (location)</returns>
        /// <response code="201">Success - Created - The entity was successfully created</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response> 
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add([FromBody] API.Models.v1.Request.CreateEntityRequest request)
        {
            if (request == null) return BadRequest();

            var entity = AddEntity(request);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        #endregion POST

        #region PUT

        /// <summary>
        /// Fully update a entity
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <param name="request">An UpdateEntityRequest object which contains all the updates</param>
        /// <returns>The updated entity</returns>
        /// <response code="200">Success - OK - The entity was successfully updated</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No entity matched the given identifier</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(
            [FromRoute] Guid id,
            [FromBody] API.Models.v1.Request.UpdateEntityRequest request)
        {
            if (request == null) return BadRequest();

            // ToDo --> Update the entity on the system
            var entity = UpdateEntity(id, request);

            return Ok(entity);
        }

        #endregion PUT

        #region PATCH

        /// <summary>
        /// Fully or partially update a entity
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <param name="patchDocument">
        /// A JSON Patch Document detailing the full or partial updates to the entity
        /// </param>
        /// <returns>The updated entity</returns>
        /// <response code="200">Success - OK - The entity was successfully updated</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No entity matched the given identifier</response>
        /// <response code="422">Error - Unprocessable Entity - Unable to process the contained instructions.</response>
        /// <remarks>
        /// Sample request (this request updates the entity's first name): \
        /// PATCH /entities/id \
        /// [ \
        ///     { \
        ///         "op": "replace", \
        ///         "path": "/firstname", \
        ///         "value": "Dave" \
        ///     } \
        /// ] \
        /// </remarks>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Update(
            [FromRoute] Guid id,
            [FromBody] JsonPatchDocument<API.Models.v1.Request.UpdateEntityRequest> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            // ToDo --> Update the entity on the system
            var entity = UpdateEntity(id, patchDocument);

            return Ok(entity);
        }

        #endregion PATCH

        #region DELETE

        /// <summary>
        /// Delete a entity
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>Nothing</returns>
        /// <response code="204">Success - No Content - Entity was successfully deleted</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No entity matched the given identifier</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] Guid id)
        {
            if (id == default(Guid)) return BadRequest();

            DeleteEntity(id);

            return NoContent();
        }

        #endregion DELETE

        #region Methods

        private API.Models.v1.Response.EntityResponse AddEntity(API.Models.v1.Request.CreateEntityRequest request)
        {
            // ToDo --> Try and add entity to system

            // For now, fake successful response...
            return new API.Models.v1.Response.EntityResponse { Id = Guid.NewGuid() };
        }

        private void DeleteEntity(Guid id)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));

            // ToDo --> Try and delete the entity from system

            return;
        }

        private API.Models.v1.Response.EntityResponse GetEntity(Guid id)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));

            // ToDo --> Try and get entity from system

            return null;
        }

        private IEnumerable<API.Models.v1.Response.EntityResponse> GetEntities(API.Models.v1.Request.GetEntitiesRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Try and get entities from system

            return null;
        }

        private API.Models.v1.Response.EntityResponse UpdateEntity(Guid id, API.Models.v1.Request.UpdateEntityRequest request)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Try and update entity in system

            return null;
        }

        private API.Models.v1.Response.EntityResponse UpdateEntity(Guid id, JsonPatchDocument<API.Models.v1.Request.UpdateEntityRequest> patchDocument)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));
            if (patchDocument == null) throw new ArgumentNullException(nameof(patchDocument));

            // ToDo --> Try and update entity in system

            return null;
        }

        #endregion Methods
    }
}