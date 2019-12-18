using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concreate
{
    public class EFPhoneRepository : IPhoneRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Phone> Phones
        {
            get { return context.Phones; }
        }

        public void SavePhone(Phone phone)
        {
            if (phone.PhoneId == 0)
            {
                context.Phones.Add(phone);
            }
            else
            {
                Phone dbEntry = context.Phones.Find(phone.PhoneId);
                if (dbEntry != null)
                {
                    dbEntry.Mark = phone.Mark;
                    dbEntry.Model = phone.Model;
                    dbEntry.Description = phone.Description;
                    dbEntry.Category = phone.Category;
                    dbEntry.Price = phone.Price;
                    dbEntry.ImageData = phone.ImageData;
                    dbEntry.ImageMimeType = phone.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Phone DeletePhone(int phoneId)
        {
            Phone dbEntry = context.Phones.Find(phoneId);
            if (dbEntry!=null)
            {
                context.Phones.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
