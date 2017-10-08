using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using refactor_me.Models;

namespace refactor_me.Repositories
{
    public class ProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Products GetAll()
        {
            return new Products { Items = _connection.Query<Product>("select * from product") };
        }

        public Products GetByName(string name)
        {
            return new Products
            {
                Items = _connection.Query<Product>("select * from product where lower(name) like @name", new { name = '%' + name + '%'})
            };
        }

        public void Add(Product product)
        {
            _connection.ExecuteScalar(
                "insert into product (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)",
                new {product.Id, product.Name, product.Description, product.Price, product.DeliveryPrice});
        }

        public Product GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<Product>("select * from product where id = @id", new { id });
        }

        public void Update(Guid id, Product product)
        {
            _connection.ExecuteScalar(
                "update product set name = @productName, description = @productDescription, price = @productPrice, deliveryprice = @productDeliveryPrice where id = @id",
                new {productName = product.Name, productDescription = product.Description, productPrice= product.Price, productDeliveryPrice = product.DeliveryPrice, id});
        }

        public void Delete(Guid id)
        {
            var productOptions = GetOptions(id);

            foreach (var productOption in productOptions)
            {
                DeleteOption(productOption.Id);
            }

            _connection.ExecuteScalar("delete from product where id = @id", new { id });
        }

        public void DeleteOption(Guid id)
        {
            _connection.ExecuteScalar("delete from productoption where id = @id", new {id});
        }

        public void AddOption(Guid productId, ProductOption productOption)
        {
            _connection.ExecuteScalar(
                "insert into productoption (id, productid, name, description) " +
                "values (@Id, @productId, @Name, @Description)", new
                {
                    productOption.Id,
                    productId,
                    productOption.Name,
                    productOption.Description
                });
        }

        public IEnumerable<ProductOption> GetOptions(Guid productId)
        {
            return _connection.Query<ProductOption>(
                "select * from productoption where productid = @productId", new {productId});
        }

        public void UpdateOption(Guid optionId, ProductOption productOption)
        {
            _connection.ExecuteScalar(
                "update productoption set name = @Name, description = @Description where id = @optionId",
                new {productOption.Name, productOption.Description, optionId});
        }

        public ProductOption GetOption(Guid id)
        {
            return _connection.QuerySingleOrDefault<ProductOption>("select * from productoption where id = @id",
                new {id});
        }
    }
}