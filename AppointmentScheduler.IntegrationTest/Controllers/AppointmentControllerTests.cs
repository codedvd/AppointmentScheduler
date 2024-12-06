//using MediatR;
//using Moq;
//using System;
//using System.Threading.Tasks;
//using Xunit;

//namespace AppointmentScheduler.UnitTest.Controllers
//{
//    public class AppointmentControllerTests
//    {
//        private readonly Mock<IMediator> _mediatorMock;
//        private readonly AppointmentController _controller;

//        public AppointmentControllerTests()
//        {
//            _mediatorMock = new Mock<IMediator>();
//            _controller = new AppointmentController(_mediatorMock.Object);
//        }

//        [Fact]
//        public async Task GetAppointment_ShouldReturnOk_WhenAppointmentExists()
//        {
//            // Arrange
//            var appointmentId = Guid.NewGuid();
//            var appointmentDto = new AppointmentDto(
//                appointmentId, "John Doe", DateTime.Now, "12345", Guid.NewGuid(), "Dr. Smith", DateTime.Now, "Checkup", AppointmentStatus.Scheduled);

//            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppointmentQuery>(), It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(appointmentDto);

//            // Act
//            var result = await _controller.GetAppointment(appointmentId);

//            // Assert
//            var okResult = result as OkObjectResult;
//            okResult.Should().NotBeNull();
//            okResult.StatusCode.Should().Be(200);
//            okResult.Value.Should().Be(appointmentDto);
//        }

//        [Fact]
//        public async Task CancelAppointment_ShouldReturnNoContent_WhenSuccessful()
//        {
//            // Arrange
//            var command = new CancelAppointmentCommand(Guid.NewGuid());
//            _mediatorMock.Setup(m => m.Send(It.IsAny<CancelAppointmentCommand>(), It.IsAny<CancellationToken>()))
//                         .ReturnsAsync(Unit.Value);

//            // Act
//            var result = await _controller.CancelAppointment(command);

//            // Assert
//            var noContentResult = result as NoContentResult;
//            noContentResult.Should().NotBeNull();
//            noContentResult.StatusCode.Should().Be(204);
//        }
//    }
//}
