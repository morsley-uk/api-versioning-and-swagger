using API.Controllers.v1;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace API.UnitTests
{
    public class EntitiesControllerUnitTests
    {
        [Fact]
        public void GET_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var sut = new EntitiesController();
            const API.Models.v1.Request.GetEntitiesRequest request = null;

            // Act...
            var result = sut.Get(request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void GET_Should_Return_NoContent_When_No_Entities_Exist()
        {
            // Arrange...
            var sut = new EntitiesController();
            var request = new API.Models.v1.Request.GetEntitiesRequest();

            // Act...
            var response = sut.Get(request);

            // Assert...
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public void GET_By_Id_Should_Return_BadRequest_When_Id_Is_Default()
        {
            // Arrange...
            var sut = new EntitiesController();
            var id = Guid.Empty;

            // Act...
            var result = sut.Get(id);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void GET_By_Id_Should_Return_NoContent_When_No_Entities_Exist()
        {
            // Arrange...
            var sut = new EntitiesController();
            var userId = Guid.NewGuid();

            // Act...
            var response = sut.Get(userId);

            // Assert...
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public void POST_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var sut = new EntitiesController();
            const API.Models.v1.Request.CreateEntityRequest request = null;

            // Act...
            var result = sut.Add(request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void POST_Should_Return_Created_When_Request_Is_Successful()
        {
            // Arrange...
            var sut = new EntitiesController();
            var request = new API.Models.v1.Request.CreateEntityRequest();

            // Act...
            var result = sut.Add(request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        public void DELETE_Should_Return_BadRequest_When_Id_Is_Default()
        {
            // Arrange...
            var sut = new EntitiesController();
            var id = Guid.Empty;

            // Act...
            var result = sut.Delete(id);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void PUT_Should_Return_Bad_Request_When_Request_Is_Null()
        {
            // Arrange...
            var sut = new EntitiesController();
            var userId = Guid.NewGuid();
            const API.Models.v1.Request.UpdateEntityRequest request = null;

            // Act...
            var result = sut.Update(userId, request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public void PATCH_Should_Return_Bad_Request_When_PatchDocument_Is_Null()
        {
            // Arrange...
            var sut = new EntitiesController();
            var userId = Guid.NewGuid();
            const JsonPatchDocument<API.Models.v1.Request.UpdateEntityRequest> patchDocument = null;

            // Act...
            var result = sut.Update(userId, patchDocument);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }
    }
}