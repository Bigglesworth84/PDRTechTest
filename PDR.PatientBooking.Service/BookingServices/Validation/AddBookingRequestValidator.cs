using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.Validation;
using System;
using System.Linq;

namespace PDR.PatientBooking.Service.BookingServices.Validation
{
    public class AddBookingRequestValidator : IAddBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(AddBookingRequest request)
        {
            var result = new PdrValidationResult(true);

            if (StartTimeIsInPast(request, ref result))
                return result;

            if (DoctorAlreadyBooked(request, ref result))
                return result;

            return result;
        }

        public bool StartTimeIsInPast(AddBookingRequest request, ref PdrValidationResult result)
        {
            if (request.StartTime < DateTime.Now)
            {
                result.Errors.Add("Start time cannot be in the past.");
                result.PassedValidation = false;
                return true;
            }

            return false;
        }

        public bool DoctorAlreadyBooked(AddBookingRequest request, ref PdrValidationResult result)
        {
            if (_context.Order.Any(o => o.StartTime <= request.StartTime && o.EndTime >= request.StartTime))
            {
                result.Errors.Add("Doctor already has a booking scheduled over the requested time.");
                result.PassedValidation = false;
                return true;
            }

            return false;
        }
    }
}
