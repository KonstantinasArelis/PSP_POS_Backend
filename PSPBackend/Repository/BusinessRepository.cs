using Microsoft.AspNetCore.Identity;
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
        BusinessModel? gottenBusiness = _context.Business.Single(c => c.id == businessId);
        return gottenBusiness;
    }

    public int createBusiness(BusinessModel newBusiness)
    {
        _context.Business.Add(newBusiness);
        int rowsAffected = _context.SaveChanges(); 

        return rowsAffected;
    }

    public int updateBusiness(int businessId, BusinessUpdateDto businessUpdateDto)
    {
        BusinessModel business = getBusinessById(businessId);

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

        return _context.SaveChanges();
    }

    public int deleteBusiness(int businessId)
    {
        BusinessModel business = getBusinessById(businessId);
        _context.Remove(business);
        
        return 0;
    }

    public int GetNewBusinessId()
    {
        return _context.Business.Select(o => o.id).ToList().OrderByDescending(a => a).FirstOrDefault() + 1;
    }
}