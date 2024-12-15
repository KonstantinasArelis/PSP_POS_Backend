using Microsoft.EntityFrameworkCore;
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
        BusinessModel result;

        try {
            result = _businessRepository.getBusinessById(businessId);
        } catch (KeyNotFoundException){
            throw;
        }

        return result;
    }

    public BusinessModel createBusiness(BusinessCreateDto newBusinessDto)
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


        BusinessModel result;

        try {
            result = _businessRepository.createBusiness(newBusinessModel);
        } catch (DbUpdateException){
            throw;
        }

        return result;
    }

    public BusinessModel updateBusiness(int businessId, BusinessUpdateDto updatedBusinessDto)
    {
        BusinessModel result;

        try {
            result = _businessRepository.updateBusiness(businessId, updatedBusinessDto);
        } catch(DbUpdateException){
            throw;
        }

        return result;
    }

    public void deleteBusiness(int businessId)
    {
        _businessRepository.deleteBusiness(businessId);
    }
}