using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;

namespace PDR.PatientBooking.Service.BookingServices
{
    public interface IBookingService
    {
        GetPatientNextAppointmentResponse GetPatientNextAppointment(long identificationNumber);

        AddBookingResponse AddBooking(AddBookingRequest request);
    }
}
