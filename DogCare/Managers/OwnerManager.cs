using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare
{
    class OwnerManager
    {
        static OwnerManager defaultInstance = new OwnerManager();
        MobileServiceClient client;
        IMobileServiceTable<Owner> ownerTable;

        private OwnerManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

            this.ownerTable = client.GetTable<Owner>();
        }


        public static OwnerManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return ownerTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Dog>; }
        }

        public async Task<ObservableCollection<Owner>> GetOwnerItemsAsync(bool syncItems = false)
        {
            try
            {
                IEnumerable<Owner> items = await ownerTable.ToEnumerableAsync();

                return new ObservableCollection<Owner>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(Owner item)
        {
            if (item.Id == null)
            {
                await ownerTable.InsertAsync(item);
            }
            else
            {
                await ownerTable.UpdateAsync(item);
            }
        }

        public async Task<Owner> CheckIfOwnerAlreadyExists(string userName)
        {
            
            var items = await ownerTable
            .Where(owner => owner.UserName == userName)
            .ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items[0];
            
        }
        public async Task<Owner> CheckUserNameAndPassword(string userName,string password)
        {

            var items = await ownerTable
            .Where(owner => owner.UserName == userName)
            .Where(owner => owner.Password == password)
            .ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items[0];

        }
    }
}
