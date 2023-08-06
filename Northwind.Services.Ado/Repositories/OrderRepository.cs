using System.Data.Common;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Northwind.Services.Repositories;

namespace Northwind.Services.Ado.Repositories
{
    public sealed class OrderRepository : IOrderRepository
    {
        private readonly DbConnection context;

        public OrderRepository(DbProviderFactory dbProviderFactory, string connectionString)
        {
            this.context = ((SqliteFactory)dbProviderFactory).CreateConnection();
            this.context.ConnectionString = connectionString;
        }

        public async Task<long> AddOrderAsync(Order order)
        {
            await this.context.OpenAsync();
            var sqlTran = this.context.BeginTransaction();
            var command = this.context.CreateCommand();
            command.Transaction = sqlTran;
            try
            {
                command.CommandText = "INSERT INTO Shippers (ShipperID, CompanyName) VALUES (@IdShippers, @CompanyNameShippers)";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@IdShippers";
                parameter.Value = order.Shipper.Id;
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@CompanyNameShippers";
                parameter.Value = $"'{order.Shipper.CompanyName}'";
                _ = command.Parameters.Add(parameter);
                _ = await command.ExecuteNonQueryAsync();

                command.CommandText = "INSERT INTO Customers (CustomerID, CompanyName) VALUES (@IdCustomers, @CompanyName)";
                parameter = command.CreateParameter();
                parameter.ParameterName = "@IdCustomers";
                parameter.Value = $"'{order.Customer.Code.Code}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@CompanyName";
                parameter.Value = $"'{order.Customer.CompanyName}'";
                _ = command.Parameters.Add(parameter);
                _ = await command.ExecuteNonQueryAsync();

                command.CommandText = "INSERT INTO Orders (OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, " +
                "ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry) VALUES (@OrderID, @CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShippedDate, " +
                "@ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)";
                parameter = command.CreateParameter();
                parameter.ParameterName = "@OrderID";
                parameter.Value = $"{order.Id}";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@CustomerID";
                parameter.Value = $"'{order.Customer.Code.Code}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@EmployeeID";
                parameter.Value = $"{order.Employee.Id}";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@OrderDate";
                parameter.Value = $"'{order.OrderDate}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@RequiredDate";
                parameter.Value = $"'{order.RequiredDate}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShippedDate";
                parameter.Value = $"'{order.ShippedDate}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipVia";
                parameter.Value = $"{order.Shipper.Id}";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@Freight";
                parameter.Value = $"'{order.Freight}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipName";
                parameter.Value = $"'{order.ShipName}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipAddress";
                parameter.Value = $"'{order.ShippingAddress.Address.Replace("'", "`", StringComparison.Ordinal)}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipCity";
                parameter.Value = $"'{order.ShippingAddress.City}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipRegion";
                parameter.Value = $"'{order.ShippingAddress.Region}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipPostalCode";
                parameter.Value = $"'{order.ShippingAddress.PostalCode}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@ShipCountry";
                parameter.Value = $"'{order.ShippingAddress.Country}'";
                _ = command.Parameters.Add(parameter);
                _ = await command.ExecuteNonQueryAsync();

                command.CommandText = "select last_insert_rowid()";
                var answer = ((long?)await command.ExecuteScalarAsync()) ?? 0;

                foreach (var orderDate in order.OrderDetails.Where(orderDate => orderDate is not null))
                {
                    command.CommandText = $"INSERT INTO Categories (CategoryName) VALUES ('{orderDate.Product.Category}')";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = "select last_insert_rowid()";
                    var answerCategories = ((long?)await command.ExecuteScalarAsync()) ?? 0;

                    command.CommandText = $"INSERT INTO Suppliers (CompanyName) VALUES ('{orderDate.Product.Supplier.Replace("'", "`", StringComparison.Ordinal)}')";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = "select last_insert_rowid()";
                    var answerSuppliers = ((long?)await command.ExecuteScalarAsync()) ?? 0;

                    command.CommandText = $"INSERT INTO Products (ProductName, SupplierID, CategoryID) VALUES (" +
                                            $"'{orderDate.Product.ProductName}', {answerSuppliers}, {answerCategories})";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = $"INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, Discount) VALUES (" +
                        $"{answer}, {orderDate.Product.Id}, {orderDate.UnitPrice}, {orderDate.Quantity}, {orderDate.Discount})";
                    _ = await command.ExecuteNonQueryAsync();
                }

                await sqlTran.CommitAsync();
                await this.context.CloseAsync();
                return answer;
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                await this.context.CloseAsync();
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async Task<Order> GetOrderAsync(long orderId)
        {
            try
            {
                var order = await this.OrderAsync(orderId);
                if (order.Employee is null)
                {
                    throw new ArgumentOutOfRangeException($"{order}");
                }

                return order;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }

        public async Task<IList<Order>> GetOrdersAsync(int skip, int count)
        {
            try
            {
                CheckParametersOrders(skip, count);

                return await this.InternalOrdersAsync(skip, count);
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException(ex.Message, ex);
            }
        }

        public async Task RemoveOrderAsync(long orderId)
        {
            Order order = await this.OrderAsync(orderId);

            await this.context.OpenAsync();
            var dbcm = this.context.CreateCommand();

            dbcm.CommandText = "DELETE FROM Shippers WHERE ShipperID = @ShippersId";
            var parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@ShippersId";
            parameter.Value = orderId;
            _ = dbcm.Parameters.Add(parameter);
            _ = await dbcm.ExecuteNonQueryAsync();

            if (order.Customer is not null)
            {
                dbcm.CommandText = $"DELETE FROM Customers WHERE CustomerID = @CustomersId";
                parameter = dbcm.CreateParameter();
                parameter.ParameterName = "@CustomersId";
                parameter.Value = order.Customer.Code.Code;
                _ = dbcm.Parameters.Add(parameter);
                _ = await dbcm.ExecuteNonQueryAsync();
            }

            if (order.Employee is not null)
            {
                dbcm.CommandText = $"DELETE FROM Employees WHERE EmployeeID = @EmployeesId ";
                parameter = dbcm.CreateParameter();
                parameter.ParameterName = "@EmployeesId";
                parameter.Value = order.Employee.Id;
                _ = dbcm.Parameters.Add(parameter);
                _ = await dbcm.ExecuteNonQueryAsync();
            }

            dbcm.CommandText = "DELETE FROM OrderDetails WHERE OrderID = @OrderDetailsId";
            parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@OrderDetailsId";
            parameter.Value = orderId;
            _ = dbcm.Parameters.Add(parameter);
            _ = await dbcm.ExecuteNonQueryAsync();

            dbcm.CommandText = "DELETE FROM Orders WHERE OrderID = @OrdersId";
            parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@OrdersId";
            parameter.Value = orderId;
            _ = dbcm.Parameters.Add(parameter);

            _ = await dbcm.ExecuteNonQueryAsync();
            await this.context.CloseAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await this.context.OpenAsync();
            var sqlTran = this.context.BeginTransaction();
            var command = this.context.CreateCommand();

            try
            {
                command.CommandText = $"UPDATE Shippers SET CompanyName = @CompanyNameShippers" +
                    $" WHERE ShipperID = @IdShippers";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@IdShippers";
                parameter.Value = $"{order.Shipper.Id}";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@CompanyNameShippers";
                parameter.Value = $"'{order.Shipper.CompanyName.Replace("'", "`", StringComparison.Ordinal)}'";
                _ = command.Parameters.Add(parameter);
                _ = await command.ExecuteNonQueryAsync();

                command.CommandText = $"UPDATE Customers SET CompanyName = @CompanyName WHERE CustomerID = @IdCustomers";
                parameter = command.CreateParameter();
                parameter.ParameterName = "@IdCustomers";
                parameter.Value = $"'{order.Customer.Code.Code}'";
                _ = command.Parameters.Add(parameter);
                parameter.ParameterName = "@CompanyName";
                parameter.Value = $"'{order.Customer.CompanyName.Replace("'", "`", StringComparison.Ordinal)}'";
                _ = command.Parameters.Add(parameter);
                _ = await command.ExecuteNonQueryAsync();
                var typeDateTime = "MM.dd.yyyy";
                command.CommandText = $"UPDATE Orders SET CustomerID = '{order.Customer.Code.Code}', EmployeeID = {order.Employee.Id}, OrderDate = '{order.OrderDate.ToString(typeDateTime, new CultureInfo("en-US"))}', RequiredDate = '{order.RequiredDate.ToString(typeDateTime, new CultureInfo("en-US"))}', " +
                    $"ShippedDate = '{order.ShippedDate.ToString(typeDateTime, new CultureInfo("en-US"))}', ShipVia = {order.Shipper.Id}, Freight = '{order.Freight}', ShipName = '{order.ShipName}', ShipAddress = '{order.ShippingAddress.Address.Replace("'", "`", StringComparison.Ordinal)}', " +
                    $"ShipCity = '{order.ShippingAddress.City}', ShipRegion = '{order.ShippingAddress.Region}', ShipPostalCode = '{order.ShippingAddress.PostalCode}', " +
                    $"ShipCountry = '{order.ShippingAddress.Country}' WHERE OrderID = {order.Id}";
                _ = await command.ExecuteNonQueryAsync();

                command.CommandText = $"DELETE FROM OrderDetails WHERE OrderID = {order.Id}";
                _ = await command.ExecuteNonQueryAsync();

                foreach (var orderDate in order.OrderDetails.Where(orderDate => orderDate is not null))
                {
                    command.CommandText = $"UPDATE Categories SET CategoryName = '{orderDate.Product.Category}' " +
                        $"WHERE CategoryID = {orderDate.Product.CategoryId}";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = $"UPDATE Suppliers SET CompanyName = '{orderDate.Product.Supplier.Replace("'", "`", StringComparison.Ordinal)}'" +
                        $"WHERE SupplierID = {orderDate.Product.SupplierId}";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = $"UPDATE Products SET ProductName = '{orderDate.Product.ProductName}', " +
                        $"SupplierID = {orderDate.Product.SupplierId}, CategoryID = {orderDate.Product.CategoryId} " +
                        $"WHERE ProductID = {orderDate.Product.Id}";
                    _ = await command.ExecuteNonQueryAsync();

                    command.CommandText = $"INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, Discount) VALUES (" +
                        $"{order.Id}, {orderDate.Product.Id}, {orderDate.UnitPrice}, {orderDate.Quantity}, {orderDate.Discount})";
                    _ = await command.ExecuteNonQueryAsync();
                }

                await sqlTran.CommitAsync();
                await this.context.CloseAsync();
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                await this.context.CloseAsync();
                throw new RepositoryException(ex.Message, ex);
            }
        }

        private static void CheckParametersOrders(int skip, int count)
        {
            if (count < 1 || skip < 0)
            {
                throw new ArgumentOutOfRangeException($"{count} or {skip}");
            }
        }

        private async Task<IList<Order>> InternalOrdersAsync(int skip, int count)
        {
            await this.context.OpenAsync();
            var dbc = this.context.CreateCommand();
            List<Order> orders = new List<Order>();
            List<int> idsList = new List<int>();
            int[] idsArray = new int[count];

            dbc.CommandText = "SELECT OrderID FROM Orders";
            var db = await dbc.ExecuteReaderAsync();

            while (await db.ReadAsync())
            {
                idsList.Add(db.GetInt32(0));
            }

            idsList.CopyTo(skip, idsArray, 0, count);
            await db.CloseAsync();
            await this.context.CloseAsync();

            foreach (var orderId in idsArray)
            {
                orders.Add(await this.OrderAsync(orderId));
            }

            return orders;
        }

        private async Task<Order> OrderAsync(long orderId)
        {
            long employee = -1;
            long shipper = -1;
            Order order = new Order(orderId);
            await this.context.OpenAsync();
            var dbcm = this.context.CreateCommand();
            dbcm.CommandText = "SELECT RequiredDate, ShippedDate, ShipName, Freight, OrderDate, EmployeeId, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry, CustomerID, ShipVia FROM Orders WHERE OrderID = @Id";
            var parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = orderId;
            _ = dbcm.Parameters.Add(parameter);
            var dbr = await dbcm.ExecuteReaderAsync();
            while (await dbr.ReadAsync())
            {
                int index = 0;
                order.RequiredDate = dbr.GetDateTime(index++);
                order.ShippedDate = dbr.IsDBNull(index) ? DateTime.Now : dbr.GetDateTime(index);
                index++;
                order.ShipName = dbr.GetString(index++);
                order.Freight = dbr.GetDouble(index++);
                order.OrderDate = dbr.GetDateTime(index++);
                employee = dbr.GetInt64(index++);
                var shipAddress = dbr.GetString(index++).Replace("`", "'", StringComparison.Ordinal);
                var shipCity = dbr.GetString(index++);
                var shipRegion = dbr.IsDBNull(index) ? null : dbr.GetString(index);
                index++;
                var shipPostalCode = dbr.GetString(index++);
                var shipCountry = dbr.GetString(index++);
                order.ShippingAddress = new ShippingAddress(shipAddress, shipCity, shipRegion, shipPostalCode, shipCountry);
                order.Customer = new Customer(new CustomerCode(dbr.GetString(index)))
                {
                    CompanyName = await this.RequestForAddAsync("SELECT CompanyName FROM Customers WHERE CustomerID == @Id", dbr.GetString(index++)),
                };
                shipper = dbr.GetInt64(index);
            }

            await dbr.CloseAsync();

            dbcm = this.context.CreateCommand();
            dbcm.CommandText = "SELECT ProductID, UnitPrice, Quantity, Discount FROM OrderDetails WHERE OrderID = @Id";
            parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = order.Id;
            _ = dbcm.Parameters.Add(parameter);
            dbr = await dbcm.ExecuteReaderAsync();
            while (await dbr.ReadAsync())
            {
                int index = 0;
                var dbcmn = this.context.CreateCommand();
                dbcmn.CommandText = "SELECT ProductName, CategoryID, SupplierID FROM Products WHERE ProductID == @Id";
                parameter = dbcmn.CreateParameter();
                parameter.ParameterName = "@Id";
                parameter.Value = dbr.GetInt32(index);
                _ = dbcmn.Parameters.Add(parameter);
                var dbrm = await dbcmn.ExecuteReaderAsync();

                while (await dbrm.ReadAsync())
                {
                    int i = 0;
                    order.OrderDetails.Add(new OrderDetail(order)
                    {
                        Product = new Product(dbr.GetInt32(index++))
                        {
                            ProductName = dbrm.GetString(i++),
                            CategoryId = dbrm.GetInt32(i),
                            Category = await this.RequestForAddAsync("SELECT CategoryName FROM Categories WHERE CategoryID == @Id", $"{dbrm.GetInt32(i++)}"),
                            SupplierId = dbrm.GetInt32(i),
                            Supplier = await this.RequestForAddAsync("SELECT CompanyName FROM Suppliers WHERE SupplierID == @Id", $"{dbrm.GetInt32(i)}"),
                        },
                        UnitPrice = dbr.GetDouble(index++),
                        Quantity = dbr.GetInt32(index++),
                        Discount = dbr.GetInt32(index++),
                    });
                }
            }

            await dbr.CloseAsync();

            dbcm = this.context.CreateCommand();
            dbcm.CommandText = "SELECT FirstName, LastName, Country FROM Employees WHERE EmployeeID = @Id";
            parameter = dbcm.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = employee;
            _ = dbcm.Parameters.Add(parameter);
            dbr = await dbcm.ExecuteReaderAsync();
            while (await dbr.ReadAsync())
            {
                int index = 0;
                order.Employee = new Employee(employee)
                {
                    FirstName = dbr.GetString(index++),
                    LastName = dbr.GetString(index++),
                    Country = dbr.GetString(index),
                };
            }

            order.Shipper = new Shipper(shipper)
            {
                CompanyName = await this.RequestForAddAsync("SELECT CompanyName FROM Shippers WHERE ShipperID = @Id", $"{shipper}"),
            };

            await dbr.CloseAsync();
            await this.context.CloseAsync();

            return order;
        }

        private async Task<string> RequestForAddAsync(string commandText, string id)
        {
            var dbc = this.context.CreateCommand();
            dbc.CommandText = commandText;

            var parameter = dbc.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            _ = dbc.Parameters.Add(parameter);

            var dbr = await dbc.ExecuteReaderAsync();
            while (await dbr.ReadAsync())
            {
                commandText = dbr.GetString(0);
            }

            await dbr.CloseAsync();
            return commandText;
        }
    }
}
