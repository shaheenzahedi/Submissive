using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mfr.Core.Model;
using Mfr.Services.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Mfr.Test
{
    interface IGenericTest<TEntity> where TEntity : class
    {
        ApplicationDbContext Context { get; set; }
        IRepository<TEntity> Target { get; set; }
        void InitializeTest();
        void GetAllTest();
        void GetByIdTest();
        void RemoveSingleModelTest();
        void RemoveRangeTest();
        void AddSingleModelTest();
        void AddRangeTest();

        void FindTest();
    }
}
