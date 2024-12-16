using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PSPBackend.Model;
public class BusinessRepository 
{
    private readonly AppDbContext _context;

    public BusinessRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<BusinessModel> getBusinesses(BusinessGetDto businessGetDto)
    {
        var query = _context.Business.AsQueryable();

        if(businessGetDto.name != null){
            query = query.Where(c => c.business_name == businessGetDto.name);
        }
        if(businessGetDto.address != null){
            query = query.Where(c => c.business_address == businessGetDto.address);
        }
        if(businessGetDto.phone != null){
            query = query.Where(c => c.phone == businessGetDto.phone);
        }
        if(businessGetDto.email != null){
            query = query.Where(c => c.email == businessGetDto.email);
        }
        if(businessGetDto.currency != null){
            query = query.Where(c => c.currency == businessGetDto.currency);
        }

        return query; 
    }

    public BusinessModel getBusinessById(int businessId)
    {
        BusinessModel? result = _context.Business.Single(c => c.id == businessId);

        if(result == null)
        {
            throw new KeyNotFoundException("Business with id " + businessId + " was not found");
        }
        return result;
    }

    public BusinessModel createBusiness(BusinessModel newBusiness)
    {
        _context.Business.Add(newBusiness);
        int rowsAffected = _context.SaveChanges(); 

        if(rowsAffected == 0)
        {
            throw new DbUpdateException("Failed to create business in database");
        }

        return newBusiness;
    }

    public BusinessModel updateBusiness(int businessId, BusinessUpdateDto businessUpdateDto)
    {
        BusinessModel business;
        try {
            business = getBusinessById(businessId);
        } catch(KeyNotFoundException){
            throw;
        }

        //reflection cant be used because model and request propery names differ

        if(businessUpdateDto.name != null)
        {
            business.business_name = businessUpdateDto.name;
        }
        if(businessUpdateDto.address != null)
        {
            business.business_address = businessUpdateDto.address;
        }
        if(businessUpdateDto.phone != null)
        {
            business.phone = businessUpdateDto.phone;
        }
        if(businessUpdateDto.email != null)
        {
            business.email = businessUpdateDto.email;
        }
        if(businessUpdateDto.currency != null)
        {
            business.currency = businessUpdateDto.currency;
        }

        int rowsAffected = _context.SaveChanges();
        if(rowsAffected == 0){
            throw new DbUpdateException("Failed to update business in database");
        }

        return business;
    }

    public void deleteBusiness(int businessId)
    {
        BusinessModel business;
        try {
            business = getBusinessById(businessId);
        } catch (KeyNotFoundException){
            throw;
        }
        
        _context.Remove(business);
    }

    public int GetNewBusinessId()
    {
        return _context.Business.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}