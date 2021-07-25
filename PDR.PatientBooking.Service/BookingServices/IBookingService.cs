using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using System;

namespace PDR.PatientBooking.Service.BookingServices
{
    public interface IBookingService
    {
        GetPatientNextBookingResponse GetPatientNextBooking(long identificationNumber);

        AddBookingResponse AddBooking(AddBookingRequest request);

        void CancelBooking(Guid id);
    }
}
