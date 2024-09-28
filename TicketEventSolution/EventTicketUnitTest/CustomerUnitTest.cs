using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEventBackEnd.Repositories.Customer;
using Moq;
using TicketEventBackEnd.Models.Customer;
using Castle.Core.Resource;
namespace EventTicketUnitTest
{
    public class CustomerUnitTest
    {
        [Fact]
        public void getAllCustomer_True()
        {
            //create a mockRepository for the adminRepository
            var mockRepository = new Mock<ICustomerRepository>();
            var customerList = new List<CustomerModel>
            {
                new CustomerModel
                {
                    CustomerId = 1,
                    FirstName = "Test",
                    LastName = "Testing",
                    Email = "testing@gmail.com",
                    Password = "testing1234"
                },
                new CustomerModel
                {
                    CustomerId = 2,
                    FirstName = "Test2",
                    LastName = "Testing2",
                    Email = "testing2@gmail.com",
                    Password = "testing12345"
                }
            };

            // Setup mock to return the list when getAllAdmin is called
            mockRepository.Setup(x => x.getAllCustomer()).Returns(customerList);

            // Act
            var results = mockRepository.Object.getAllCustomer();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count()); // Check the number of items returned
            Assert.Equal("testing@gmail.com", results.First().Email); // Check the first admin's email
            Assert.Equal("testing2@gmail.com", results.ElementAt(1).Email); // Check the second admin's email
        }
        [Fact]
        public void getAllCustomer_Empty()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customerList = new List<CustomerModel>
            {

            };
            mockRepository.Setup(x => x.getAllCustomer()).Returns(customerList);

            // Act
            var results = mockRepository.Object.getAllCustomer();

