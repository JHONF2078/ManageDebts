using Xunit;
using Moq;
using FluentAssertions;
using AutoFixture;
using ManageDebts.Application.Auth.Register.Commands;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
using ManageDebts.Application.Common;

namespace ManageDebts.Application.Tests
{
    public class RegisterCommandHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _fixture = new Fixture();
            _mockAuthService = new Mock<IAuthService>();
            _handler = new RegisterCommandHandler(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            var command = _fixture.Create<RegisterCommand>();
            var expectedResponse = _fixture.Build<AuthenticationResponse>()
                                           .With(x => x.Email, command.Email)
                                           .Create();

            _mockAuthService
                .Setup(s => s.RegisterAsync(It.Is<RegisterCommand>(c =>
                        c.Email == command.Email &&
                        c.FullName == command.FullName &&
                        c.Password == command.Password),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthenticationResponse>.Success(expectedResponse));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value!.Email.Should().Be(command.Email);
            _mockAuthService.Verify(s => s.RegisterAsync(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
       
    }
}
