using System;
using System.Collections.Generic;
using System.Linq;
using ProductsWithRouting.Models;

namespace ProductsWithRouting.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> products;
        private readonly object syncRoot = new object();

        public ProductService(Data data)
        {
            products = data.Products;
        }

        public IEnumerable<Product> GetAll(int? filterId, string filterName)
        {
            lock (syncRoot)
            {
                IEnumerable<Product> result = products;

                if (filterId.HasValue)
                {
                    result = result.Where(product => product.Id == filterId.Value);
                }

                if (!string.IsNullOrWhiteSpace(filterName))
                {
                    result = result.Where(product =>
                        product.Name != null && product.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase));
                }

                return result.ToList();
            }
        }

        public Product GetById(int id)
        {
            lock (syncRoot)
            {
                return products.FirstOrDefault(product => product.Id == id);
            }
        }

        public int GetNextId()
        {
            lock (syncRoot)
            {
                return products.Count == 0 ? 1 : products.Max(product => product.Id) + 1;
            }
        }

        public void Add(Product product)
        {
            lock (syncRoot)
            {
                product.Id = GetNextId();
                products.Add(product);
            }
        }

        public bool Update(Product product)
        {
            lock (syncRoot)
            {
                var existing = products.FirstOrDefault(p => p.Id == product.Id);
                if (existing == null)
                {
                    return false;
                }

                existing.Name = product.Name;
                existing.Description = product.Description;
                return true;
            }
        }

        public bool Delete(int id)
        {
            lock (syncRoot)
            {
                return products.RemoveAll(product => product.Id == id) > 0;
            }
        }
    }
}