            // Assert
            Assert.Empty(results);

        }
        [Fact]
        public void getAllCustomer_False()
        {
            //create a mockRepository for the adminRepository
            var mockRepository = new Mock<ICustomerRepository>();
            var customerList = new List<CustomerModel>
            {
                  new CustomerModel
                {
                    CustomerId = 1,
                    FirstName = "Test",
                    LastName = "Testing",
                    Email = "testing@gmail.com",
                    Password = "testing1234"
                },
                new CustomerModel
                {
                    CustomerId = 2,
                    FirstName = "Test2",
                    LastName = "Testing2",
                    Email = "testing2@gmail.com",
                    Password = "testing12345"
                }
            };

            // Setup mock to return the list when getAllAdmin is called
            mockRepository.Setup(x => x.getAllCustomer()).Returns(customerList);

            // Act
            var results = mockRepository.Object.getAllCustomer();

            // Assert
            Assert.NotNull(results);
            Assert.NotEqual(3, results.Count()); // Check the number of items returned
            Assert.NotEqual("falseTesting@gmail.com", results.First().Email); // Check the first admin's email
            Assert.NotEqual("falseTesting2@gmail.com", results.ElementAt(1).Email); // Check the second admin's email
        }
        [Fact]
        public void getCustomerInfo_True()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            var results = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.Equal(1, results.CustomerId);
            Assert.Equal("testing@gmail.com", results.Email);
            Assert.Equal("testing1234", results.Password);
        }
        [Fact]
        public void getCustomerInfo_Empty()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {

            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            var results = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.Equal(0, results.CustomerId);
            Assert.Null(results.Email);
            Assert.Null(results.Password);
        }
        [Fact]
        public void getCustomerInfo_False()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            var results = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.NotEqual(2, results.CustomerId);
            Assert.NotEqual("falsetesting@gmail.com", results.Email);
            Assert.NotEqual("falsetesting1234", results.Password);
        }
        [Fact]
        public void addCustomer_True()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customerEmail = "testing@gmail.com";
            var customerPassword = "testing1234";
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            // Setup addCustomer method to add the customer
            mockRepository.Setup(repo => repo.addCustomer(It.IsAny<CustomerModel>()))
                .Callback<CustomerModel>(c =>
                {
                    customer = c;
                });

            // Setup getCustomerInfo to return the customer by email
            mockRepository.Setup(x => x.getCustomerInfo(It.IsAny<string>()))
                .Returns((string email) => email == customer.Email ? customer : null);

            mockRepository.Object.addCustomer(customer);
            var result = mockRepository.Object.getCustomerInfo(customerEmail);
            Assert.NotNull(result);
            Assert.Equal(customerEmail, result.Email);  // Check if the email matches
            Assert.Equal(customerPassword, result.Password);  // Check if the password matches
        }
        
        [Fact]
        public void addCustomer_False()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customerEmail = "testing@gmail.com";
            var customerPassword = "testing1234";
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            // Setup addCustomer method to add the customer
            mockRepository.Setup(repo => repo.addCustomer(It.IsAny<CustomerModel>()))
                .Callback<CustomerModel>(c =>
                {
                    customer = c;
                });

            // Setup getCustomerInfo to return the customer by email
            mockRepository.Setup(x => x.getCustomerInfo(It.IsAny<string>()))
                .Returns((string email) => email == customer.Email ? customer : null);

            mockRepository.Object.addCustomer(customer);
            var result = mockRepository.Object.getCustomerInfo(customerEmail);
            Assert.NotNull(result);
            Assert.NotEqual("falsetesting@gmail.com", result.Email);
            Assert.NotEqual("falsetesting1234", result.Password);
        }
        
        [Fact]
        public void deleteCustomer_True()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            mockRepository.Setup(x => x.deleteCustomer("testing@gmail.com"));
            var fetchedAdmin = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            mockRepository.Object.deleteCustomer("testing@gmail.com");
            Assert.NotNull(fetchedAdmin);
            mockRepository.Verify(x => x.deleteCustomer("testing@gmail.com"), Times.Once);
        }
        [Fact]
        public void deleteCustomer_False()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            mockRepository.Setup(x => x.deleteCustomer("testing@gmail.com"))
                          .Throws(new Exception("Delete failed"));
            var fetchedAdmin = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.NotNull(fetchedAdmin);
            var ex = Assert.Throws<Exception>(() => mockRepository.Object.deleteCustomer("testing@gmail.com"));
            Assert.Equal("Delete failed", ex.Message);
            mockRepository.Verify(x => x.deleteCustomer("testing@gmail.com"), Times.Once);
        }
        
        [Fact]
        public void updateCustomer_True()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            mockRepository.Setup(x => x.updateCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string, string, string>((firstname, lastname, email, password, target) =>
                {
                    if (customer.Email == target)
                    {
                        customer.FirstName = firstname;
                        customer.LastName = lastname;
                        customer.Email = email;
                        customer.Password = password;
                    }
                }
            );
            mockRepository.Object.updateCustomer("updateFirstName","updateLastName","updateEmail@gmail.com", "updatePassword", "testing@gmail.com");
            var result = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.NotNull(result);
            Assert.Equal("updateEmail@gmail.com", result.Email);
            Assert.Equal("updatePassword", result.Password);
        }
        [Fact]
        public void updateCustomer_False()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var customer = new CustomerModel()
            {
                CustomerId = 1,
                FirstName = "Test",
                LastName = "Testing",
                Email = "testing@gmail.com",
                Password = "testing1234"
            };
            mockRepository.Setup(x => x.getCustomerInfo("testing@gmail.com")).Returns(customer);
            mockRepository.Setup(x => x.updateCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string, string, string, string>((firstname, lastname, email, password, target) =>
            {
                if (customer.Email == target)
                {
                    customer.FirstName = firstname;
                    customer.LastName = lastname;
                    customer.Email = email;
                    customer.Password = password;
                }
            }
        );
            mockRepository.Object.updateCustomer("updateFirstName", "updateLastName", "updateEmail@gmail.com", "updatePassword", "testing@gmail.com");
            var result = mockRepository.Object.getCustomerInfo("testing@gmail.com");
            Assert.NotNull(result);
            Assert.NotEqual("falseEmail@gmail.com", result.Email);
            Assert.NotEqual("falsePassword", result.Password);
        }
        
        
    }
}
