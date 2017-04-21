using DogCare.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare.Managers
{
    class TripsManager
    {
        static TripsManager defaultInstance = new TripsManager();
        MobileServiceClient client;
        IMobileServiceTable<Trips> tripsTable;

        private TripsManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.tripsTable = client.GetTable<Trips>();
        }

        public static TripsManager DefaultManager
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
            get { return tripsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Trips>; }
        }

        public async Task<ObservableCollection<Trips>> GetTripsItemsAsync(bool syncItems = false)
        {
            try
            {
                IEnumerable<Trips> items = await tripsTable.ToEnumerableAsync();

                return new ObservableCollection<Trips>(items);
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

        public async Task SaveTaskAsync(Trips item)
        {
            if (item.Id == null)
            {
                await tripsTable.InsertAsync(item);
            }
            else
            {
                await tripsTable.UpdateAsync(item);
            }
        }
    }
}
