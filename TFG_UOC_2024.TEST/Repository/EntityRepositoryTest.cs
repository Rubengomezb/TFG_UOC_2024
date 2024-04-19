using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;
using Moq;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Repository;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.TEST.Repository
{
    public class EntityRepositoryTest
    {
        private readonly Mock<DbSet<Menu>> _mockSet;
        private readonly Mock<ApplicationContext> _mockContext;
        private readonly EntityRepository<Menu> _repository;

        public EntityRepositoryTest()
        {
            _mockSet = new Mock<DbSet<Menu>>();
            _mockContext = new Mock<ApplicationContext>();
            _repository = new EntityRepository<Menu>(_mockContext.Object);
        }

        [Test]
        public void GetAll_ReturnsAllEntities()
        {
            // Arrange
            _mockContext.Setup(ctx => ctx.Set<Menu>()).Returns(_mockSet.Object);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.IsInstanceOf<DbSet<Menu>>(result);
        }

        [Test]
        public void GetById_ReturnsEntityById()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new Menu { Id = id };
            _mockSet.Setup(m => m.Find(id)).Returns(entity);
            _mockContext.Setup(ctx => ctx.Set<Menu>()).Returns(_mockSet.Object);

            // Act
            var result = _repository.GetById(id);

            // Assert
            Assert.That(entity, Is.EqualTo(result));
        }
    }
}
