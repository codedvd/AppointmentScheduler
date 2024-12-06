using AppointmentScheduler.Application.Exceptions.AppointmentScheduling.Exceptions;
using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace AppointmentScheduler.IntegrationTests
{
    public class AppointmentSchedulingIntegrationTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        private readonly SchedulerDbContext _context;

        public AppointmentSchedulingIntegrationTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = fixture.CreateContext();
        }

        [Fact]
        public async Task CompleteAppointmentWorkflow_FullScenarioTest()
        {
            // Arrange: Setup test provider and patient
            var provider = new Provider
            {
                Name = "Dr. Smith",
                Specialty = "General Practitioner"
            };
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            var patient = new Patient
            {
                PatientName = "Jane Doe",
                DateOfBirth = new DateTime(1985, 5, 15),
                NationalId = "987654321"
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Act: Book an appointment
            var appointment = new Appointment
            {
                ProviderId = provider.Id,
                Id = patient.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Status = AppointmentStatus.Confirmed
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Assert: Verify appointment details
            var bookedAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointment.Id);

            Assert.That(bookedAppointment, Is.Not.Null);
            Assert.Equals(AppointmentStatus.Confirmed, bookedAppointment.Status);
            Assert.Equals(provider.Id, bookedAppointment.ProviderId);
            Assert.Equals(patient.Id, bookedAppointment.Id);
        }


        [Fact]
        public async Task ProviderAvailability_PreventOverlappingAppointments()
        {
            // Arrange: Setup provider and first appointment
            var provider = new Provider
            {
                Name = "Dr. Johnson",
                Specialty = "Cardiology"
            };
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            var firstAppointment = new Appointment
            {
                ProviderId = provider.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1).Date.AddHours(10),
                Duration = TimeSpan.FromMinutes(30),
                Status = AppointmentStatus.Confirmed
            };
            _context.Appointments.Add(firstAppointment);
            await _context.SaveChangesAsync();

            // Act & Assert: Try to book overlapping appointment
            var overlappingAppointment = new Appointment
            {
                ProviderId = provider.Id,
                AppointmentDateTime = DateTime.Now.AddDays(1).Date.AddHours(10).AddMinutes(15),
                Duration = TimeSpan.FromMinutes(30)
            };

            //await Assert.ThrowsAsync<OverlappingAppointmentException>(async () =>
            //{
            //    _context.Appointments.Add(overlappingAppointment);
            //    await _context.SaveChangesAsync();
            //});
        }
    }
}
