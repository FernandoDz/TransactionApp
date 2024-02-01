using AutoMapper;
using Transaction.WebAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreditCard, CreditCardResponse>()
            .ForMember(dest => dest.Response, opt => opt.MapFrom(src => new List<CreditCard> { src }))
            .ForMember(dest => dest.Response, opt => opt.MapFrom(src => HideCreditCardNumber(src.Number)));

        CreateMap<CreditCard, CreditCard>()  
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => HideCreditCardNumber(src.Number)));
        
    }

    private string HideCreditCardNumber(string fullNumber)
    {
        if (!string.IsNullOrEmpty(fullNumber) && fullNumber.Length >= 4)
        {
            string lastFourDigits = fullNumber.Substring(fullNumber.Length - 4);
            return new string('*', fullNumber.Length - 4) + lastFourDigits;
        }

        return fullNumber;
    }
}



