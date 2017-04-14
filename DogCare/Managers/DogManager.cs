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
    class DogManager
    {
        static DogManager defaultInstance = new DogManager();
        MobileServiceClient client;
        IMobileServiceTable<Dog> dogTable;

        private DogManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

            this.dogTable = client.GetTable<Dog>();
        }

        public static DogManager DefaultManager
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
            get { return dogTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Dog>; }
        }

        public async Task<ObservableCollection<Dog>> GetDogItemsAsync(bool syncItems = false)
        {
            try
            {
                IEnumerable<Dog> items = await dogTable.ToEnumerableAsync();

                return new ObservableCollection<Dog>(items);
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

        public async Task SaveTaskAsync(Dog item)
        {
            if (item.Id == null)
            {
                await dogTable.InsertAsync(item);
            }
            else
            {
                await dogTable.UpdateAsync(item);
            }
        }
        public async Task<List<Dog>> CheckOwnerDogs(string userName)
        {

            var items = await dogTable
            .Where(dog => dog.Owner == userName)
            .ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items;

        }

    }
}
