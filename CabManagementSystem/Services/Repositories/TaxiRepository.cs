using CabManagementSystem.AppContext;
using CabManagementSystem.Models;
using CabManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace CabManagementSystem.Services.Repositories
{
    public class TaxiRepository : TaxiContext, ITaxiRepository<TaxiModel>
    {

        /// <summary>
        /// adds data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public ExceptionModel Create(TaxiModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            if (item.BindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            var operation = AddBindTaxiDriver(item.BindTaxiDriver);
            if (operation != ExceptionModel.Successfull)
                return operation;
            Taxi.Add(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        /// <summary>
        /// removes data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public ExceptionModel Delete(TaxiModel item)
        {
            item = Taxi.FirstOrDefault(x => x.ID == item.ID);
            ChangeTracker.Clear();
            if (item is null)
                return ExceptionModel.VariableIsNull;

            if (item.BindTaxiDriver is null)
                return ExceptionModel.VariableIsNull;
            var operation = DeleteBindTaxiDriver(item.BindTaxiDriver);
            if (operation != ExceptionModel.Successfull)
                return operation;
            Taxi.Remove(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        public bool Exist(Guid id) => Taxi.Any(x => x.ID == id);

        public IEnumerable<TaxiModel> Get() => Taxi;

        public TaxiModel Get(Guid id) => Get(id);

        public TaxiModel Get(Expression<Func<TaxiModel, bool>> predicate) => Get(predicate);

        /// <summary>
        /// updates data of taxi in the database
        /// </summary>
        /// <param name="taxi"></param>
        public ExceptionModel Update(TaxiModel item)
        {
            if (item is null)
                return ExceptionModel.VariableIsNull;
            Taxi.Update(item);
            SaveChanges();
            return ExceptionModel.Successfull;
        }

        void ITaxiRepository<TaxiModel>.ChangeTracker() => ChangeTracker.Clear();
    }
}
