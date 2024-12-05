using PSPBackend.Model;

public class BusinessService {
    private readonly BusinessRepository _businessRepository;

    public BusinessService(BusinessRepository businessRepository)
    {
        _businessRepository = businessRepository;
    }

    public List<BusinessModel> getBusinesses(BusinessGetDto businessGetDto)
    {
        var query = _businessRepository.getBusinesses(businessGetDto);

        var gottenBusinesses = query.Skip(businessGetDto.page_nr * businessGetDto.limit).Take(businessGetDto.limit).ToList();
        return gottenBusinesses;
    }

    public BusinessModel getBusinessById(int businessId)
    {
        var result = _businessRepository.getBusinessById(businessId);

        return result;
    }

    public int createBusiness(BusinessCreateDto newBusinessDto)
    {
        BusinessModel newBusinessModel = new BusinessModel();
        newBusinessModel.id = _businessRepository.GetNewBusinessId();

        //validation - we dont do that here
        if(newBusinessDto.name != null)
        {
            newBusinessModel.business_name = newBusinessDto.name;
        }
        if(newBusinessDto.address != null)
        {
            newBusinessModel.business_address = newBusinessDto.address;
        }
        if(newBusinessDto.phone != null)
        {
            newBusinessModel.phone = newBusinessDto.phone;
        }
        if(newBusinessDto.email != null)
        {
            newBusinessModel.email = newBusinessDto.email;
        }
        if(newBusinessDto.currency != null)
        {
            newBusinessModel.currency = newBusinessDto.currency;
        }


        var result = _businessRepository.createBusiness(newBusinessModel);
        return 0;
    }

    public int updateBusiness(int businessId, BusinessUpdateDto updatedBusinessDto)
    {
        var result = _businessRepository.updateBusiness(businessId, updatedBusinessDto);
        return result;
    }

    public int deleteBusiness(int businessId)
    {
        var result = _businessRepository.deleteBusiness(businessId);
        return 0;
    }
}