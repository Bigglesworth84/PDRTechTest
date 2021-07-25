using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using PDR.PatientBooking.Service.BookingServices.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDR.PatientBooking.Service.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddBookingRequestValidator _validator;

        public BookingService(PatientBookingContext context, IAddBookingRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public AddBookingResponse AddBooking(AddBookingRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
                throw new ArgumentException(validationResult.Errors.First());

            var patient = _context.Patient.Find(request.PatientId);

            var order = new Order
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                SurgeryType = (int)patient.Clinic.SurgeryType
            };

            _context.Order.Add(order);
            _context.SaveChanges();

            return new AddBookingResponse 
            { 
                Id = order.Id, 
                PatientId = order.PatientId, 
                DoctorId = order.DoctorId, 
                StartTime = order.StartTime, 
                EndTime = order.EndTime 
            };
        }

        public GetPatientNextAppointmentResponse GetPatientNextAppointment(long identificationNumber)
        {
            var nextBooking = _context.Order
                .Where(o => o.PatientId == identificationNumber && o.StartTime > DateTime.Now)
                .FirstOrDefault();

            if (nextBooking is null)
                return null;

            return new GetPatientNextAppointmentResponse
            {
                Id = nextBooking.Id,
                DoctorId = nextBooking.DoctorId,
                StartTime = nextBooking.StartTime,
                EndTime = nextBooking.EndTime
            };
        }
    }
}
