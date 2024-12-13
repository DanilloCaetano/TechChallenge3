using Common.MessagingService;
using Domain.Contact.Entity;
using Domain.Region.Entity;
using Domain.Region.Service;
using Infraestructure.Context;
using Infraestructure.Repository.RegionRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Drawing;
using TechChallenge.Controllers.Regions.Dto;
using TechChallenge.Controllers.Regions.Http;

namespace Tests
{
    public class RegionsTest
    {
        Mock<IRabbitMqService> mockRabService;
        Mock<IRegionService> mockRegionService;
        public RegionsTest()
        {
            mockRabService = new Mock<IRabbitMqService>();
            mockRegionService = new Mock<IRegionService>();
        }

        #region MOCK DATA
        private static RegionPostDto dtoRegionInvalidDescPost = new RegionPostDto
        {
            Active = true,
            DDD = "11",
            Description = null
        };
        public static IEnumerable<object[]> GetInvalidRegionDescPostDtoData()
        {
            yield return new object[] { dtoRegionInvalidDescPost, "description must not be null" };
        }

        private static RegionPostDto dtoRegionInvalidDDDPost = new RegionPostDto
        {
            Active = true,
            DDD = "",
            Description = "test"
        };
        public static IEnumerable<object[]> GetInvalidRegionDDDPostDtoData()
        {
            yield return new object[] { dtoRegionInvalidDDDPost, "ddd must not be empty" };
        }
        #endregion

        [Theory]
        [MemberData(nameof(GetInvalidRegionDescPostDtoData))]
        [MemberData(nameof(GetInvalidRegionDDDPostDtoData))]
        public async Task ShouldReturnErrorAllPostDto(RegionPostDto dto, string expectedErrorMsg)
        {
            var validator = new RegionPostValidate();
            var result = validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Equal(result.Errors.FirstOrDefault()?.ErrorMessage, expectedErrorMsg);
        }

        [Fact]
        public async Task ShouldReturnErrorPostRegionAlreadyExists()
        {
            var dto = new RegionPostDto
            {
                Active = true,
                DDD = "11",
                Description = ""
            };
            var mockRegionService = new Mock<IRegionService>();
            mockRegionService.Setup(r => r.GetByDDD(It.IsAny<string>())).Returns(Task.FromResult(new RegionEntity
            {
                Active  = dto.Active,
                DDD = dto.DDD,
                Description = dto.Description
            }));

            var controller = new RegionsController(mockRegionService.Object, mockRabService.Object);

            var result = await controller.AddRegion(dto);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var message = (string)objectResult?.Value ?? "";

            Assert.Equal("region already exist", message.ToLower());
            Assert.Equal(statusCode, StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnOKPost()
        {
            var dto = new RegionPostDto
            {
                Active = true,
                DDD = "21",
                Description = "TEST"
            };

            var mockRegionService = new Mock<IRegionService>();

            var mockRabbitService = new Mock<IRabbitMqService>();
            mockRabbitService.Setup(m => m.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RegionEntity>())).Returns(Task.FromResult(true));

            var controller = new RegionsController(mockRegionService.Object, mockRabbitService.Object);

            var result = await controller.AddRegion(dto);
            var objectResult = result as StatusCodeResult;
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task ShouldAddRegionOnRepository()
        {
            var mockSet = new Mock<DbSet<RegionEntity>>();
            var data = new List<RegionEntity>
            {
                new RegionEntity
                {
                    Active = true,
                    DDD = "21",
                    Description = "TEST"
                }
            }.AsQueryable();

            var newData = data.FirstOrDefault();
            newData.Id = Guid.NewGuid();

            var mockContext = new Mock<TechChallengeContext>();
            mockContext.Setup(c => c.Regions).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<RegionEntity>()).Returns(mockSet.Object);

            var repo = new RegionRepository(mockContext.Object);
            await repo.AddAsync(newData);

            mockSet.Verify(m => m.Add(It.IsAny<RegionEntity>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}