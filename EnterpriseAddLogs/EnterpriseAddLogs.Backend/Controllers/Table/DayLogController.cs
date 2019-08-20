using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using EnterpriseAddLogs.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server;
using EnterpriseAddLogs.Backend.Models;

namespace EnterpriseAddLogs.Backend.Controllers
{
    public class DayLogController : TableController<DayLog>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<DayLog>(context, Request);
        }

        // GET tables/DayLog
        public IQueryable<DayLog> GetAllDayLog()
        {
            return Query(); 
        }

        // GET tables/DayLog/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DayLog> GetDayLog(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/DayLog/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DayLog> PatchDayLog(string id, Delta<DayLog> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/DayLog
        public async Task<IHttpActionResult> PostDayLog(DayLog item)
        {
            DayLog current = await InsertAsync(item);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/DayLog/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDayLog(string id)
        {
             return DeleteAsync(id);
        }
    }
}
