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

        public IEnumerable<Product> GetAll()
        {
            return _connection.Query<Product>("select * from product");
        }

        public IEnumerable<Product> GetByName(string name)
        {
            return _connection.Query<Product>($"select * from product where lower(name) like '%{name.ToLower()}%'");
        }

        public void Add(Product product)
        {
            _connection.ExecuteScalar(
                $"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})");
        }

        public Product GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<Product>("select * from product where id = @id", new { id });
        }

        public void Update(Product product)
        {
            _connection.ExecuteScalar(
                $"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'");
        }

        public void Delete(Guid id)
        {
            _connection.ExecuteScalar($"delete from product where id = '{id}'");
        }
    }
}